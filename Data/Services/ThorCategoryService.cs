using TheradexPortal.Data.Models;
using TheradexPortal.Data;
using TheradexPortal.Data.Services.Abstract;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
namespace TheradexPortal.Data.Services
{
    public class ThorCategoryService : BaseService, IThorCategoryService
    {
        private readonly IErrorLogService _errorLogService;
        private readonly NavigationManager _navManager;
        public ThorCategoryService(IDbContextFactory<ThorDBContext> dbFactory, IErrorLogService errorLogService, NavigationManager navigationManager) : base(dbFactory)
        {
            _errorLogService = errorLogService;
            _navManager = navigationManager;
        }
        public async Task<IList<ThorCategory>> GetCategories() {
            return await context.THORDataCategory.OrderBy(c => c.SortOrder).ToListAsync();
        }

        public async Task<bool> SaveCategory(ThorCategory category)
        {
            DateTime curDateTime = DateTime.UtcNow;
            if (category.CreateDate == null)
                category.CreateDate = curDateTime;
            try
            {
                category.UpdateDate = curDateTime;
                context.Add(category);
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
