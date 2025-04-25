using OARS.Data.Models;
using OARS.Data.Models.DTO;

namespace OARS.Data.Services.Abstract
{
    public interface IAuditService
    {
        public Task<List<AuditTrailDTO>> GetFullAuditTrailAsync(int userId, int reviewId, int reviewHistoryId, string reviewType, List<int> reviewHistoryItemIds, 
            List<int> reviewHistoryNoteIds, List<int> reviewHistoryEmailIds);
    }
}
