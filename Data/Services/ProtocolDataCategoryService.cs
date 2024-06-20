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

            var defaultDataCategoryStatus = await context.ProtocolCategoryStatus.FirstOrDefaultAsync(x => x.ProtocolCategoryStatusId == 1);

            foreach (var category in dataCategories)
            {
                if (!protocolDataCategories.Any(x => x.THORDataCategoryId == category.ThorDataCategoryId))
                {
                    ProtocolDataCategory newCategory = new ProtocolDataCategory
                    {
                        ProtocolMappingId = mappingId,
                        THORDataCategoryId = category!.ThorDataCategoryId,
                        THORDataCategory = category,
                        ProtocolCategoryStatusId = 1,
                        ProtocolCategoryStatus = defaultDataCategoryStatus,
                        IsMultiForm = false,
                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now
                    };
                    protocolDataCategories.Add(newCategory);

                }
            }

            return protocolDataCategories.OrderBy(x => x.THORDataCategory.CategoryName).ToList();

            //var profile = await context.ProtocolMapping.Include(x => x.Profile).Where(x => x.ProtocolMappingId == mappingId).FirstOrDefaultAsync();
            //if (profile == null)
            //{
            //    return new List<ProtocolDataCategory>();
            //} else
            //{
            //    var profileDataCategories = await context.ProfileDataCategory
            //        .Include(x => x.ThorCategory).Where(x => x.ProfileId == profile.ProfileId).ToListAsync();
            //    var protocolDataCategories = await context.ProtocolDataCategories
            //    .Include(x => x.ProtocolMapping)
            //    .Include(x => x.THORDataCategory)
            //    .Include(x => x.ProtocolCategoryStatus)
            //    .Where(x => x.ProtocolMappingId == mappingId)
            //    .ToListAsync();

            //    foreach (var profileCategory in profileDataCategories)
            //    {
            //        if(protocolDataCategories.Find(x => x.THORDataCategoryId == profileCategory.ThorDataCategoryId) == null)
            //        {
            //            ProtocolDataCategory newCategory = new ProtocolDataCategory
            //            {
            //                ProtocolMappingId = mappingId,
            //                THORDataCategoryId = profileCategory.ThorDataCategoryId,
            //                THORDataCategory = profileCategory.ThorCategory,
            //                ProtocolCategoryStatus = await context.ProtocolCategoryStatus.FirstOrDefaultAsync(x => x.ProtocolCategoryStatusId == 1),
            //                ProtocolCategoryStatusId = 1,
            //                IsMultiForm = false,
            //                CreateDate = DateTime.Now,
            //                UpdateDate = DateTime.Now
            //            };
            //            protocolDataCategories.Add(newCategory);
            //        }
            //    }
            //    return protocolDataCategories.OrderBy(x => x.THORDataCategory.CategoryName).ToList();
            //}
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
