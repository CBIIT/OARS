using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TheradexPortal.Data.Models;
using TheradexPortal.Data.Services.Abstract;

namespace TheradexPortal.Data.Services
{
    public class ProtocolFormMappingService : BaseService, IProtocolFormMappingService
    {
        private readonly IErrorLogService _errorLogService;
        private readonly NavigationManager _navManager;
        public ProtocolFormMappingService(IDatabaseConnectionService databaseConnectionService, IErrorLogService errorLogService, NavigationManager navigationManager) : base(databaseConnectionService)
        {
            _errorLogService = errorLogService;
            _navManager = navigationManager;
        }

        public async Task<IList<ProtocolFormMapping>> GetProtocolFormMappings()
        {
            return await context.ProtocolFormMappings.ToListAsync();
        }

        public async Task<IList<ProtocolFormMapping>> GetProtocolFormMappings(int formId)
        {
            return await context.ProtocolFormMappings.Where(x => x.ProtocolEDCFormId == formId).ToListAsync();
        }

        public async Task<ProtocolFormMapping> GetProtocolFormMapping(int id)
        {
            var formMapping = await context.ProtocolFormMappings.FirstOrDefaultAsync(x => x.ProtocolFormMappingId == id);
            return formMapping;
        }

        public async Task<IList<ProtocolFormMapping>> GetProtocolFormMappingsForCategory(int categoryId)
        {
            return await context.ProtocolFormMappings.Where(x => x.ProtocolCategoryId == categoryId).ToListAsync();
        }

        public async Task<bool> SaveProtocolFormMapping(ProtocolFormMapping protocolFormMapping)
        {
            try
            {
                DateTime currentDateTime = DateTime.UtcNow;

                ProtocolFormMapping currentMapping = context.ProtocolFormMappings.FirstOrDefault(x => x.ProtocolFormMappingId == protocolFormMapping.ProtocolFormMappingId);

                if (currentMapping == null || protocolFormMapping.CreateDate == null)
                {
                    protocolFormMapping.CreateDate = currentDateTime;
                    protocolFormMapping.UpdateDate = currentDateTime;
                    context.Add(protocolFormMapping);
                }
                else
                {
                    currentMapping.ProtocolEDCFormId = protocolFormMapping.ProtocolEDCFormId;
                    currentMapping.ProtocolFormMappingId = protocolFormMapping.ProtocolFormMappingId;
                    currentMapping.IsPrimaryForm = protocolFormMapping.IsPrimaryForm;
                    currentMapping.ProtocolCategoryId = protocolFormMapping.ProtocolCategoryId;
                    currentMapping.UpdateDate = currentDateTime;
                    context.Update(currentMapping);
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

        public async Task<bool> DeleteProtocolFormMapping(ProtocolFormMapping protocolFormMapping)
        {
            try
            {
                context.Remove(protocolFormMapping);
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
