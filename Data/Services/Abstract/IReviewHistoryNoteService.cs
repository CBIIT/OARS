using TheradexPortal.Data.Models;
using TheradexPortal.Data.Models.DTO;

namespace TheradexPortal.Data.Services.Abstract
{
    public interface IReviewHistoryNoteService
    {
        public Task<List<ReviewHistoryNoteDTO>> GetNotesAsync(int reviewHistoryID);
        public Task<string> GetAllNotesAsync(int reviewHistoryID);
        public Task<bool> SaveNoteAsync(int userId, ReviewHistoryNote note);
        public Task<List<int>> GetReviewHistoryNoteIdsAsync(int reviewHistoryID);

        public int GetNextReviewHistoryNoteId();
    }
}
