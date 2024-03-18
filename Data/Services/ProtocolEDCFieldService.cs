using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using TheradexPortal.Data.Models;
using TheradexPortal.Data.Services.Abstract;
using YamlDotNet.Core.Events;

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

        public async Task<bool> DeleteAllFieldsForFormIds(List<int> formIds)
        {
            try
            {
                // Similar to above, this version of EF doesn't support bulk deletes and RemoveRange is too slow, so we have to do it this way
                string command = "DELETE FROM DMU.\"ProtocolEDCField\" WHERE \"Protocol_EDC_Form_Id\" IN (" + String.Join(",", formIds) + ")";
                
                context.Database.ExecuteSqlRaw(command);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                await _errorLogService.SaveErrorLogAsync(0, "", ex.InnerException, ex.Source, ex.Message, ex.StackTrace);
                return false;
            }
        }
    }
}
