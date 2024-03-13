using TheradexPortal.Data.Models;

namespace TheradexPortal.Data.Services.Abstract
{
    public interface IProtocolMappingService
    {
        Task<IList<ProtocolMapping>> GetProtocolMappings();
        Task<bool> SaveProtocolMapping(ProtocolMapping protocolMapping, IList<ProtocolPhase> phasesSet);
        Task<IList<ProtocolMapping>> GetAllProtocolMappingsFromProfileType(int profileType);
    }
}
