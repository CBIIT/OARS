using TheradexPortal.Data.Models;
using TheradexPortal.Data.Models.DTO;

namespace TheradexPortal.Data.Services.Abstract
{
    public interface IReviewHistoryService
    {
        public Task<int> GetDaysLateAsync(int protocolId);

        public Task<IList<int>> GetHistoryRecordsByProtocolAsync(int protocolId);

        public int GetNextReviewHistoryId();

        public Task<ReviewHistory> GetLatestReviewHistoryByProtocolAsync(int protocolId, int userId, string reviewType);
        public Task<bool> SaveNewReviewHistoryAsync(ReviewHistory reviewHistory);
        public Task<bool> CloseCurrentReviewAsync(int userId, int reviewHistoryID);
        public Task<bool> isReviewActive(int reviewHistoryID);
        public Task<bool> StartNewReviewAsync(int userId, int protocolId, string reviewType, int reviewHistoryID);
        public Task<bool> SetReviewHistoryLateStatusAsync(int userId, int protocolId, string reviewType, int daysLate);
    }
}
