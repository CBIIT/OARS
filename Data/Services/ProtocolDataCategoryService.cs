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
        public ProtocolDataCategoryService(IDatabaseConnectionService databaseConnectionService, IErrorLogService errorLogService, NavigationManager navigationManager) : base(databaseConnectionService)
        {
            _errorLogService = errorLogService;
            _navManager = navigationManager;
        }

        public async Task<List<ProtocolDataCategory>> GetCategoriesByMappingId(int mappingId)
        {
            var mapping = await context.ProtocolMapping.Include(x => x.Profile).Where(x => x.ProtocolMappingId == mappingId).FirstOrDefaultAsync();
            var profileCategories = await context.ProfileDataCategory.Include(x => x.ThorCategory).Where(x => x.ProfileId == mapping.ProfileId).ToListAsync();
            var pcIds = profileCategories.Select(x => x.ThorDataCategoryId).ToList();
            var protocolCategories = await context.ProtocolDataCategories
                .Include(x => x.ProtocolMapping)
                .Include(x => x.THORDataCategory)
                .Include(x => x.ProtocolCategoryStatus)
                .Where(x => x.ProtocolMappingId == mappingId)
                .ToListAsync();

            protocolCategories = protocolCategories.Where(x => pcIds.Contains(x.THORDataCategoryId)).ToList();
            return protocolCategories;

        }

        public async Task<ProtocolDataCategory> BuildDefaultProtocolDataCategory(ThorCategory category, int mappingId)
        {
            var defaultDataCategoryStatus = await context.ProtocolCategoryStatus.FirstOrDefaultAsync(x => x.ProtocolCategoryStatusId == 1);
            if (defaultDataCategoryStatus == null) {
                throw new Exception("Default Protocol Category Status not found");
            }
            return new ProtocolDataCategory
            {
                ProtocolMappingId = mappingId,
                THORDataCategoryId = category.ThorDataCategoryId,
                THORDataCategory = category,
                ProtocolCategoryStatusId = 1,
                ProtocolCategoryStatus = defaultDataCategoryStatus,
                IsMultiForm = false,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now
            };
        }

        public async Task<List<ProtocolDataCategory>> GetCategoriesByMappingProfile(int mappingId)
        {
            var dataCategories = await context.ProtocolField
                .Include(x => x.ThorField)
                .Where(x => x.ProtocolMappingId == mappingId)
                .GroupBy(x => x.ThorField.ThorDataCategoryId)
                .Select(x => x.Select(y => y.ThorField.Category).First())
                .ToListAsync();

            var protocolDataCategories = await context.ProtocolDataCategories
                .Include(x => x.THORDataCategory)
                .Include(x => x.ProtocolCategoryStatus)
                .Where(x => x.ProtocolMappingId == mappingId) 
                .ToListAsync();


            foreach (var category in dataCategories)
            {
                if (category == null)
                {
                    continue;
                }
                if (!protocolDataCategories.Any(x => x.THORDataCategoryId == category.ThorDataCategoryId))
                {
                    ProtocolDataCategory newCategory = await BuildDefaultProtocolDataCategory(category, mappingId);
                    protocolDataCategories.Add(newCategory);
                }
                
            }

            // Get the in-progress status
            var inProgressStatus = await context.ProtocolCategoryStatus.FirstOrDefaultAsync(x => x.ProtocolCategoryStatusId == 2);
            if (inProgressStatus == null)
            {
                throw new Exception("In-progress Protocol Category Status not found");
            }

            foreach (var protocolDataCategory in protocolDataCategories)
            {
                // Ensure that the mapping has not started if the status is Not Started
                if (protocolDataCategory.ProtocolCategoryStatusId == 1 &&
                    await context.ProtocolFieldMappings.AnyAsync(x => 
                        x.ThorFieldId != null &&                        
                        x.ProtocolEDCField.ProtocolEDCForm.ProtocolMappingId == mappingId &&
                        x.ThorField.ThorDataCategoryId == protocolDataCategory.THORDataCategoryId))
                {
                    // Field mappings found for this category, set status to in-progress
                    protocolDataCategory.ProtocolCategoryStatus = inProgressStatus;
                    protocolDataCategory.ProtocolCategoryStatusId = inProgressStatus.ProtocolCategoryStatusId;
                    await SaveCategory(protocolDataCategory, mappingId);                    
                }
            }

            return protocolDataCategories.OrderBy(x => x.THORDataCategory.CategoryName).ToList();

        }

        public async Task<ProtocolDataCategory?> GetCategory(int categoryId)
        {
            var category = await context.ProtocolDataCategories
                .Include(x => x.ProtocolMapping)
                .Include(x => x.THORDataCategory)
                .Include(x => x.ProtocolCategoryStatus)
                .FirstOrDefaultAsync(x => x.ProtocolCategoryId == categoryId);

            return category;
        }

        public async Task<ProtocolDataCategory> GetOrBuildProtocolDataCategory(int mappingId, string thorDataCategoryId)
        {

            var existingCategory = await context.ProtocolDataCategories
                .FirstOrDefaultAsync(x => x.ProtocolMappingId == mappingId && x.THORDataCategoryId == thorDataCategoryId);
            if (existingCategory != null)
            {
                return existingCategory;
            }

            var category = await context.THORDataCategory.FirstOrDefaultAsync(x => x.ThorDataCategoryId == thorDataCategoryId);
            if (category == null)
            {
                throw new Exception("Category not found");
            }
            return await this.BuildDefaultProtocolDataCategory(category, mappingId);
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
                    existingCategory.IsMultiForm = category.IsMultiForm;
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
