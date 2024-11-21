using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using TheradexPortal.Data.Models;
using TheradexPortal.Data.Services.Abstract;

namespace TheradexPortal.Data.Services
{
    public class ReviewItemService: BaseService, IReviewItemService
    {
        private readonly IErrorLogService _errorLogService;
        private readonly NavigationManager _navManager;
        public ReviewItemService(IDatabaseConnectionService databaseConnectionService, IErrorLogService errorLogService, NavigationManager navigationManager) : base(databaseConnectionService)
        {
            _errorLogService = errorLogService;
            _navManager = navigationManager;
        }
        public async Task<IList<ReviewItem>> GetActiveReviewItemsAsync()
        {
            return await context.ReviewItems.Where(p=>p.IsActive == 'T' && p.ReviewType == "PI").ToListAsync();
        }

        public async Task<IList<ReviewItem>> GetReviewItemsByTypeAsync(string reviewType)
        {
            return await context.ReviewItems.Where(p => p.ReviewType == reviewType).ToListAsync();
        }
    }
}
