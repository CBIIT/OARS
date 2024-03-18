using System.Data;
using TheradexPortal.Data.Models;

namespace TheradexPortal.Data.Services.Abstract
{
    public interface IProtocolEDCDictionaryService
    {
        public Task<bool> BulkSaveDictionaries(DataTable dictionaries);
        public Task<bool> DeleteAllDictionariesForMappingId(int mappingId);
    }
}
