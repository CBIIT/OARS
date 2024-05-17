using TheradexPortal.Data.Models;

namespace TheradexPortal.Data.Services.Abstract
{
    public interface IProtocolMappingService
    {
        Task<IList<ProtocolMapping>> GetProtocolMappings(bool includeArchived);
        Task<IList<ProtocolMapping>> GetProtocolMappings(List<string> studyIds, bool includeArchived);
        Task<IList<ProtocolMapping>> GetExistingProtocolMappings();
        Task<ProtocolMapping> GetProtocolMapping(int id);
        Task<bool> SaveProtocolMapping(ProtocolMapping protocolMapping, IList<ProtocolPhase> phasesSet);
        Task<IList<ProtocolMapping>> GetAllProtocolMappingsFromProfileType(int profileType);
        Task<bool> PublishProtocolMapping(int id);
        Task<bool> CopyMapping(int sourceId, int targetId);
        Task<IList<CrossoverOption>> GetCrossoverOptions();
    }
}
