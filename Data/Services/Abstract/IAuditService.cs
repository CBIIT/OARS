using TheradexPortal.Data.Models;
using TheradexPortal.Data.Models.DTO;

namespace TheradexPortal.Data.Services.Abstract
{
    public interface IAuditService
    {
        public Task<List<AuditTrailDTO>> GetFullAuditTrailAsync(int userId, int reviewId);
    }
}
