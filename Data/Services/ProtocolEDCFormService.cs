using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using TheradexPortal.Data.Models;
using TheradexPortal.Data.Services.Abstract;

namespace TheradexPortal.Data.Services
{
    public class ProtocolEDCFormService : BaseService, IProtocolEDCFormService
    {
        private readonly IErrorLogService _errorLogService;
        private readonly NavigationManager _navManager;
        public ProtocolEDCFormService(IDbContextFactory<ThorDBContext> context, IErrorLogService errorLogService, NavigationManager navigationManager) : base(context)
        {
            _errorLogService = errorLogService;
            _navManager = navigationManager;
        }

        public async Task<List<int>> GetFormIdsForMappingId(int mappingId)
        {
            List<int> formIds = new List<int>();
            try
            {
                formIds = await context.ProtocolEDCForm.Where(f => f.ProtocolMappingId == mappingId).Select(f => f.ProtocolEDCFormId).ToListAsync();
                return formIds;
            }
            catch (Exception ex)
            {
                await _errorLogService.SaveErrorLogAsync(0, _navManager.Uri, ex.InnerException, ex.Source, ex.Message, ex.StackTrace);
                return formIds;
            }
        }

        public async Task<bool> BulkSaveForms(List<ProtocolEDCForm> forms)
        {
            try
            {
                context.AddRange(forms);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                await _errorLogService.SaveErrorLogAsync(0, _navManager.Uri, ex.InnerException, ex.Source, ex.Message, ex.StackTrace);
                return false;
            }
        }

        public async Task<bool> DeleteAllFormsForMappingId(int mappingId)
        {
            try
            {
                context.RemoveRange(context.ProtocolEDCForm.Where(f => f.ProtocolMappingId == mappingId));
                await context.SaveChangesAsync();
                return true;
            } catch (Exception ex)
            {
                await _errorLogService.SaveErrorLogAsync(0, _navManager.Uri, ex.InnerException, ex.Source, ex.Message, ex.StackTrace);
                return false;
            }
        }
    }
}
