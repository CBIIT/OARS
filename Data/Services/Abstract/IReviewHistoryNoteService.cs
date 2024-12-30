using TheradexPortal.Data.Models;
using TheradexPortal.Data.Models.DTO;

namespace TheradexPortal.Data.Services.Abstract
{
    public interface IReviewHistoryNoteService
    {
        public Task<List<ReviewHistoryNoteDTO>> GetNotesAsync(int protocolId);
        public Task<string> GetAllNotesAsync(int protocolId);
        public Task<bool> SaveNoteAsync(int userId, ReviewHistoryNote note);
        public int GetNextReviewHistoryNoteId();
    }
}
