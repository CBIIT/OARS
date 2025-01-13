using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using System.Data;
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
        private readonly IUserService _userService;
        public ReviewHistoryService(IDatabaseConnectionService databaseConnectionService,
                                    IErrorLogService errorLogService,
                                    NavigationManager navigationManager,
                                    IReviewService reviewService,
                                    IUserService userService) : base(databaseConnectionService)
        {
            _errorLogService = errorLogService;
            _navManager = navigationManager;
            _reviewService = reviewService;
            _userService = userService;
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
            previousReview.NextDueDate = ((DateTime)previousReview.NextDueDate).AddDays(previousReview.ReviewPeriod);
            previousReview.ReviewPeriodName = UpdateReviewName(previousReview);

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

        private string UpdateReviewName(Review previousReview)
        {
            var previousReviewName = previousReview.ReviewPeriodName;
            string[] nameParts = previousReviewName.Split('-');
            int counter = int.Parse(nameParts[0]);
            int year = int.Parse(nameParts[1]);
            int section = int.Parse(nameParts[2]);

            counter++;
            
            if(year == DateTime.Now.Year)
            {
                section++;
            }
            else
            {
                year = DateTime.Now.Year;
                section = 1;
            }
            var newPeriodName = $"{counter:D3}-{year:D4}-{section:D2}";
            previousReview.ReviewPeriodName = newPeriodName;

            return newPeriodName;
        }

        public async Task<bool> StartNewReviewAsync(int userId, int protocolId, string reviewType, int reviewHistoryID)
        {
            var previousReviewHistory = await context.ReviewHistories
                .FirstOrDefaultAsync(r => r.ReviewHistoryId == reviewHistoryID);
            var previousReview = await _reviewService.GetCurrentReviewAsync(protocolId, userId, reviewType);

            var newHistory = new ReviewHistory();
            newHistory.ReviewHistoryId = GetNextReviewHistoryId();
            newHistory.UserId = userId;
            var user = await _userService.GetUserAsync(userId);

            newHistory.EmailAddress = user.EmailAddress;

            newHistory.CreateDate = DateTime.Now;
            newHistory.ReviewType = reviewType;
            newHistory.DueDate = previousReview.NextDueDate;

            newHistory.ReviewLate = 'F';
            newHistory.ReviewStatus = previousReview.ReviewStatus;
            newHistory.DaysLate = 0;
            newHistory.UpdateDate = DateTime.Now;

            newHistory.ProtocolId = protocolId;
            newHistory.ReviewPeriodName = previousReview.ReviewPeriodName;
            newHistory.ReviewId = previousReview.ReviewId;
            newHistory.IsWebReporting = 'F';
            await context.AddAsync(newHistory);

            var primaryTable = context.Model.FindEntityType(typeof(ReviewHistory)).ToString().Replace("EntityType: ", "");
            var status = context.SaveChangesAsync(userId, primaryTable);

            if(previousReviewHistory != null)
            {
                previousReview.NextDueDate = newHistory.DueDate;
                previousReview.UpdateDate = DateTime.Now;
            }

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
