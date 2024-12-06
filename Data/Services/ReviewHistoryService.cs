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
    }
}
