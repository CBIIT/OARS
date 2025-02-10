using TheradexPortal.Data.Models;
using TheradexPortal.Data.Models.DTO;

namespace TheradexPortal.Data.Services.Abstract
{
    public interface IReviewService
    {
        public Task<string> GetLeadAgentByIdAsync(int protocolId);
        public Task<List<int>> GetActivePIReviewsAsync(int protocolId);

        public Task<Review> GetCurrentReviewAsync(int protocolId, int userId, string type);

        public Task<List<int>> GetAllAuthorizedUsersAsync(int protocolId);
        public Task<List<ReviewPiDTO>> GetPiInfoAsync(int protocolId);
        public Task<(int, int)> GetReviewDurationsAsync(int protocolId);
        public Task<bool> SetReviewDurationsAsync(int userId, int protocolId, int MOReviewPeriod, int PIReviewPeriod);
        public Task<bool> SetMissedReviewCountAsync(int userId, int protocolId, string reviewType, int missedReviewCount);
    }
}
