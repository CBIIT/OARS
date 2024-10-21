using Microsoft.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using TheradexPortal.Data.Models;
using TheradexPortal.Data.Services.Abstract;

namespace TheradexPortal.Data.Services
{
    public class ProtocolEDCFieldService : BaseService, IProtocolEDCFieldService
    {
        private readonly IErrorLogService _errorLogService;
        private readonly IConfiguration _configuration;
        public ProtocolEDCFieldService(IDatabaseConnectionService databaseConnectionService, IErrorLogService errorLogService, IConfiguration configuration) : base(databaseConnectionService)
        {
            _errorLogService = errorLogService;
            _configuration = configuration;
        }

        public async Task<List<ProtocolEDCField>> GetFieldsByFormIds(List<int> formIds)
        {
            try
            {
                return await context.ProtocolEDCField
                    .Where(x => formIds.Contains(x.ProtocolEDCFormId))
                    .OrderBy(o=>o.EDCFieldIdentifier)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                await _errorLogService.SaveErrorLogAsync(0, "", ex.InnerException, ex.Source, ex.Message, ex.StackTrace);
                return new List<ProtocolEDCField>();
            }
        }
        public async Task<List<ProtocolEDCField>> GetFieldsByProtocolMappingId(int protocolMappingId)
        {
            try
            {
                return await context.ProtocolEDCField
                    .Where(x => x.ProtocolEDCForm.ProtocolMappingId == protocolMappingId)
                    .OrderBy(x => x.EDCFieldIdentifier)
                    .ToListAsync();
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
                    bulkCopy.DestinationTableName = "\"PROTOCOLEDCFIELD\"";
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

        public async Task<bool> DeleteAllFieldsForFormIds(int protocolMappingId)
        {
            try
            {
                // Similar to above, this version of EF doesn't support bulk deletes and RemoveRange is too slow, so we have to do it this way
                string command = $@"
                    DELETE from 
                      (
                        SELECT 
                            fld.*
                        FROM 
                            DMU.""PROTOCOLEDCFIELD"" fld
                            INNER JOIN DMU.""PROTOCOLEDCFORM"" frm 
                                ON frm.""PROTOCOL_EDC_FORM_ID"" = fld.""PROTOCOL_EDC_FORM_ID""
                        WHERE 
                            frm.""PROTOCOL_MAPPING_ID"" = {protocolMappingId}
                      );";  

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
