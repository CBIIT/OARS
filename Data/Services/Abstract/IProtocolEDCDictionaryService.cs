using System.Data;
using TheradexPortal.Data.Models;

namespace TheradexPortal.Data.Services.Abstract
{
    public interface IProtocolEDCDictionaryService
    {
        public Task<List<ProtocolEDCDictionary>> GetDictionariesByMappingId(int mappingId);
        public Task<bool> SaveDictionary(ProtocolEDCDictionary dictionary, int mappingId);
        public Task<bool> DeleteDictionary(ProtocolEDCDictionary dictionary);
        public Task<bool> BulkSaveDictionaries(DataTable dictionaries);
        public Task<bool> DeleteAllDictionariesForMappingId(int mappingId);
    }
}
