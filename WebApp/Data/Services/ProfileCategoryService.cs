using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using OARS.Data.Models;
using OARS.Data.Services.Abstract;

namespace OARS.Data.Services
{
    public class ProfileCategoryService: BaseService, IProfileCategoryService
    {
        private readonly IErrorLogService _errorLogService;
        private readonly NavigationManager _navManager;
        public ProfileCategoryService(IDatabaseConnectionService databaseConnectionService, IErrorLogService errorLogService, NavigationManager navigationManager) : base(databaseConnectionService)
        {
            _errorLogService = errorLogService;
            _navManager = navigationManager;
        }
        public async Task<IList<ProfileDataCategory>> GetCategories(int profileId)
        {
            return await context.ProfileDataCategory.Where(c => c.ProfileId == profileId).Include(c => c.ThorCategory).ToListAsync();
        }

        public async Task<bool> SaveCategory(int profileId, ProfileDataCategory category)
        {
            try
            {
                DateTime curDateTime = DateTime.UtcNow;
                ProfileDataCategory currCategory = context.ProfileDataCategory.Where(p => (p.ProfileDataCategoryId == category.ProfileDataCategoryId)).FirstOrDefault();

                if (currCategory != null && currCategory.ThorCategory != null && currCategory.ThorCategory.ThorDataCategoryId != category.ThorDataCategoryId)
                {   // Get all the current fields for the category and remove them
                    if (currCategory.ThorCategory.ThorDataCategoryId != category.ThorDataCategoryId)
                    {
                        var currFields = await context.ProfileFields.Where(pf => pf.ProfileId == category.ProfileId).ToListAsync();
                        var toRemove = await context.THORField.Where(tf => tf.ThorDataCategoryId == currCategory.ThorCategory.ThorDataCategoryId).ToListAsync();
                        var idsToRemove = toRemove.Select(tf => tf.ThorFieldId).ToList();

                        currFields = currFields.Where(pf => idsToRemove.Contains(pf.THORFieldId)).ToList();
                        context.RemoveRange(currFields);
                    }
                }

                //Get all the fields for the category and add them to the profile
                var thorFields = await context.THORField.Where(tf => tf.ThorDataCategoryId == category.ThorDataCategoryId).Where(tf => tf.IsActive).ToListAsync();
                foreach (var thorField in thorFields)
                {
                    ProfileField newField = new ProfileField
                    {
                        THORFieldId = thorField.ThorFieldId,
                        ProfileId = profileId,
                        CreateDate = curDateTime
                    };

                    context.Add(newField);
                }

                if (currCategory == null)
                {
                    category.ProfileId = profileId;
                    category.CreateDate = curDateTime;
                    context.Add(category);
                }
                else
                {
                    currCategory.ProfileId = category.ProfileId;
                    currCategory.ThorDataCategoryId = category.ThorDataCategoryId; 
                    currCategory.CreateDate = category.CreateDate;
                    context.Update(currCategory);
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

        public async Task<bool> DeleteCategory(ProfileDataCategory category)
        {
            try
            {
                var profileFields = await context.ProfileFields.Where(pf => pf.ProfileId == category.ProfileId).ToListAsync();
                var thorFields = await context.THORField.Where(tf => tf.ThorDataCategoryId == category.ThorDataCategoryId).ToListAsync();
                var thorFieldIds = thorFields.Select(tf => tf.ThorFieldId).ToList();
                var fieldsToDelete = profileFields.Where(pf => thorFieldIds.Contains(pf.THORFieldId)).ToList();
                context.RemoveRange(fieldsToDelete);
                context.Remove(category);
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
