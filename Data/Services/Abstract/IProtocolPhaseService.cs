using TheradexPortal.Data.Models;

namespace TheradexPortal.Data.Services.Abstract
{
    public interface IProtocolPhaseService
    {
        Task<IList<ProtocolPhase>> GetProtocolMappingPhases(int protocolMapping);
        Task<bool> SaveProtocolPhase(ProtocolPhase protocolPhase);
    }
}