using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using TheradexPortal.Data.Models;
using TheradexPortal.Data.Services.Abstract;

namespace TheradexPortal.Data.Services
{
    public class ProtocolTACService : BaseService, IProtocolTACService
    {
        private readonly IErrorLogService _errorLogService;
        private readonly NavigationManager _navManager;
        public ProtocolTACService(IDatabaseConnectionService databaseConnectionService, IErrorLogService errorLogService, NavigationManager navigationManager) : base(databaseConnectionService)
        {
            _errorLogService = errorLogService;
            _navManager = navigationManager;
        }

        public async Task<bool> SaveProtocolTAC(ProtocolTac protocolTac)
        {
            try
            {
                int? statusId = context.ProtocolMapping.Where(x => x.ProtocolMappingId == protocolTac.ProtocolMappingId).Select(x => x.ProtocolMappingStatusId).FirstOrDefault();
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
                DateTime currentDateTime = DateTime.UtcNow;
                protocolTac.UpdatedDate = currentDateTime;

                ProtocolTac currentProtocolTac = context.ProtocolTacs.Where(p => p.ProtocolTacId == protocolTac.ProtocolTacId).FirstOrDefault();

                if (currentProtocolTac == null || currentProtocolTac.CreateDate == null)
                {
                    protocolTac.CreateDate = currentDateTime;
                    context.Add(protocolTac);
                }
                else
                {
                    currentProtocolTac.UpdatedDate = currentDateTime;
                    context.Update(currentProtocolTac);
                }

                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                await _errorLogService.SaveErrorLogAsync(0, _navManager.Uri, ex.InnerException, ex.Source, ex.Message, ex.StackTrace);
                return false;
            }
        }    

        public async Task<IList<ProtocolTac>> GetProtocolTACByProtocolMappingId(int protocolMappingId) {
            return await context.ProtocolTacs.Where(p=>p.ProtocolMappingId == protocolMappingId).ToListAsync();
        }

        public async Task<bool> DeleteProtocolTAC(int protocolTacId)
        {
            try
            {
                context.ProtocolTacs.Remove(context.ProtocolTacs.Where(f => f.ProtocolTacId == protocolTacId).FirstOrDefault());
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                await _errorLogService.SaveErrorLogAsync(0, _navManager.Uri, ex.InnerException, ex.Source, ex.Message, ex.StackTrace);
                return false;
            }
        }
    }
}
