using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using TheradexPortal.Data.Models;
using TheradexPortal.Data.Services.Abstract;

namespace TheradexPortal.Data.Services
{
    public class ProtocolFieldMappingService : BaseService, IProtocolFieldMappingService
    {
        private readonly IErrorLogService _errorLogService;
        private readonly NavigationManager _navManager;
        public ProtocolFieldMappingService(IDbContextFactory<ThorDBContext> dbFactory, IErrorLogService errorLogService, NavigationManager navigationManager) : base(dbFactory)
        {
            _errorLogService = errorLogService;
            _navManager = navigationManager;
        }

        public async Task<IList<ProtocolFieldMapping>> GetProtocolFieldMappings()
        {
            return await context.ProtocolFieldMappings.ToListAsync();
        }
        
        public async Task<IList<ProtocolFieldMapping>> GetProtocolFieldMappings(string fieldId)
        {
            return await context.ProtocolFieldMappings.Where(x => x.ThorFieldId == fieldId).Include(x => x.ProtocolEDCField).Include(p => p.ProtocolEDCField.ProtocolEDCForm).ToListAsync();
        }

        public async Task<IList<ProtocolFieldMapping>> GetProtocolFieldMappingsForCategory(string categoryId)
        {
            var thorFields = await context.THORField.Where(x => x.ThorDataCategoryId == categoryId).ToListAsync();
            var fieldIds = thorFields.Select(x => x.ThorFieldId).ToList();
            var mappings = await context.ProtocolFieldMappings.Where(x => fieldIds.Contains(x.ThorFieldId)).Include(x => x.ProtocolEDCField).Include(p => p.ProtocolEDCField.ProtocolEDCForm).ToListAsync();

            foreach (string fieldId in fieldIds)
            {
                if (!mappings.Any(x => x.ThorFieldId == fieldId))
                {
                    ProtocolFieldMapping mapping = new ProtocolFieldMapping();
                    mapping.ThorFieldId = fieldId;
                    mappings.Add(mapping);
                }
            }

            return mappings;
        }

        public async Task<ProtocolFieldMapping> GetProtocolFieldMapping(int id)
        {
            var fieldMapping = await context.ProtocolFieldMappings.Include(x=>x.ThorField).Include(x=>x.ProtocolEDCField).FirstOrDefaultAsync(x => x.ProtocolFieldMappingId == id);
            return fieldMapping;
        }

        public async Task<bool> SaveProtocolFieldMapping(ProtocolFieldMapping protocolFieldMapping)
        {
            try
            {
                DateTime currentDateTime = DateTime.UtcNow;

                ProtocolFieldMapping currentMapping = context.ProtocolFieldMappings.FirstOrDefault(x => x.ProtocolFieldMappingId == protocolFieldMapping.ProtocolFieldMappingId);
                if (currentMapping == null)
                {
                    protocolFieldMapping.CreateDate = currentDateTime;
                    protocolFieldMapping.UpdateDate = currentDateTime;
                    context.Add(protocolFieldMapping);
                }
                else
                {
                    currentMapping.ThorFieldId = protocolFieldMapping.ThorFieldId;
                    currentMapping.ProtocolEDCFieldId = protocolFieldMapping.ProtocolEDCFieldId;
                    currentMapping.UpdateDate = currentDateTime;
                }

                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                await _errorLogService.SaveErrorLogAsync(0, "", ex.InnerException, ex.Source, ex.Message, ex.StackTrace);
                return false;
            }
        }

        public async Task<bool> DeleteProtocolFieldMapping(ProtocolFieldMapping mapping)
        {
            try
            {
                context.ProtocolFieldMappings.Remove(mapping);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                await _errorLogService.SaveErrorLogAsync(0, "", ex.InnerException, ex.Source, ex.Message, ex.StackTrace);
                return false;
            }   
        }
    }
}
