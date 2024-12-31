using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using TheradexPortal.Data.Models;
using TheradexPortal.Data.Models.DTO;
using TheradexPortal.Data.Services.Abstract;

namespace TheradexPortal.Data.Services
{
    public class ReviewHistoryService: BaseService, IReviewHistoryService
    {
        private readonly IErrorLogService _errorLogService;
        private readonly NavigationManager _navManager;
        private readonly IReviewService _reviewService;
        public ReviewHistoryService(IDatabaseConnectionService databaseConnectionService,
                                    IErrorLogService errorLogService,
                                    NavigationManager navigationManager,
                                    IReviewService reviewService) : base(databaseConnectionService)
        {
            _errorLogService = errorLogService;
            _navManager = navigationManager;
            _reviewService = reviewService;
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

        public async Task<bool> CloseCurrentReviewAsync(int userId, int reviewHistoryID)
        {
            var previousReviewHistory = await context.ReviewHistories
                .FirstOrDefaultAsync(r => r.ReviewHistoryId == reviewHistoryID);
            var previousReview = await context.Reviews
                .FirstOrDefaultAsync(r => r.ReviewId == previousReviewHistory.ReviewId);

            previousReview.UpdateDate = DateTime.Now;
            previousReviewHistory.ReviewCompleteDate = DateTime.Now;
            var primaryTable = context.Model.FindEntityType(typeof(ReviewHistory)).ToString().Replace("EntityType: ", "");
            var status = context.SaveChangesAsync(userId, primaryTable);

            return true;
        }

        public async Task<bool> isReviewActive(int reviewHistoryID)
        {
            var previousReviewHistory = await context.ReviewHistories
                .FirstOrDefaultAsync(r => r.ReviewHistoryId == reviewHistoryID);
            if (previousReviewHistory.ReviewCompleteDate == null)
                return true;
            return false;
        }


        public async Task<bool> StartNewReviewAsync(int userId, int reviewHistoryID)
        {
            var previousReviewHistory = await context.ReviewHistories
                .FirstOrDefaultAsync(r => r.ReviewHistoryId == reviewHistoryID);
            var previousReview = await context.Reviews
                .FirstOrDefaultAsync(r => r.ReviewId == previousReviewHistory.ReviewId);
            var newHistory = new ReviewHistory();
            newHistory.ReviewHistoryId = GetNextReviewHistoryId();
            newHistory.UserId = previousReviewHistory.UserId;
            newHistory.EmailAddress = previousReviewHistory.EmailAddress;

            newHistory.CreateDate = DateTime.Now;
            newHistory.ReviewType = previousReviewHistory.ReviewType;
            newHistory.DueDate = ((DateTime)previousReviewHistory.DueDate).AddDays(previousReview.ReviewPeriod);
            newHistory.ReviewLate = 'F';
            newHistory.ReviewStatus = previousReviewHistory.ReviewStatus;
            newHistory.DaysLate = 0;
            newHistory.UpdateDate = DateTime.Now;
            newHistory.ProtocolId = previousReviewHistory.ProtocolId;
            //Need to update Review Name
            newHistory.ReviewId = previousReviewHistory.ReviewId;
            await context.AddAsync(newHistory);
            var primaryTable = context.Model.FindEntityType(typeof(ReviewHistory)).ToString().Replace("EntityType: ", "");
            var status = context.SaveChangesAsync(userId, primaryTable);

            return true;
        }

        public async Task<List<ReviewHistoryPiDTO>> GetPiInfoAsync(int protocolId)
        {
            var lstPiReviews = await _reviewService.GetActivePIReviewsAsync(protocolId);

            var query = context.ReviewHistories
                    .Where(rh => lstPiReviews.Contains(rh.ReviewId ?? 0) && rh.ReviewCompleteDate == null)
                    .Join(context.Users,
                    rh => rh.UserId,
                    u => u.UserId,
                    (rh, u) => new ReviewHistoryPiDTO
                    {
                        PiName = u.FirstName + " " + u.LastName,
                        caseNumber = rh.ProtocolId.ToString(),
                        updateDate = rh.UpdateDate,
                        currentStatus = rh.ReviewStatus
                    });

            var ret = await query.ToListAsync(); ;

            return ret;
        }
    }
}
