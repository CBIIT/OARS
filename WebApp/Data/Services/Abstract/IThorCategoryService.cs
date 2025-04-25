using OARS.Data.Models;

namespace OARS.Data.Services.Abstract
{
        public interface IThorCategoryService
        {
            public Task<IList<ThorCategory>> GetCategories();
            public Task<ThorCategory> GetCategory(string id);
            public Task<bool> SaveCategory(ThorCategory category);
            public Task<IList<ThorCategory>> GetCategoriesForMapping(int mappingId);
        }
}
