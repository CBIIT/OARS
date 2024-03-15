using TheradexPortal.Data.Models;
using TheradexPortal.Data.Services.Abstract;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
namespace TheradexPortal.Data.Services
{
    public class ProtocolEDCFormService : BaseService, IProtocolEDCFormService
    {
        private readonly IErrorLogService _errorLogService;
        private readonly NavigationManager _navManager;

        public ProtocolEDCFormService(IDbContextFactory<ThorDBContext> dbFactory, IErrorLogService errorLogService, NavigationManager navigationManager) : base(dbFactory)
        {
            _errorLogService = errorLogService;
            _navManager = navigationManager;
        }
        public async Task<IList<ProtocolEDCForm>> GetProtocolEDCFormsByProtocolMappingId(int protocolMappingId) {
            return await context.ProtocolEDCForms.Where(p=>p.ProtocolMappingId == protocolMappingId).ToListAsync();
        }

        public async Task<bool> SaveProtocolEDCForm(ProtocolEDCForm protocolEDCForm)
        {
            try
            {
                DateTime currentDateTime = DateTime.UtcNow;
                protocolEDCForm.UpdateDate = currentDateTime;

                ProtocolEDCForm currentProtocolEDCForm = context.ProtocolEDCForms.Where(p => p.ProtocolEDCFormId == protocolEDCForm.ProtocolEDCFormId).FirstOrDefault();

                if (currentProtocolEDCForm == null || protocolEDCForm.CreateDate == null)
                {
                    protocolEDCForm.CreateDate = currentDateTime;
                    context.Add(protocolEDCForm);
                }
                else
                {
                    currentProtocolEDCForm.UpdateDate = currentDateTime;
                    context.Update(currentProtocolEDCForm);
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
