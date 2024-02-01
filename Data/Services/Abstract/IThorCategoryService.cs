using TheradexPortal.Data.Models;

namespace TheradexPortal.Data.Services.Abstract
{
        public interface IThorCategoryService
        {
            public Task<IList<ThorCategory>> GetCategories();
            public bool SaveCategories(IList<ThorCategory> categories);

        }
}
