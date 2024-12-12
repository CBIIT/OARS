using Oracle.EntityFrameworkCore.Query.Internal;
using TheradexPortal.Data.Models;
using TheradexPortal.Data.Models.DTO;

namespace TheradexPortal.Data.Services.Abstract
{
    public interface IReviewHistoryEmailService
    {
        public Task<ReviewHistoryEmailDTO> GetSingleEmailAsync(int protocolId);
        public Task<string> GetAllEmailsAsync(int protocolId);
        public Task<bool> SaveNewEmailAsync(int reviewHistoryId, string recipient, string body);
        public int GetNextReviewHistoryEmailId();
    }
}
