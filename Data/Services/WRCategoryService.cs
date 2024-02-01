using TheradexPortal.Data.Models;
using TheradexPortal.Data;
using TheradexPortal.Data.Services.Abstract;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
namespace TheradexPortal.Data.Services
{
    public class WRCategoryService : BaseService, IWRCategoryService
    {
        private readonly IErrorLogService _errorLogService;
        private readonly NavigationManager _navManager;
        public WRCategoryService(IDbContextFactory<ThorDBContext> dbFactory, IErrorLogService errorLogService, NavigationManager navigationManager) : base(dbFactory)
        {
            _errorLogService = errorLogService;
            _navManager = navigationManager;
        }
        public async Task<IList<WRCategory>> GetCategories() {
            return await context.THORDataCategory.OrderBy(c => c.SortOrder).ToListAsync();
        }

        public bool SaveCategories(IList<WRCategory> categories)
        {
            DateTime curDateTime = DateTime.UtcNow;
            try
            {
                foreach(var category in categories)
                {
                    category.UpdateDate = curDateTime;
                    context.Add(category);
                }
                context.SaveChanges();
                return true;

            }
            catch (Exception ex)
            {
                _errorLogService.SaveErrorLogAsync(0, _navManager.Uri, ex.InnerException, ex.Source, ex.Message, ex.StackTrace);
                return false;
            }
        }
    }
}
