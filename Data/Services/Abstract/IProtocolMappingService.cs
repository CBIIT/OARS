using TheradexPortal.Data.Models;

namespace TheradexPortal.Data.Services.Abstract
{
    public interface IProtocolMappingService
    {
        Task<IList<ProtocolMapping>> GetProtocolMappings();
        Task<ProtocolMapping> SaveProtocolMapping(ProtocolMapping protocolMapping);
    }
}
