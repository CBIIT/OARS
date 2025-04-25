using OARS.Data.Models;
using OARS.Data.Models.DTO;

namespace OARS.Data.Services.Abstract
{
    public interface IReviewHistoryService
    {
        public Task<int> GetDaysLateAsync(string protocolId);

        public Task<IList<int>> GetHistoryRecordsByProtocolAsync(string protocolId);

        public int GetNextReviewHistoryId();

        public Task<ReviewHistory> GetLatestReviewHistoryByProtocolAsync(string protocolId, int userId, string reviewType);
        public Task<List<int>> GetAllReviewHistoryIdsByNameAsync(int reviewId, string ReviewPeriodName);
        public Task<bool> SaveNewReviewHistoryAsync(ReviewHistory reviewHistory);
        public Task<bool> CloseCurrentReviewAsync(int userId, int reviewHistoryID);
        public Task<bool> isReviewActive(int reviewHistoryID);
        public Task<bool> StartNewReviewAsync(int userId, string protocolId, string reviewType, int reviewHistoryID);
        public Task<bool> SetReviewHistoryLateStatusAsync(int userId, string protocolId, string reviewType, int daysLate);
        public Task<bool> SetMissedReviewHistoryCountAsync(int userId, string protocolId, string reviewType, int missedReviewCount);
    }
}
