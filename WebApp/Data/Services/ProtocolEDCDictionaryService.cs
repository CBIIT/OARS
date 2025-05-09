﻿using Microsoft.EntityFrameworkCore;
using Oracle.EntityFrameworkCore;
using Oracle.ManagedDataAccess.Client;
using System.ComponentModel;
using System.Data;
using OARS.Data.Models;
using OARS.Data.Services.Abstract;

namespace OARS.Data.Services
{
    public class ProtocolEDCDictionaryService : BaseService, IProtocolEDCDictionaryService
    {
        private readonly IErrorLogService _errorLogService;
        private readonly IConfiguration _configuration;
        public ProtocolEDCDictionaryService(IDatabaseConnectionService databaseConnectionService, IErrorLogService errorLogService, IConfiguration configuration) : base(databaseConnectionService)
        {
            _errorLogService = errorLogService;
            _configuration = configuration;
        }

        public async Task<List<ProtocolEDCDictionary>> GetDictionariesByMappingId(int protocolMappingId)
        {
            try
            {
                return context.ProtocolEDCDictionary.Where(p => p.ProtocolMappingId == protocolMappingId).ToList();
            } catch (Exception ex)
            {
                await _errorLogService.SaveErrorLogAsync(0, "", ex.InnerException, ex.Source, ex.Message, ex.StackTrace);
                return new List<ProtocolEDCDictionary>();
            }
        }

        public async Task<List<ProtocolEDCDictionary>> GetDictionariesByMappingIdAndDictionaryName(int protocolMappingId, string dictionaryName)
        {
            try
            {
                return context.ProtocolEDCDictionary.Where(p => p.ProtocolMappingId == protocolMappingId && p.EDCDictionaryName == dictionaryName).ToList();
            }
            catch (Exception ex)
            {
                await _errorLogService.SaveErrorLogAsync(0, "", ex.InnerException, ex.Source, ex.Message, ex.StackTrace);
                return new List<ProtocolEDCDictionary>();
            }
        }

        public async Task<bool> SaveDictionary(ProtocolEDCDictionary dictionary, int protocolMappingId)
        {
            try
            {
                DateTime currentDateTime = DateTime.UtcNow;

                int? statusId = context.ProtocolMapping.Where(x => x.ProtocolMappingId == protocolMappingId).Select(x => x.ProtocolMappingStatusId).FirstOrDefault();
                if (statusId != null)
                {
                    string? statusText = context.ProtocolMappingStatus.Where(x => x.ProtocolMappingStatusId == statusId).Select(x => x.StatusName).FirstOrDefault();
                    if (statusText != "Active")
                    {
                        return false;
                    }   
                }
                else
                {
                    return false;
                }
                ProtocolEDCDictionary currentDictionary = context.ProtocolEDCDictionary.Where(p => p.ProtocolEDCDictionaryId == dictionary.ProtocolEDCDictionaryId).FirstOrDefault();

                if (currentDictionary == null || dictionary.CreateDate == null)
                {
                    dictionary.ProtocolMappingId = protocolMappingId;
                    dictionary.CreateDate = currentDateTime;
                    dictionary.UpdatedDate = currentDateTime;
                    context.Add(dictionary);
                }
                else
                {
                    currentDictionary.UpdatedDate = currentDateTime;
                    currentDictionary.EDCDictionaryName = dictionary.EDCDictionaryName;
                    currentDictionary.EDCItemName = dictionary.EDCItemName;
                    currentDictionary.EDCItemId = dictionary.EDCItemId;
                    context.Update(dictionary);
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

        public async Task<bool> BulkSaveDictionaries(DataTable dictionaries)
        {
            // EF doesn't natively support bulk inserts, so the closest we can get is doing an AddRange and then SaveChanges
            // that isn't performant at all for datasets as large as these dictionaries, so do it the Oracle way
         
            DateTime curDateTime = DateTime.UtcNow;
            try
            {
                using (var bulkCopy = new OracleBulkCopy(oracleConnection))
                {
                    bulkCopy.DestinationSchemaName = "DMU";
                    bulkCopy.DestinationTableName = "\"PROTOCOLEDCDICTIONARY\"";
                    bulkCopy.BatchSize = dictionaries.Rows.Count;
                    bulkCopy.WriteToServer(dictionaries);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                await _errorLogService.SaveErrorLogAsync(0, "", ex.InnerException, ex.Source, ex.Message, ex.StackTrace);
                return false;
            }
        }

        public async Task<bool> DeleteDictionary(ProtocolEDCDictionary dictionary)
        {
            try
            {
                context.Remove(dictionary);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                await _errorLogService.SaveErrorLogAsync(0, "", ex.InnerException, ex.Source, ex.Message, ex.StackTrace);
                return false;
            }
        }   

        public async Task<bool> DeleteAllDictionariesForMappingId(int protocolMappingId)
        {
            try
            {
                // Similar to above, this version of EF doesn't support bulk deletes and RemoveRange is too slow, so we have to do it this way
                context.Database.ExecuteSqlRaw("DELETE FROM DMU.\"PROTOCOLEDCDICTIONARY\" WHERE \"PROTOCOL_MAPPING_ID\" = :protocolMappingId", new OracleParameter("protocolMappingId", protocolMappingId));
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
