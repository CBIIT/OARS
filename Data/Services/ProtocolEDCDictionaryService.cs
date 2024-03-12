using Microsoft.EntityFrameworkCore;
using TheradexPortal.Data.Models;
using TheradexPortal.Data.Services.Abstract;

namespace TheradexPortal.Data.Services
{
    public class ProtocolEDCDictionaryService : BaseService, IProtocolEDCDictionaryService
    {
        private readonly IErrorLogService _errorLogService;
        public ProtocolEDCDictionaryService(IDbContextFactory<ThorDBContext> dbFactory, IErrorLogService errorLogService) : base(dbFactory)
        {
            _errorLogService = errorLogService;
        }

        public async Task<bool> BulkSaveDictionaries(List<ProtocolEDCDictionary> dictionaries)
        {
            // EF doesn't natively support bulk inserts, so the closest we can get is doing an AddRange and then SaveChanges
            // SaveChanges still makes a round trip to the DB for each entity, so if this isn't performant there may be extensions to try
            DateTime curDateTime = DateTime.UtcNow;
            try
            {
                context.AddRange(dictionaries);
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
