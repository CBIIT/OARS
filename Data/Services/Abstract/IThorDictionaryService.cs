using TheradexPortal.Data.Models;

namespace TheradexPortal.Data.Services.Abstract
{
        public interface IThorDictionaryService
        {
            public Task<IList<ThorDictionary>> GetDictionaries();
            public Task<bool> SaveDictionary(ThorDictionary dictionary);

        }
}
