using Microsoft.EntityFrameworkCore;
using Oracle.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System.Data;
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

        public async Task<bool> BulkSaveDictionaries(DataTable dictionaries)
        {
            // EF doesn't natively support bulk inserts, so the closest we can get is doing an AddRange and then SaveChanges
            // that isn't performant at all for datasets as large as these dictionaries, so do it the Oracle way
         
            DateTime curDateTime = DateTime.UtcNow;
            try
            {
                using (var connection = new OracleConnection(context.Database.GetDbConnection().ConnectionString))
                {
                    connection.Open();
                    using (var bulkCopy = new OracleBulkCopy(connection))
                    {
                        bulkCopy.DestinationSchemaName = "DMU";
                        bulkCopy.DestinationTableName = "\"ProtocolEDCDictionary\"";
                        bulkCopy.BatchSize = dictionaries.Rows.Count;
                        bulkCopy.WriteToServer(dictionaries);
                    }
                }


                //context.AddRange(dictionaries);
                //await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                await _errorLogService.SaveErrorLogAsync(0, "", ex.InnerException, ex.Source, ex.Message, ex.StackTrace);
                return false;
            }
        }

        public async Task<bool> DeleteAllDictionariesForMappingId(int mappingId)
        {
            try
            {
                // Similar to above, this version of EF doesn't support bulk deletes and RemoveRange is too slow, so we have to do it this way
                context.Database.ExecuteSqlRaw("DELETE FROM DMU.\"ProtocolEDCDictionary\" WHERE \"Protocol_Mapping_Id\" = :mappingId", new OracleParameter("mappingId", mappingId));
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
