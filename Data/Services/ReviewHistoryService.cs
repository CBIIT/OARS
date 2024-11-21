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
    }
}
