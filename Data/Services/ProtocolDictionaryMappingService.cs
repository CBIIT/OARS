using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using TheradexPortal.Data.Models;
using TheradexPortal.Data.Services.Abstract;

namespace TheradexPortal.Data.Services
{
    public class ProtocolDictionaryMappingService : BaseService, IProtocolDictionaryMappingService
    {
        private readonly IErrorLogService _errorLogService;
        private readonly NavigationManager _navManager;
        public ProtocolDictionaryMappingService(IDbContextFactory<ThorDBContext> dbFactory, IErrorLogService errorLogService, NavigationManager navigationManager) : base(dbFactory)
        {
            _errorLogService = errorLogService;
            _navManager = navigationManager;
        }

        public async Task<List<ProtocolDictionaryMapping>> GetProtocolDictionaryMappings()
        {
            var protocolDictionaryMappings = await context.ProtocolDictionaryMapping.ToListAsync();
            return protocolDictionaryMappings;
        }

        public async Task<List<ProtocolDictionaryMapping>> GetProtocolDictionaryMappings(int protocolMappingId, int fieldId)
        {
            var protocolDictionaryMappings = await context.ProtocolDictionaryMapping.Where(p => p.ProtocolFieldMappingId == fieldId).ToListAsync();
            var protocolEDCDictionaries = await context.ProtocolEDCDictionary.Where(p => p.ProtocolMappingId == protocolMappingId).ToListAsync();

            foreach (var dictionary in protocolEDCDictionaries)
            {
                if (!protocolDictionaryMappings.Any(p => p.ProtocolEDCDictionaryId == dictionary.ProtocolEDCDictionaryId))
                {
                    protocolDictionaryMappings.Add(new ProtocolDictionaryMapping
                    {
                        ProtocolEDCDictionaryId = dictionary.ProtocolEDCDictionaryId,
                        ProtocolEDCDictionaryName = dictionary.EDCItemName,
                        ProtocolFieldMappingId = fieldId
                    });
                }
                else
                {
                    var currMapping = protocolDictionaryMappings.Where(p => p.ProtocolEDCDictionaryId == dictionary.ProtocolEDCDictionaryId).FirstOrDefault();
                    currMapping.ProtocolEDCDictionaryName = dictionary.EDCItemName; // since this isn't actually from the DB make sure it's set on all the mappings to display
                }
            }
            return protocolDictionaryMappings;
        }

        public async Task<ProtocolDictionaryMapping> GetProtocolDictionaryMapping(int id)
        {
            var protocolDictionaryMapping = await context.ProtocolDictionaryMapping.Where(p => p.ProtocolDictionaryMappingId == id).FirstOrDefaultAsync();
            return protocolDictionaryMapping;
        }

        public async Task<bool> SaveProtocolDictionaryMapping(ProtocolDictionaryMapping protocolDictionaryMapping)
        {
            try
            {
                var currMapping = await context.ProtocolDictionaryMapping.Where(p => p.ProtocolDictionaryMappingId == protocolDictionaryMapping.ProtocolDictionaryMappingId).FirstOrDefaultAsync();

                if (currMapping == null || currMapping.CreateDate == null)
                {
                    protocolDictionaryMapping.CreateDate = DateTime.Now;
                    context.ProtocolDictionaryMapping.Add(protocolDictionaryMapping);
                }
                else
                {
                    currMapping.ProtocolEDCDictionaryId = protocolDictionaryMapping.ProtocolEDCDictionaryId;
                    currMapping.ProtocolFieldMappingId = protocolDictionaryMapping.ProtocolFieldMappingId;
                    currMapping.THORDictionaryId = protocolDictionaryMapping.THORDictionaryId;
                    context.ProtocolDictionaryMapping.Update(protocolDictionaryMapping);
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
