using TheradexPortal.Data.Models;

namespace TheradexPortal.Data.Services.Abstract
{
        public interface IThorCategoryService
        {
            public Task<IList<ThorCategory>> GetCategories();
            public Task<ThorCategory> GetCategory(string id);
            public Task<bool> SaveCategory(ThorCategory category);

        }
}
