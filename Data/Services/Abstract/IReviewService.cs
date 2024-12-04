using TheradexPortal.Data.Models;

namespace TheradexPortal.Data.Services.Abstract
{
    public interface IReviewService
    {
        public Task<string> GetLeadAgentByIdAsync(int protocolId);
    }
}
