using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using TheradexPortal.Data.Models;
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
    }
}
