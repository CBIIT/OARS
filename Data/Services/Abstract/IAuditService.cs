using TheradexPortal.Data.Models;

namespace TheradexPortal.Data.Services.Abstract
{
    public interface IAuditService
    {
        public Task<IList<Audit>> GetReviewAuditTrailAsync(int userId, int reviewId);
    }
}
