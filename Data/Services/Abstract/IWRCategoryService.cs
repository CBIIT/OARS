using TheradexPortal.Data.Models;

namespace TheradexPortal.Data.Services.Abstract
{
        public interface IWRCategoryService
        {
            public Task<IList<WRCategory>> GetCategories();
            public bool SaveCategories(IList<WRCategory> categories);

        }
}
