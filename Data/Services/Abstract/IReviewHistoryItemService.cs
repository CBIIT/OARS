using TheradexPortal.Data.Models;
using TheradexPortal.Data.Models.DTO;

namespace TheradexPortal.Data.Services.Abstract
{
    public interface IReviewHistoryItemService
    {
        public Task<bool> CheckHistoryItemChangedAsync(int reviewHistoryItemId);
        public Task<Dictionary<int, bool>> GetCurrentReviewHistoryItemStatusAsync(int reviewHistoryID);
        public Task<bool> SaveReviewHistoryItemAsync(int activeUserId, int reviewHistoryID, int reviewItemID, bool newValue);
        public Task<bool> UpdateReviewHistoryItemAsync(int activeUserId, int reviewHistoryID, int reviewItemID, bool newValue);
        public int GetNextReviewHistoryItemId();
        public Task<List<int>> GetReviewHistoryItemIdsAsync(int reviewHistoryID);
        public Task<List<ReviewItem>> GetReviewHistoryItemsAsync(int reviewHistoryID);
        public Task<string> GetReviewHistoryItemNameAsync(int reviewItemId);
    }
}
