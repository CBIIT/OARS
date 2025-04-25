using OARS.Data.Models;

namespace OARS.Data.Services.Abstract
{
    public interface IProfileCategoryService
    {
        public Task<IList<ProfileDataCategory>> GetCategories(int profileId);
        public Task<bool> SaveCategory(int profileId, ProfileDataCategory category);
        public Task<bool> DeleteCategory(ProfileDataCategory category);
    }
}
