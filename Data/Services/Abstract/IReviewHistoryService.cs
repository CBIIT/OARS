using TheradexPortal.Data.Models;

namespace TheradexPortal.Data.Services.Abstract
{
    public interface IReviewHistoryService
    {
        public Task<int> GetDaysLateAsync(int protocolId);

        public Task<IList<int>> GetHistoryRecordsByProtocolAsync(int protocolId);
    }
}
