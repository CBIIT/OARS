using System.Data;
using TheradexPortal.Data.Models;

namespace TheradexPortal.Data.Services.Abstract
{
    public interface IProtocolEDCDictionaryService
    {
        public Task<List<ProtocolEDCDictionary>> GetDictionariesByMappingId(int protocolMappingId);
        public Task<List<ProtocolEDCDictionary>> GetDictionariesByMappingIdAndDictionaryName(int protocolMappingId, string dictionaryName);
        public Task<bool> SaveDictionary(ProtocolEDCDictionary dictionary, int protocolMappingId);
        public Task<bool> DeleteDictionary(ProtocolEDCDictionary dictionary);
        public Task<bool> BulkSaveDictionaries(DataTable dictionaries);
        public Task<bool> DeleteAllDictionariesForMappingId(int protocolMappingId);
    }
}
