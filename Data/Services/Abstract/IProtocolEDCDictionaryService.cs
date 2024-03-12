using TheradexPortal.Data.Models;

namespace TheradexPortal.Data.Services.Abstract
{
    public interface IProtocolEDCDictionaryService
    {
        public Task<bool> BulkSaveDictionaries(List<ProtocolEDCDictionary> dictionaries);
    }
}
