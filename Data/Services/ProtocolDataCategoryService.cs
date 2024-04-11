using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using TheradexPortal.Data.Models;
using TheradexPortal.Data.Services.Abstract;

namespace TheradexPortal.Data.Services
{
    public class ProtocolDataCategoryService : BaseService, IProtocolDataCategoryService
    {
        private readonly IErrorLogService _errorLogService;
        private readonly NavigationManager _navManager;
        public ProtocolDataCategoryService(IDbContextFactory<ThorDBContext> dbFactory, IErrorLogService errorLogService, NavigationManager navigationManager) : base(dbFactory)
        {
            _errorLogService = errorLogService;
            _navManager = navigationManager;
        }

        public async Task<List<ProtocolDataCategory>> GetCategoriesByMappingId(int mappingId)
        {
            return await context.ProtocolDataCategories
                .Include(x => x.ProtocolMapping)
                .Include(x => x.THORDataCategory)
                .Include(x => x.ProtocolCategoryStatus)
                .Where(x => x.ProtocolMappingId == mappingId)
                .ToListAsync();
        }

        public async Task<ProtocolDataCategory> GetCategory(int categoryId)
        {
            var category = await context.ProtocolDataCategories
                .Include(x => x.ProtocolMapping)
                .Include(x => x.THORDataCategory)
                .Include(x => x.ProtocolCategoryStatus)
                .FirstOrDefaultAsync(x => x.ProtocolCategoryId == categoryId);
            return category;
        }

        public async Task<bool> SaveCategory(ProtocolDataCategory category, int mappingId)
        {
            try
            {
                if (category.ProtocolCategoryId == 0)
                {
                    category.ProtocolMappingId = mappingId;
                    category.CreateDate = DateTime.Now;
                    context.ProtocolDataCategories.Add(category);
                }
                else
                {
                    var existingCategory = await context.ProtocolDataCategories
                        .FirstOrDefaultAsync(x => x.ProtocolCategoryId == category.ProtocolCategoryId);
                    if (existingCategory == null)
                    {
                        return false;
                    }

                    existingCategory.THORDataCategoryId = category.THORDataCategoryId;
                    existingCategory.ProtocolCategoryStatusId = category.ProtocolCategoryStatusId;
                    existingCategory.UpdateDate = DateTime.Now;
                }

                await context.SaveChangesAsync();
                return true;
            } catch (Exception ex)
            {
                await _errorLogService.SaveErrorLogAsync(0, _navManager.Uri, ex.InnerException, ex.Source, ex.Message, ex.StackTrace);
                return false;
            }

        }

    }
}
