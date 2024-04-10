using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using TheradexPortal.Data.Models;
using TheradexPortal.Data.Services.Abstract;

namespace TheradexPortal.Data.Services
{
    public class ProtocolDiseaseService : BaseService, IProtocolDiseaseService
    {
        private readonly IErrorLogService _errorLogService;
        private readonly NavigationManager _navManager;
        public ProtocolDiseaseService(IDbContextFactory<ThorDBContext> context, IErrorLogService errorLogService, NavigationManager navigationManager) : base(context)
        {
            _errorLogService = errorLogService;
            _navManager = navigationManager;
        }

        public async Task<bool> SaveProtocolDisease(ProtocolDisease protocolDisease)
        {
            try
            {
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
