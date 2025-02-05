using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using TheradexPortal.Data.Models.DTO;
using TheradexPortal.Data.Services.Abstract;
using TheradexPortal.Data.Models;

namespace TheradexPortal.Data.Services
{
    public class ReviewHistoryItemService: BaseService, IReviewHistoryItemService
    {
        private readonly IErrorLogService _errorLogService;
        private readonly NavigationManager _navManager;
        public ReviewHistoryItemService(IDatabaseConnectionService databaseConnectionService, IErrorLogService errorLogService, NavigationManager navigationManager) : base(databaseConnectionService)
        {
            _errorLogService = errorLogService;
            _navManager = navigationManager;
        }

        public async Task<Dictionary<int, bool>> GetCurrentReviewHistoryItemStatusAsync(int reviewHistoryID)
        {
            var queryResult = await context.ReviewHistoryItems
                .Where(r => r.ReviewHistoryId == reviewHistoryID)
                .ToDictionaryAsync(
                r => r.ReviewItemId,
                r => r.IsCompleted == 'T'
                );

            return queryResult;
        }

        public async Task<List<int>> GetReviewHistoryItemIdsAsync(int reviewHistoryID)
        {
            var queryResult = await context.ReviewHistoryItems
                .Where(r => r.ReviewHistoryId == reviewHistoryID)
                .Select(r => r.ReviewHistoryItemId)
                .ToListAsync();

            return queryResult;
        }

        public async Task<bool> CheckHistoryItemChangedAsync(int reviewHistoryItemId)
        {
            return true;
        }

        public async Task<bool> SaveReviewHistoryItemAsync(int activeUserId, int reviewHistoryID, int reviewItemID, bool newValue)
        {
            ReviewHistoryItem newReviewHistoryItem = new ReviewHistoryItem();
            newReviewHistoryItem.ReviewHistoryItemId = GetNextReviewHistoryItemId();
            newReviewHistoryItem.ReviewHistoryId = reviewHistoryID;
            newReviewHistoryItem.ReviewItemId = reviewItemID;
            newReviewHistoryItem.IsCompleted = newValue ? 'T' : 'F';
            newReviewHistoryItem.CreateDate = DateTime.Now;
            newReviewHistoryItem.UpdateDate = DateTime.Now;
            var primaryTable = context.Model.FindEntityType(typeof(ReviewHistory)).ToString().Replace("EntityType: ", "");
            await context.ReviewHistoryItems.AddAsync(newReviewHistoryItem);
            var status = await context.SaveChangesAsync(activeUserId, primaryTable);
            return true;
        }

        public async Task<bool> UpdateReviewHistoryItemAsync(int activeUserId, int reviewHistoryID, int reviewItemID, bool newValue)
        {
            var reviewItem = await context.ReviewHistoryItems
                .FirstOrDefaultAsync(r => r.ReviewHistoryId == reviewHistoryID && r.ReviewItemId == reviewItemID);
            reviewItem.IsCompleted = newValue ? 'T' : 'F';
            reviewItem.UpdateDate = DateTime.Now;
            var primaryTable = context.Model.FindEntityType(typeof(ReviewHistory)).ToString().Replace("EntityType: ", "");
            await context.SaveChangesAsync(activeUserId, primaryTable);
            return true;
        }

        public int GetNextReviewHistoryItemId()
        {
            return context.ReviewHistoryItems.Max(p => p.ReviewHistoryItemId) + 1;
        }

        public async Task<string> GetReviewHistoryItemNameAsync(int reviewHistoryItemId)
        {
            var itemId = await context.ReviewHistoryItems
                .Where(r => r.ReviewHistoryItemId == reviewHistoryItemId)
                .Select(r => r.ReviewItemId)
                .FirstOrDefaultAsync();

            return await context.ReviewItems
                .Where(i => i.ReviewItemId == itemId)
                .Select(i => i.ReviewItemName)
                .FirstOrDefaultAsync() ?? "";
        }
    }
}
