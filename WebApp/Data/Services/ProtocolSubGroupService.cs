using Microsoft.EntityFrameworkCore;
using OARS.Data.Models;
using OARS.Data.Services.Abstract;

namespace OARS.Data.Services
{
    public class ProtocolSubGroupService: BaseService, IProtocolSubGroupService
    {
        private readonly IErrorLogService _errorLogService;
        public ProtocolSubGroupService(IDatabaseConnectionService databaseConnectionService, IErrorLogService errorLogService) : base(databaseConnectionService)
        {
            _errorLogService = errorLogService;
        }

        public async Task<List<ProtocolSubGroup>> GetSubGroupsByMappingId(int mappingId)
        {
            return await context.ProtocolSubGroups.Where(x => x.ProtocolMappingId == mappingId).ToListAsync();
        }

        public async Task<bool> SaveSubGroup(ProtocolSubGroup subgroup, int mappingId)
        {
            try
            {
                int? statusId = context.ProtocolMapping.Where(x => x.ProtocolMappingId == mappingId).Select(x => x.ProtocolMappingStatusId).FirstOrDefault();
                if (statusId != null)
                {
                    string? statusText = context.ProtocolMappingStatus.Where(x => x.ProtocolMappingStatusId == statusId).Select(x => x.StatusName).FirstOrDefault();
                    if (statusText != "Active")
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
                ProtocolSubGroup currSubGroup = context.ProtocolSubGroups.FirstOrDefault(x => x.ProtocolSubGroupId == subgroup.ProtocolSubGroupId);
                if (currSubGroup == null || subgroup.CreateDate == null)
                {
                    subgroup.ProtocolMappingId = mappingId;
                    subgroup.CreateDate = DateTime.Now;
                    subgroup.UpdateDate = DateTime.Now;
                    context.ProtocolSubGroups.Add(subgroup);
                }
                else
                {
                    currSubGroup.UpdateDate = DateTime.Now;
                    currSubGroup.ProtocolMappingId = mappingId;
                    currSubGroup.SubGroupCode = subgroup.SubGroupCode;
                    currSubGroup.Description = subgroup.Description;

                    context.ProtocolSubGroups.Update(subgroup);
                }
                await context.SaveChangesAsync();
                return true;
            } catch (Exception ex)
            {
                await _errorLogService.SaveErrorLogAsync(0, "", ex.InnerException, ex.Source, ex.Message, ex.StackTrace);
                return false;
            }

        }

        public async Task<bool> DeleteSubGroup(ProtocolSubGroup subgroup)
        {
            try
            {
                context.ProtocolSubGroups.Remove(subgroup);
                await context.SaveChangesAsync();
                return true;
            } catch (Exception ex)
            {
                await _errorLogService.SaveErrorLogAsync(0, "", ex.InnerException, ex.Source, ex.Message, ex.StackTrace);
                return false;
            }
        }
    }
}
