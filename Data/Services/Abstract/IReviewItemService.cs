using TheradexPortal.Data.Models;

namespace TheradexPortal.Data.Services.Abstract
{
    public interface IReviewItemService
    {
        public Task<IList<ReviewItem>> GetActiveReviewItemsAsync();

        public Task<IList<ReviewItem>> GetReviewItemsByTypeAsync(string reviewType);
    }
}
