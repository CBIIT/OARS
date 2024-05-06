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
        public ThorCategoryService(IDatabaseConnectionService databaseConnectionService, IErrorLogService errorLogService, NavigationManager navigationManager) : base(databaseConnectionService)
        {
            _errorLogService = errorLogService;
            _navManager = navigationManager;
        }
        public async Task<IList<ThorCategory>> GetCategories() {
            return await context.THORDataCategory.OrderBy(c => c.SortOrder).ToListAsync();
        }

        public async Task<bool> SaveCategory(ThorCategory category)
        {
            try
            {
                DateTime currentDateTime = DateTime.UtcNow;

                ThorCategory currentCategory = context.THORDataCategory.Where(p => p.ThorDataCategoryId == category.ThorDataCategoryId).FirstOrDefault();

                if (currentCategory == null || category.CreateDate == null)
                {
                    category.IsActive = true;
                    category.CreateDate = currentDateTime;
                    category.UpdateDate = currentDateTime;
                    context.Add(category);
                }
                else
                {
                    //Can we change the ThorIDfield?
                    currentCategory.CategoryName = category.CategoryName;
                    currentCategory.ThorDataCategoryId = category.ThorDataCategoryId;
                    currentCategory.IsMultiForm = category.IsMultiForm;
                    currentCategory.SortOrder = category.SortOrder;
                    currentCategory.IsActive = category.IsActive;
                    currentCategory.UpdateDate = currentDateTime;
                    context.Update(currentCategory);
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
