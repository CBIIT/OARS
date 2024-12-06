using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using TheradexPortal.Data.Models.DTO;
using TheradexPortal.Data.Services.Abstract;
using TheradexPortal.Data.Models;

namespace TheradexPortal.Data.Services
{
    public class ReviewHistoryNoteService: BaseService, IReviewHistoryNoteService
    {
        private readonly IErrorLogService _errorLogService;
        private readonly NavigationManager _navManager;
        public ReviewHistoryNoteService(IDatabaseConnectionService databaseConnectionService, IErrorLogService errorLogService, NavigationManager navigationManager) : base(databaseConnectionService)
        {
            _errorLogService = errorLogService;
            _navManager = navigationManager;
        }
        public async Task<List<ReviewHistoryNoteDTO>> GetNotesAsync(int historyId)
        {
            var result = await context.ReviewHistoryNotes
                .Where(rhn => rhn.ReviewHistoryId == historyId)
                .OrderBy(rhn => rhn.CreateDate)
                .Join(context.ReviewHistories,
                      rhn => rhn.ReviewHistoryId,
                      rh => rh.ReviewHistoryId,
                      (rhn, rh) => new
                      {
                          Id = rh.UserId,
                          Notes = rhn.NoteText!,
                          CreationDate = rhn.CreateDate
                      })
                .Join(context.Users,
                    temp => temp.Id,
                    u => u.UserId,
                    (temp, u) => new ReviewHistoryNoteDTO
                    {
                        UserName = u.FirstName + " " + u.LastName,
                        Notes = temp.Notes,
                        CreationDate = temp.CreationDate,
                    }
                )
                .ToListAsync<ReviewHistoryNoteDTO>();
            return result;
        }

        public async Task<string> GetAllNotesAsync(int historyId)
        {
            return "";
        }

        public Task<bool> SaveNoteAsync(ReviewHistoryNote note)
        {
            context.AddAsync(note);
            var status = context.SaveChangesAsync();
            return Task.FromResult(true);
        }

        public int GetNextReviewHistoryNoteId()
        {
            return context.ReviewHistoryNotes.Max(p => p.ReviewHistoryNoteId) + 1;
        }
    }
}
