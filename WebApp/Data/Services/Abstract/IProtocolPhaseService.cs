using OARS.Data.Models;

namespace OARS.Data.Services.Abstract
{
    public interface IProtocolPhaseService
    {
        Task<IList<ProtocolPhase>> GetProtocolMappingPhases(int protocolMapping);
        Task<bool> SaveProtocolPhase(ProtocolPhase protocolPhase);
    }
}