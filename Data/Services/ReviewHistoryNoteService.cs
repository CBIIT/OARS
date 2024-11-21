using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using TheradexPortal.Data.Models.DTO;
using TheradexPortal.Data.Services.Abstract;

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
        public async Task<ReviewHistoryNoteDTO> GetSingleNoteAsync(int historyId)
        {
            var result =  await context.ReviewHistoryNotes
                .Where(rhn => rhn.ReviewHistoryId == historyId)
                .OrderBy(rhn => rhn.CreateDate)
                .Join(context.ReviewHistories,
                      rhn => rhn.ReviewHistoryId,
                      rh => rh.ReviewHistoryId,
                      (rhn, rh) => new {
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
                .FirstOrDefaultAsync();
            return result;
        }

        public async Task<string> GetAllNotesAsync(int historyId)
        {
            return "";
        }
    }
}
