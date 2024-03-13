using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using TheradexPortal.Data.Models;
using TheradexPortal.Data.Services.Abstract;

namespace TheradexPortal.Data.Services
{
    public class ProtocolPhaseService: BaseService, IProtocolPhaseService
    {
        private readonly IErrorLogService _errorLogService;
        private readonly NavigationManager _navManager;
        public ProtocolPhaseService(IDbContextFactory<ThorDBContext> dbFactory, IErrorLogService errorLogService, NavigationManager navigationManager) : base(dbFactory)
        {
            _errorLogService = errorLogService;
            _navManager = navigationManager;
        }

        public async Task<IList<ProtocolPhase>> GetProtocolMappingPhases(int protocolMapping)
        {
            var protocolPhases = await context.ProtocolPhases.Where(p => p.ProtocolMappingId == protocolMapping).ToListAsync();

            return protocolPhases;
        }

        public async Task<bool> SaveProtocolPhase(ProtocolPhase protocolPhase)
        {
            try
            {
                DateTime currentDateTime = DateTime.UtcNow;
                protocolPhase.UpdateDate = currentDateTime;
                protocolPhase.CreateDate = currentDateTime;

                context.Add(protocolPhase);

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
