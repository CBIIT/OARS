﻿using OARS.Data.Models;

namespace OARS.Data.Services.Abstract
{
        public interface IThorDictionaryService
        {
            public Task<IList<ThorDictionary>> GetDictionaries(bool activeOnly = false);
            public Task<bool> SaveDictionary(ThorDictionary dictionary);
            public Task<IList<ThorDictionary>> GetDictionaryEntries(int dictionaryId, bool activeOnly = true);
        }
}
