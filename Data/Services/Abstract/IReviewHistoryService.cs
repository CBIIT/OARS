using TheradexPortal.Data.Models;

namespace TheradexPortal.Data.Services.Abstract
{
    public interface IReviewHistoryService
    {
        public Task<int> GetDaysLateAsync(int protocolId);

        public Task<IList<int>> GetHistoryRecordsByProtocolAsync(int protocolId);

        public int GetNextReviewHistoryId();

        public Task<ReviewHistory> GetLatestReviewHistoryByProtocolAsync(int protocolId);
        public Task<bool> SaveNewReviewHistoryAsync(ReviewHistory reviewHistory);

        public Task<bool> StartNewReviewAsync(int reviewHistoryID);
    }
}
