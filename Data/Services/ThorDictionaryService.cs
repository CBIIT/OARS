using TheradexPortal.Data.Models;
using TheradexPortal.Data.Services.Abstract;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
namespace TheradexPortal.Data.Services
{
    public class ThorDictionaryService : BaseService, IThorDictionaryService
    {
        private readonly IErrorLogService _errorLogService;
        private readonly NavigationManager _navManager;

        public ThorDictionaryService(IDatabaseConnectionService databaseConnectionService, IErrorLogService errorLogService, NavigationManager navigationManager) : base(databaseConnectionService)
        {
            _errorLogService = errorLogService;
            _navManager = navigationManager;
        }
        public async Task<IList<ThorDictionary>> GetDictionaries(bool activeOnly = false) {

            var dictQuery = context.THORDictionary.AsQueryable();

            if (activeOnly)
            {
                dictQuery = dictQuery.Where(x => x.IsActive);
            }

            return await dictQuery
                .OrderBy(c => c.SortOrder)
                .ThenBy(c => c.DictionaryName)
                .ThenBy(c => c.DictionaryOption)
                .ToListAsync();
        }

        public async Task<IList<ThorDictionary>> GetDictionaryEntries(int dictionaryId, bool activeOnly = true)
        {
            // this needs to get the rest of the things with that dict NAME from the one you pass in
            var dict = context.THORDictionary.Where(x => x.ThorDictionaryId == dictionaryId).FirstOrDefault();
            if (dict != null)
            {
                var dictQuery = context.THORDictionary.Where(x => x.DictionaryName == dict.DictionaryName);
                if (activeOnly)
                {
                    dictQuery = dictQuery.Where(x => x.IsActive);
                }
                var entries = await dictQuery
                    .OrderBy(o=>o.DictionaryName)
                    .ThenBy(c => c.DictionaryName)
                    .ThenBy(c => c.DictionaryOption)
                    .ToListAsync();
                return entries;
            }
            return new List<ThorDictionary>();
        }

        public async Task<bool> SaveDictionary(ThorDictionary dictionary)
        {
            try
            {
                DateTime currentDateTime = DateTime.UtcNow;
                dictionary.UpdateDate = currentDateTime;

                ThorDictionary currentThorDictionary = context.THORDictionary.Where(p => p.ThorDictionaryId == dictionary.ThorDictionaryId).FirstOrDefault();

                if (currentThorDictionary == null || dictionary.CreateDate == null)
                {
                    dictionary.IsActive = true;
                    dictionary.SortOrder = dictionary.SortOrder ?? 0;
                    dictionary.CreateDate = currentDateTime;
                    context.Add(dictionary);
                }
                else
                {
                    currentThorDictionary.DictionaryName = dictionary.DictionaryName;
                    currentThorDictionary.DictionaryOption = dictionary.DictionaryOption;
                    currentThorDictionary.DictionaryValue = dictionary.DictionaryValue;
                    currentThorDictionary.SortOrder = dictionary.SortOrder ?? 0;
                    currentThorDictionary.IsActive = dictionary.IsActive;
                    currentThorDictionary.UpdateDate = currentDateTime;
                    context.Update(currentThorDictionary);
                }

                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                await _errorLogService.SaveErrorLogAsync(0, _navManager.Uri, ex.InnerException, ex.Source, ex.Message, ex.StackTrace);
                return false;
            }
        }
    }
}
