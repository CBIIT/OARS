using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using OARS.Data.Models;
using OARS.Data.Services.Abstract;

namespace OARS.Data.Services
{
    public class ProtocolDiseaseService : BaseService, IProtocolDiseaseService
    {
        private readonly IErrorLogService _errorLogService;
        private readonly NavigationManager _navManager;
        public ProtocolDiseaseService(IDatabaseConnectionService databaseConnectionService, IErrorLogService errorLogService, NavigationManager navigationManager) : base(databaseConnectionService)
        {
            _errorLogService = errorLogService;
            _navManager = navigationManager;
        }

        public async Task<bool> SaveProtocolDisease(ProtocolDisease protocolDisease)
        {
            try
            {
                int? statusId = context.ProtocolMapping.Where(x => x.ProtocolMappingId == protocolDisease.ProtocolMappingId).Select(x => x.ProtocolMappingStatusId).FirstOrDefault();
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
                protocolDisease.UpdatedDate = currentDateTime;

                ProtocolDisease currentProtocolDisease = context.ProtocolDiseases.Where(p => p.ProtocolDiseaseId == protocolDisease.ProtocolDiseaseId).FirstOrDefault();

                if (currentProtocolDisease == null || currentProtocolDisease.CreateDate == null)
                {
                    protocolDisease.CreateDate = currentDateTime;
                    context.Add(protocolDisease);
                }
                else
                {
                    currentProtocolDisease.UpdatedDate = currentDateTime;
                    context.Update(currentProtocolDisease);
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

        public async Task<IList<ProtocolDisease>> GetProtocolDiseaseByProtocolMappingId(int protocolMappingId) {
            return await context.ProtocolDiseases.Where(p=>p.ProtocolMappingId == protocolMappingId).ToListAsync();
        }

        public async Task<bool> DeleteProtocolDisease(int ProtocolDiseaseId)
        {
            try
            {
                context.ProtocolDiseases.Remove(context.ProtocolDiseases.Where(f => f.ProtocolDiseaseId == ProtocolDiseaseId).FirstOrDefault());
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
