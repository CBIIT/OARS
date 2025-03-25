using TheradexPortal.Data.Models;
using TheradexPortal.Data.Models.DTO;

namespace TheradexPortal.Data.Services.Abstract
{
    public interface IReviewService
    {
        public Task<string> GetLeadAgentByIdAsync(string protocolId);
        public Task<List<int>> GetActivePIReviewsAsync(string protocolId);

        public Task<Review> GetCurrentReviewAsync(string protocolId, int userId, string type);
        public Task<List<int>> GetAllAuthorizedUsersAsync(string protocolId);
        public Task<List<ReviewPiDTO>> GetPiInfoAsync(string protocolId);
        public Task<(int, int)> GetReviewDurationsAsync(string protocolId);
        public Task<bool> SetReviewDurationsAsync(int userId, string protocolId, int MOReviewPeriod, int PIReviewPeriod);
        public Task<bool> SetMissedReviewCountAsync(int userId, string protocolId, string reviewType, int missedReviewCount);
        public Task<bool> ResetReviewAsync(int userId, string protocolId, string reviewType);
        public Task<(int, int)> GetPiAndMoOverdueReviewCountsAsync(int userId);
        public Task<(int, int)> GetPiAndMoUpcomingReviewCountsAsync(int userId);
    }
}
