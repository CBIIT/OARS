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
        public async Task<IList<ThorDictionary>> GetDictionaries() {
            return await context.THORDictionary.OrderBy(c => c.SortOrder).ToListAsync();
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
                    dictionary.CreateDate = currentDateTime;
                    context.Add(dictionary);
                }
                else
                {
                    currentThorDictionary.DictionaryName = dictionary.DictionaryName;
                    currentThorDictionary.DictionaryOption = dictionary.DictionaryOption;
                    currentThorDictionary.DictionaryValue = dictionary.DictionaryValue;
                    currentThorDictionary.SortOrder = dictionary.SortOrder;
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
