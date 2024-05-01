using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using TheradexPortal.Data.Models;
using TheradexPortal.Data.Services.Abstract;
using YamlDotNet.Core.Events;

namespace TheradexPortal.Data.Services
{
    public class ProtocolEDCFieldService : BaseService, IProtocolEDCFieldService
    {
        private readonly IErrorLogService _errorLogService;
        private readonly IConfiguration _configuration;
        public ProtocolEDCFieldService(IDbContextFactory<ThorDBContext> dbFactory, IErrorLogService errorLogService, IConfiguration configuration) : base(dbFactory)
        {
            _errorLogService = errorLogService;
            _configuration = configuration;
        }

        public async Task<List<ProtocolEDCField>> GetFieldsByFormIds(List<int> formIds)
        {
            try
            {
                return await context.ProtocolEDCField.Where(x => formIds.Contains(x.ProtocolEDCFormId)).ToListAsync();
            }
            catch (Exception ex)
            {
                await _errorLogService.SaveErrorLogAsync(0, "", ex.InnerException, ex.Source, ex.Message, ex.StackTrace);
                return new List<ProtocolEDCField>();
            }
        }

        public async Task<bool> SaveField(ProtocolEDCField field)
        {
            try
            {
                DateTime currentDateTime = DateTime.Now;
                ProtocolEDCField currentField = context.ProtocolEDCField.FirstOrDefault(x => x.ProtocolEDCFieldId == field.ProtocolEDCFieldId);

                if(currentField == null || currentField.CreateDate == null)
                {
                    field.CreateDate = currentDateTime;
                    context.Add(field);
                }
                else {
                    currentField.ProtocolEDCFormId = field.ProtocolEDCFormId;
                    currentField.EDCFieldIdentifier = field.EDCFieldIdentifier;
                    currentField.EDCFieldName = field.EDCFieldName;
                    currentField.EDCDictionaryName = field.EDCDictionaryName;
                    currentField.UpdateDate = currentDateTime;
                    context.Update(field);
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
        public async Task<bool> BulkSaveFields(DataTable fields)
        {
            // EF doesn't natively support bulk inserts, so the closest we can get is doing an AddRange and then SaveChanges
            // this isn't very performant, so do it the oracle way here
            DateTime curDateTime = DateTime.UtcNow;
            try
            {                
                using (var bulkCopy = new OracleBulkCopy(oracleConnection))
                {
                    bulkCopy.DestinationSchemaName = "DMU";
                    bulkCopy.DestinationTableName = "\"ProtocolEDCField\"";
                    bulkCopy.BatchSize = fields.Rows.Count;
                    bulkCopy.WriteToServer(fields);
                }
                return true;
            }
            catch (Exception ex)
            {
                await _errorLogService.SaveErrorLogAsync(0, "", ex.InnerException, ex.Source, ex.Message, ex.StackTrace);
                return false;
            }
        }

        public async Task<bool> DeleteField(ProtocolEDCField field)
        {
            try
            {
                ProtocolEDCField currentField = context.ProtocolEDCField.FirstOrDefault(x => x.ProtocolEDCFieldId == field.ProtocolEDCFieldId);
                if (currentField != null) {
                    context.Remove(currentField);
                    await context.SaveChangesAsync();
                }
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
