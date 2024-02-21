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

        public ThorDictionaryService(IDbContextFactory<ThorDBContext> dbFactory, IErrorLogService errorLogService, NavigationManager navigationManager) : base(dbFactory)
        {
            _errorLogService = errorLogService;
            _navManager = navigationManager;
        }
        public async Task<IList<ThorDictionary>> GetDictionaries() {
            return await context.THORDictionary.OrderBy(c => c.SortOrder).ToListAsync();
        }

        public async Task<bool> SaveDictionary(ThorDictionary dictionary)
        {
            DateTime currentDateTime = DateTime.UtcNow;

            if (dictionary.CreateDate == null)
                dictionary.CreateDate = currentDateTime;
            try
            {
                dictionary.UpdateDate = currentDateTime;
                context.Add(dictionary);

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
