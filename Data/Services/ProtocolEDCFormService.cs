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
        public async Task<IList<ProtocolEDCForm>> GetFormsForMappingId(int protocolMappingId) {
            return await context.ProtocolEDCForm.Where(p=>p.ProtocolMappingId == protocolMappingId).ToListAsync();
        }
        public async Task<bool> SaveProtocolEDCForm(ProtocolEDCForm protocolEDCForm)
        {
            try
            {
                DateTime currentDateTime = DateTime.UtcNow;
                protocolEDCForm.UpdatedDate = currentDateTime;

                ProtocolEDCForm currentProtocolEDCForm = context.ProtocolEDCForm.Where(p => p.ProtocolEDCFormId == protocolEDCForm.ProtocolEDCFormId).FirstOrDefault();

                if (currentProtocolEDCForm == null || protocolEDCForm.CreateDate == null)
                {
                    protocolEDCForm.CreateDate = currentDateTime;
                    context.Add(protocolEDCForm);
                }
                else
                {
                    currentProtocolEDCForm.UpdatedDate = currentDateTime;
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

        public async Task<IList<ProtocolEDCForm>> GetProtocolEDCFormsByProtocolMappingId(int protocolMappingId) {
            return await context.ProtocolEDCForms.Where(p=>p.ProtocolMappingId == protocolMappingId).ToListAsync();
        }

        public async Task<IList<int>> GetFormIdsForMappingId(int mappingId)
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
        public async Task<bool> DeleteProtocolEDCFormId(int protocolEDCFormId)
        {
            try
            {
                context.ProtocolEDCForm.Remove(context.ProtocolEDCForm.Where(f => f.ProtocolEDCFormId == protocolEDCFormId).FirstOrDefault());
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
