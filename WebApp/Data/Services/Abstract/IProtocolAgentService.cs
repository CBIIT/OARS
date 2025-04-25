using OARS.Data.Models;

namespace OARS.Data.Services.Abstract
{
    public interface IProtocolAgentService
    {
        public Task<List<ProtocolAgent>> GetAgentsByMappingId(int mappingId);
        public Task<bool> SaveAgent(ProtocolAgent agent, int mappingId);
        public Task<bool> DeleteAgent(ProtocolAgent agent);
    }
}
