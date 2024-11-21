using TheradexPortal.Data.Models;

namespace TheradexPortal.Data.Services.Abstract
{
    public interface IReviewService
    {
        public Task<List<ProtocolAgent>> GetAgentsByMappingId(int mappingId);
        public Task<bool> SaveAgent(ProtocolAgent agent, int mappingId);
        public Task<bool> DeleteAgent(ProtocolAgent agent);
    }
}
