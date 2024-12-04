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

        public async Task<IList<ReviewItem>> GetReviewItemListAsync(string? reviewType = "", char? isActive = null)
        {
            var query = context.Set<ReviewItem>().AsQueryable();
            if (!string.IsNullOrEmpty(reviewType))
            {
                query = query.Where(item =>  item.ReviewType == reviewType);
            }
            if(isActive.HasValue)
            {
                query = query.Where(item => item.IsActive == isActive.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<IList<ReviewItem>> GetActiveReviewItemsAsync()
        {
            return await context.ReviewItems.Where(p=>p.IsActive == 'T' && p.ReviewType == "PI").ToListAsync();
        }

        public async Task<IList<ReviewItem>> GetReviewItemsByTypeAsync(string reviewType)
        {
            return await context.ReviewItems.Where(p => p.ReviewType == reviewType).ToListAsync();
        }
        public async Task<bool> DeactivateReviewItemsAsync(int reviewItemId)
        {
            var res =  await context.ReviewItems.Where(p => p.ReviewItemId == reviewItemId).FirstOrDefaultAsync();
            res.IsActive = 'F';
            context.SaveChanges();
            return true;
        }
        public async Task<ReviewItem> GetReviewItemByIdAsync(int? reviewItemId)
        {
            return await context.ReviewItems.Where(p => p.ReviewItemId == reviewItemId).FirstOrDefaultAsync();
        }

        public async Task<bool> SaveReviewItemAsync(ReviewItem item)
        {
            if (item.ReviewItemId > 0)
            {
                var itemToUpdate = await context.ReviewItems.FirstOrDefaultAsync(ri => ri.ReviewItemId == item.ReviewItemId);
                if (itemToUpdate != null)
                {
                    itemToUpdate.ReviewType = item.ReviewType;
                    itemToUpdate.IsActive = item.IsActive;
                    itemToUpdate.ReviewItemName = item.ReviewItemName;
                    itemToUpdate.UpdateDate = item.UpdateDate;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                int maxValue = context.ReviewItems.Max(ri => ri.ReviewItemId);
                item.ReviewItemId = maxValue + 1;
                context.ReviewItems.Add(item);
            }
            if( await context.SaveChangesAsync() > 0)
                return true;
            return false;
        }

    }
}
