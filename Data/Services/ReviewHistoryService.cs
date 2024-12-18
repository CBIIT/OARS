using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using TheradexPortal.Data.Models;
using TheradexPortal.Data.Services.Abstract;

namespace TheradexPortal.Data.Services
{
    public class ReviewHistoryService: BaseService, IReviewHistoryService
    {
        private readonly IErrorLogService _errorLogService;
        private readonly NavigationManager _navManager;
        public ReviewHistoryService(IDatabaseConnectionService databaseConnectionService, IErrorLogService errorLogService, NavigationManager navigationManager) : base(databaseConnectionService)
        {
            _errorLogService = errorLogService;
            _navManager = navigationManager;
        }
        public async Task<int> GetDaysLateAsync(int protocolId)
        {
            return await context.ReviewHistories
                .Where(p=>p.ProtocolId == protocolId)
                .OrderByDescending(p=>p.ReviewHistoryId)
                .Select(r=>r.DaysLate)
                .FirstOrDefaultAsync() ?? 0;
        }

        public async Task<IList<int>> GetHistoryRecordsByProtocolAsync(int protocolId)
        {
            return await context.ReviewHistories
                .Where(p => p.ProtocolId == protocolId)
                .Select(r => r.ReviewHistoryId)
                .ToListAsync();
        }

        public async Task<ReviewHistory> GetLatestReviewHistoryByProtocolAsync(int protocolId)
        {
            return await context.ReviewHistories
                .AsNoTracking()
                .Where(p => p.ProtocolId == protocolId)
                .OrderByDescending(p => p.ReviewHistoryId)
                .FirstOrDefaultAsync();
        }

        public int GetNextReviewHistoryId()
        {
            return context.ReviewHistories.Max(p => p.ReviewHistoryId) + 1;
        }

        public Task<bool> SaveNewReviewHistoryAsync(ReviewHistory reviewHistory)
        {
            context.AddAsync(reviewHistory);
            var status = context.SaveChangesAsync();
            return Task.FromResult(true);
        }

        private void CloseCurrentReviewAsync(ReviewHistory previousHistory)
        {
            previousHistory.UpdateDate = DateTime.Now;
            previousHistory.ReviewCompleteDate = DateTime.Now;
            return;
        }

        public async Task<bool> StartNewReviewAsync(int reviewHistoryID)
        {
            var previousReviewHistory = await context.ReviewHistories
                .FirstOrDefaultAsync(r => r.ReviewHistoryId == reviewHistoryID);
            CloseCurrentReviewAsync (previousReviewHistory);
            var newHistory = new ReviewHistory();
            newHistory.ReviewHistoryId = GetNextReviewHistoryId();
            newHistory.UserId = previousReviewHistory.UserId;
            newHistory.EmailAddress = previousReviewHistory.EmailAddress;
            newHistory.CreateDate = previousReviewHistory.CreateDate;
            newHistory.ReviewType = previousReviewHistory.ReviewType;
            //Need to calcualte new review due date
            newHistory.ReviewLate = 'F';
            newHistory.ReviewStatus = previousReviewHistory.ReviewStatus;
            newHistory.DaysLate = 0;
            newHistory.UpdateDate = DateTime.Now;
            newHistory.ProtocolId = previousReviewHistory.ProtocolId;
            //Need to update Review Name
            newHistory.ReviewId = previousReviewHistory.ReviewId;
            await context.AddAsync(newHistory);
            var status = context.SaveChangesAsync();
            return true;
        }
    }
}
