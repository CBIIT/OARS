using TheradexPortal.Data.Models;

namespace TheradexPortal.Data.Services.Abstract
{
    public interface IReviewItemService
    {
        public Task<IList<ReviewItem>> GetReviewItemListAsync(string? reviewType, char? isActive);

        public Task<IList<ReviewItem>> GetActiveReviewItemsAsync();

        public Task<IList<ReviewItem>> GetReviewItemsByTypeAsync(string reviewType);

        public Task<bool> DeactivateReviewItemsAsync(int reviewItemId);

        public Task<ReviewItem> GetReviewItemByIdAsync(int? reviewItemId);

        public Task<bool> SaveReviewItemAsync(int activeUserId, ReviewItem item);
    }
}
