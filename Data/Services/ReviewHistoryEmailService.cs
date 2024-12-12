using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using TheradexPortal.Data.Models;
using TheradexPortal.Data.Models.DTO;
using TheradexPortal.Data.Services.Abstract;

namespace TheradexPortal.Data.Services
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
        public async Task<ReviewHistoryEmailDTO> GetSingleEmailAsync(int historyId)
        {
            var result =  await context.ReviewHistoryEmails
                .Where(p => p.ReviewHistoryId == historyId)
                .OrderBy(d => d.CreateDate)
                .Select(r => new ReviewHistoryEmailDTO
                {
                    Body = r.EmailText,
                    CreationDate = r.CreateDate,
                    Recipient = r.EmailToAddress
                }
                )
                .FirstOrDefaultAsync();
            return result;
        }

        public async Task<string> GetAllEmailsAsync(int historyId)
        {
            return "";
        }

        public async Task<bool> SaveNewEmailAsync(int reviewHistoryId, string recipient, string body)
        {
            ReviewHistoryEmail newReviewHistoryEmail = new ReviewHistoryEmail();
            newReviewHistoryEmail.EmailToAddress = recipient;
            newReviewHistoryEmail.ReviewHistoryEmailId = GetNextReviewHistoryEmailId();
            newReviewHistoryEmail.ReviewHistoryId = reviewHistoryId;
            newReviewHistoryEmail.CreateDate = DateTime.Now;
            newReviewHistoryEmail.EmailText = body;

            await context.ReviewHistoryEmails.AddAsync(newReviewHistoryEmail);
            var status = await context.SaveChangesAsync();
            return true;
        }

        public int GetNextReviewHistoryEmailId()
        {
            return context.ReviewHistoryEmails.Max(p => p.ReviewHistoryEmailId) + 1;
        }
    }
}
