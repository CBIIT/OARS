using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using TheradexPortal.Data.Models;
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

        public async Task<Review> GetCurrentReviewAsync(int protocolId)
        {
            return await context.Reviews
                .Where(p => p.ProtocolId == protocolId)
                .FirstOrDefaultAsync();
        }
    }
}
