﻿using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using System.Data;
using OARS.Data.Models;
using OARS.Data.Models.DTO;
using OARS.Data.Services.Abstract;

namespace OARS.Data.Services
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
        public async Task<int> GetDaysLateAsync(string protocolId)
        {
            return await context.ReviewHistories
                .Where(p=>p.ProtocolId == protocolId)
                .OrderByDescending(p=>p.ReviewHistoryId)
                .Select(r=>r.DaysLate)
                .FirstOrDefaultAsync() ?? 0;
        }

        public async Task<IList<int>> GetHistoryRecordsByProtocolAsync(string protocolId)
        {
            return await context.ReviewHistories
                .Where(p => p.ProtocolId == protocolId)
                .Select(r => r.ReviewHistoryId)
                .ToListAsync();
        }

        public async Task<ReviewHistory> GetLatestReviewHistoryByProtocolAsync(string protocolId, int userId, string reviewType)
        {
            return await context.ReviewHistories
                .AsNoTracking()
                .Where(p => p.ProtocolId == protocolId && p.UserId == userId && p.ReviewType == reviewType)
                .OrderByDescending(p => p.ReviewHistoryId)
                .FirstOrDefaultAsync();
        }

        public async Task<List<int>> GetAllReviewHistoryIdsByNameAsync(int reviewId, string ReviewPeriodName)
        {
            return await context.ReviewHistories
                .Where(r => r.ReviewId == reviewId && r.ReviewPeriodName == ReviewPeriodName)
                .Select(r => r.ReviewHistoryId)
                .ToListAsync();
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
            previousReview.MissedReviewCount = 0;
            previousReview.ReviewStatus = "Completed";
            previousReview.ReviewPeriod = previousReview.ReviewPeriodUpcoming;
            previousReview.NextDueDate = ((DateTime)previousReview.NextDueDate).AddDays(previousReview.ReviewPeriod);
            previousReview.ReviewPeriodName = UpdateReviewName(previousReview);

            previousReviewHistory.ReviewCompleteDate = DateTime.Now;
            previousReviewHistory.ReviewStatus = "Completed";
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

        public async Task<bool> StartNewReviewAsync(int userId, string protocolId, string reviewType, int reviewHistoryID)
        {
            var previousReviewHistory = await context.ReviewHistories
                .FirstOrDefaultAsync(r => r.ReviewHistoryId == reviewHistoryID);
            await _reviewService.ResetReviewAsync(userId, protocolId, reviewType);
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
            newHistory.ReviewStatus = "Active";

            newHistory.DaysLate = 0;
            newHistory.UpdateDate = DateTime.Now;

            newHistory.ProtocolId = protocolId;
            newHistory.ReviewPeriodName = previousReview.ReviewPeriodName;
            newHistory.ReviewId = previousReview.ReviewId;
            newHistory.MissedReviewCount = 0;
            
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

        public async Task<bool> SetReviewHistoryLateStatusAsync(int userId, string protocolId, string reviewType, int daysLate)
        {
            char lateStatus = (daysLate > 0) ? 'T' : 'F';

            ReviewHistory res = await context.ReviewHistories
                .Where(rh => rh.ReviewType == reviewType && rh.UserId == userId && rh.ProtocolId == protocolId)
                .FirstOrDefaultAsync();

            res.UpdateDate = DateTime.Now;
            res.DaysLate = daysLate;
            res.ReviewLate = lateStatus;

            var status = context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> SetMissedReviewHistoryCountAsync (int userId, string protocolId, string reviewType, int missedReviewCount)
        {
            ReviewHistory res = await context.ReviewHistories
                .Where(rh => rh.ReviewType == reviewType && rh.UserId == userId && rh.ProtocolId == protocolId && rh.ReviewCompleteDate == null)
                .FirstOrDefaultAsync();

            res.UpdateDate = DateTime.Now;
            res.MissedReviewCount = missedReviewCount;

            var status = context.SaveChangesAsync();

            return true;
        }
    }
}
