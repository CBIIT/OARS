using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using OARS.Data.Models;
using OARS.Data.Models.DTO;
using OARS.Data.Services.Abstract;

namespace OARS.Data.Services
{
    public class ReviewHistoryEmailService: BaseService, IReviewHistoryEmailService
    {
        private readonly IErrorLogService _errorLogService;
        private readonly NavigationManager _navManager;
        public ReviewHistoryEmailService(IDatabaseConnectionService databaseConnectionService, IErrorLogService errorLogService, NavigationManager navigationManager) : base(databaseConnectionService)
        {
            _errorLogService = errorLogService;
            _navManager = navigationManager;
        }

        public async Task<List<ReviewHistoryEmailDTO>> GetEmailsAsync(int historyId)
        {
            var result = await context.ReviewHistoryEmails
                .Where(rhe => rhe.ReviewHistoryId == historyId)
                .OrderByDescending(rhe => rhe.CreateDate)
                .Select(e => new ReviewHistoryEmailDTO
                    {
                        Body = e.EmailText,
                        Recipient = e.EmailToAddress,
                        CreationDate = e.CreateDate,
                    }
                )
                .ToListAsync<ReviewHistoryEmailDTO>();
            return result;
        }

        public async Task<string> GetAllEmailsAsync(int historyId)
        {
            return "";
        }

        public async Task<bool> SaveNewEmailAsync(int userId, int reviewHistoryId, string recipient, string body)
        {
            ReviewHistoryEmail newReviewHistoryEmail = new ReviewHistoryEmail();
            newReviewHistoryEmail.EmailToAddress = recipient;
            newReviewHistoryEmail.ReviewHistoryEmailId = GetNextReviewHistoryEmailId();
            newReviewHistoryEmail.ReviewHistoryId = reviewHistoryId;
            newReviewHistoryEmail.CreateDate = DateTime.Now;
            newReviewHistoryEmail.EmailText = body;

            await context.ReviewHistoryEmails.AddAsync(newReviewHistoryEmail);
            var primaryTable = context.Model.FindEntityType(typeof(ReviewHistory)).ToString().Replace("EntityType: ", "");

            var status = await context.SaveChangesAsync(userId, primaryTable);
            return true;
        }

        public async Task<List<int>> GetReviewHistoryEmailIdsAsync(int reviewHistoryID)
        {
            var queryResult = await context.ReviewHistoryEmails
                .Where(r => r.ReviewHistoryId == reviewHistoryID)
                .Select(r => r.ReviewHistoryEmailId)
                .ToListAsync();

            return queryResult;
        }

        public int GetNextReviewHistoryEmailId()
        {
            return context.ReviewHistoryEmails
                .DefaultIfEmpty()
                .Max(p => p == null ? 0 : p.ReviewHistoryEmailId) + 1;
        }
    }
}
