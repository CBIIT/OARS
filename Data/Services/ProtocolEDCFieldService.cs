using Microsoft.EntityFrameworkCore;
using TheradexPortal.Data.Models;
using TheradexPortal.Data.Services.Abstract;

namespace TheradexPortal.Data.Services
{
    public class ProtocolEDCFieldService : BaseService, IProtocolEDCFieldService
    {
        private readonly IErrorLogService _errorLogService;
        public ProtocolEDCFieldService(IDbContextFactory<ThorDBContext> dbFactory, IErrorLogService errorLogService) : base(dbFactory)
        {
            _errorLogService = errorLogService;
        }

        public async Task<bool> BulkSaveFields(List<ProtocolEDCField> fields)
        {
            // EF doesn't natively support bulk inserts, so the closest we can get is doing an AddRange and then SaveChanges
            try
            {
                context.AddRange(fields);
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
