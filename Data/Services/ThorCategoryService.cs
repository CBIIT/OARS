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
            return await context.THORDataCategory
                .OrderBy(c => c.SortOrder)
                .ThenBy(c => c.CategoryName)
                .ThenBy(c => c.ThorDataCategoryId)
                .ToListAsync();
        }

        public async Task<IList<ThorCategory>> GetCategoriesForMapping(int mappingId)
        {
            var mapping = await context.ProtocolMapping.Include(x => x.Profile).Where(x => x.ProtocolMappingId == mappingId).FirstOrDefaultAsync();
            var profileCategories = await context.ProfileDataCategory.Include(x => x.ThorCategory).Where(x => x.ProfileId == mapping.ProfileId).ToListAsync();
            List<ThorCategory> categories = new List<ThorCategory>();
            foreach (var profileCategory in profileCategories)
            {
                if(profileCategory.ThorCategory != null)
                {
                    categories.Add(profileCategory.ThorCategory);
                }
            }
            return categories
                .OrderBy(c => c.SortOrder)
                .ThenBy(c => c.CategoryName)
                .ThenBy(c => c.ThorDataCategoryId)
                .ToList();
        }
        public async Task<ThorCategory> GetCategory(string id)
        {
            return await context.THORDataCategory.FirstOrDefaultAsync(x => x.ThorDataCategoryId == id);
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
                    category.SortOrder = category.SortOrder ?? 0;
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
                    currentCategory.SortOrder = category.SortOrder ?? 0;
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
