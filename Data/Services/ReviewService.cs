using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using TheradexPortal.Data.Models;
using TheradexPortal.Data.Models.ADDR;
using TheradexPortal.Data.Models.DTO;
using TheradexPortal.Data.Services.Abstract;

namespace TheradexPortal.Data.Services
{
    public class ReviewService: BaseService, IReviewService
    {
        private readonly IErrorLogService _errorLogService;
        private readonly NavigationManager _navManager;
        public ReviewService(IDatabaseConnectionService databaseConnectionService, IErrorLogService errorLogService, NavigationManager navigationManager) : base(databaseConnectionService)
        {
            _errorLogService = errorLogService;
            _navManager = navigationManager;
        }
        public async Task<string> GetLeadAgentByIdAsync(int protocolId)
        {
            return await context.Reviews
                .Where(p=>p.ProtocolId == protocolId)
                .Select(a=>a.AgentName)
                .FirstOrDefaultAsync() ?? "";
        }

        public async Task<List<int>> GetActivePIReviewsAsync(int protocolId)
        {
            return await context.Reviews
                .Where(p => p.ProtocolId == protocolId && p.ReviewType == "PI" && p.ReviewStatus == "Active")
                .Select(r => r.ReviewId)
                .ToListAsync();
        }

        public async Task<Review> GetCurrentReviewAsync(int protocolId, int userId, string type)
        {
            return await context.Reviews
                .Where(p => 
                p.ProtocolId == protocolId &&
                p.UserId == userId &&
                p.ReviewType == type)
                .FirstOrDefaultAsync();
        }

        public async Task<(int, int)> GetReviewDurationsAsync(int protocolId)
        {
            var reviewPeriods = await context.Reviews
                .Where(r => r.ProtocolId == protocolId && (r.ReviewType == "MO" || r.ReviewType == "PI"))
                .Select(r => new { r.ReviewType, r.ReviewPeriodUpcoming })
                .ToListAsync();

            // Get the first "MO" and "PI" ReviewPeriod
            var moReviewPeriod = reviewPeriods.FirstOrDefault(r => r.ReviewType == "MO")?.ReviewPeriodUpcoming ?? 30;
            var piReviewPeriod = reviewPeriods.FirstOrDefault(r => r.ReviewType == "PI")?.ReviewPeriodUpcoming ?? 30;

            return (moReviewPeriod, piReviewPeriod);

        }

        public async Task<bool> SetReviewDurationsAsync(int userId, int protocolId, int MOReviewPeriod, int PIReviewPeriod)
        {
            List<Review> reviewItem = await context.Reviews
                .Where(r=>r.ProtocolId == protocolId)
                .ToListAsync();

            foreach (var review in reviewItem)
            {
                if (review.ReviewType == "MO")
                {
                    review.ReviewPeriodUpcoming = MOReviewPeriod;
                }
                else
                {
                    review.ReviewPeriodUpcoming = PIReviewPeriod;
                }
                review.UpdateDate = DateTime.Now;
            }
            var primaryTable = context.Model.FindEntityType(typeof(Review)).ToString().Replace("EntityType: ", "");
            context.SaveChangesAsync(userId, primaryTable);

            return true;
        }

        public async Task<List<int>> GetAllAuthorizedUsersAsync(int protocolId)
        {
            return await context.Reviews
                .Where(p=> p.ProtocolId == protocolId)
                .Select (r => r.UserId)
                .ToListAsync();
        }

        public async Task<List<ReviewPiDTO>> GetPiInfoAsync(int protocolId)
        {
            var lstPiReviews = await GetActivePIReviewsAsync(protocolId);

            var query = context.Reviews
                    .Where(r => lstPiReviews.Contains(r.ReviewId))
                    .Join(context.Users,
                    r => r.UserId,
                    u => u.UserId,
                    (r, u) => new ReviewPiDTO
                    {
                        PiName = u.FirstName + " " + u.LastName,
                        caseNumber = r.ProtocolId.ToString(),
                        dueDate = r.NextDueDate.ToString(),
                        updateDate = r.UpdateDate,
                        currentStatus = r.ReviewStatus,
                        periodName = r.ReviewPeriodName
                    });

            var ret = await query.ToListAsync(); ;

            return ret;
        }

        public async Task<(int, int)> GetPiAndMoOverdueReviewCountAsync(int userId)
        {
            var PiOverdueList = await context.Reviews
                .Where(r => r.UserId == userId && r.ReviewType == "PI" 
                && r.NextDueDate != null && r.NextDueDate < DateTime.Today)
                .ToListAsync();

            var MoOverdueList = await context.Reviews
                .Where(r => r.UserId == userId && r.ReviewType == "MO" 
                && r.NextDueDate != null && r.NextDueDate < DateTime.Today)
                .ToListAsync();

            (int, int) PiAndMoOverdueCount = (PiOverdueList.Count, MoOverdueList.Count);
            return PiAndMoOverdueCount;
        }

    }
}
