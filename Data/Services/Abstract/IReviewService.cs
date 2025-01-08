using TheradexPortal.Data.Models;

namespace TheradexPortal.Data.Services.Abstract
{
    public interface IReviewService
    {
        public Task<string> GetLeadAgentByIdAsync(int protocolId);
        public Task<List<int>> GetActivePIReviewsAsync(int protocolId);

        public Task<Review> GetCurrentReviewAsync(int protocolId);
    }
}
