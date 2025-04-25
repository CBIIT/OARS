using Oracle.EntityFrameworkCore.Query.Internal;
using OARS.Data.Models;
using OARS.Data.Models.DTO;

namespace OARS.Data.Services.Abstract
{
    public interface IReviewHistoryEmailService
    {
        public Task<List<ReviewHistoryEmailDTO>> GetEmailsAsync(int reviewHistoryId);
        public Task<string> GetAllEmailsAsync(int reviewHistoryId);
        public Task<bool> SaveNewEmailAsync(int userId, int reviewHistoryId, string recipient, string body);
        public int GetNextReviewHistoryEmailId();
        public Task<List<int>> GetReviewHistoryEmailIdsAsync(int reviewHistoryID);
    }
}
