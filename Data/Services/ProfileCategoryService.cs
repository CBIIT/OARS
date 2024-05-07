using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using TheradexPortal.Data.Models;
using TheradexPortal.Data.Services.Abstract;

namespace TheradexPortal.Data.Services
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
                ProfileDataCategory currCategory = context.ProfileDataCategory.Where(p => p.ProfileDataCategoryId == category.ProfileDataCategoryId).FirstOrDefault();
                if(currCategory == null)
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
