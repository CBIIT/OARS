using TheradexPortal.Data.Models;
using TheradexPortal.Data.Services.Abstract;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
namespace TheradexPortal.Data.Services
{
    public class ProtocolDataSystemService : BaseService, IProtocolDataSystemService
    {
        private readonly IErrorLogService _errorLogService;
        private readonly NavigationManager _navManager;

        public ProtocolDataSystemService(IDbContextFactory<ThorDBContext> dbFactory, IErrorLogService errorLogService, NavigationManager navigationManager) : base(dbFactory)
        {
            _errorLogService = errorLogService;
            _navManager = navigationManager;
        }
        public async Task<IList<ProtocolDataSystem>> GetProtocolDataSystems() {
            return await context.ProtocolDataSystem.OrderBy(c => c.SortOrder).ToListAsync();
        }

        public async Task<bool> SaveProtocolDataSystem(ProtocolDataSystem protocolDataSystem)
        {
            DateTime currentDateTime = DateTime.UtcNow;

            if (protocolDataSystem.CreateDate == null)
                protocolDataSystem.CreateDate = currentDateTime;
            try
            {
                protocolDataSystem.UpdateDate = currentDateTime;
                context.Add(protocolDataSystem);

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
