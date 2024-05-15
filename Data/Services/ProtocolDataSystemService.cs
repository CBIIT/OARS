using TheradexPortal.Data.Models;
using TheradexPortal.Data.Services.Abstract;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols.WsTrust;
namespace TheradexPortal.Data.Services
{
    public class ProtocolDataSystemService : BaseService, IProtocolDataSystemService
    {
        private readonly IErrorLogService _errorLogService;
        private readonly NavigationManager _navManager;

        public ProtocolDataSystemService(IDatabaseConnectionService databaseConnectionService, IErrorLogService errorLogService, NavigationManager navigationManager) : base(databaseConnectionService)
        {
            _errorLogService = errorLogService;
            _navManager = navigationManager;
        }
        public async Task<IList<ProtocolDataSystem>> GetProtocolDataSystems() {
            return await context.ProtocolDataSystem.OrderBy(c => c.SortOrder).ToListAsync();
        }

        public async Task<bool> SaveProtocolDataSystem(ProtocolDataSystem protocolDataSystem)
        {
            try
            {
                DateTime currentDateTime = DateTime.UtcNow;
                protocolDataSystem.UpdateDate = currentDateTime;

                ProtocolDataSystem currentProtocolDataSystem = context.ProtocolDataSystem.Where(p => p.ProtocolDataSystemId == protocolDataSystem.ProtocolDataSystemId).FirstOrDefault();

                if (currentProtocolDataSystem == null || protocolDataSystem.CreateDate == null)
                {
                    protocolDataSystem.CreateDate = currentDateTime;
                    protocolDataSystem.IsActive = true;
                    context.Add(protocolDataSystem);
                }
                else
                {
                    currentProtocolDataSystem.DataSystemName = protocolDataSystem.DataSystemName;
                    currentProtocolDataSystem.SortOrder = protocolDataSystem.SortOrder;
                    currentProtocolDataSystem.IsActive = protocolDataSystem.IsActive;
                    currentProtocolDataSystem.UpdateDate = currentDateTime;
                    context.Update(currentProtocolDataSystem);
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
    }
}
