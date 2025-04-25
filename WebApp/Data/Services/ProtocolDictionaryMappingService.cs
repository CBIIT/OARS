using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using OARS.Data.Models;
using OARS.Data.Services.Abstract;

namespace OARS.Data.Services
{
    public class ProtocolDictionaryMappingService : BaseService, IProtocolDictionaryMappingService
    {
        private readonly IErrorLogService _errorLogService;
        private readonly NavigationManager _navManager;
        public ProtocolDictionaryMappingService(IDatabaseConnectionService databaseConnectionService, IErrorLogService errorLogService, NavigationManager navigationManager) : base(databaseConnectionService)
        {
            _errorLogService = errorLogService;
            _navManager = navigationManager;
        }

        public async Task<List<ProtocolDictionaryMapping>> GetProtocolDictionaryMappings()
        {
            var protocolDictionaryMappings = await context.ProtocolDictionaryMapping.ToListAsync();
            return protocolDictionaryMappings;
        }

        public async Task<List<ProtocolDictionaryMapping>> GetProtocolDictionaryMappings(int protocolMappingId) // , int fieldId
        {
            var protocolDictionaryMappings = await context.ProtocolDictionaryMapping
                .Include(p => p.ProtocolEDCDictionary)
                .Include(p => p.ProtocolFieldMapping)
                .Include(p => p.THORDictionary)
                .Where(p => p.ProtocolFieldMappingId == protocolMappingId)
                .ToListAsync();

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
