using TheradexPortal.Data.Models;

namespace TheradexPortal.Data.Services.Abstract
{
    public interface IProtocolFormMappingService
    {
        Task<IList<ProtocolFormMapping>> GetProtocolFormMappings();
        Task<ProtocolFormMapping> GetProtocolFormMapping(int id);
        Task<IList<ProtocolFormMapping>> GetProtocolFormMappings(int formId);
        Task<IList<ProtocolFormMapping>> GetProtocolFormMappingsForCategory(int categoryId);
        Task<bool> SaveProtocolFormMapping(ProtocolFormMapping protocolFormMapping);
        Task<bool> DeleteProtocolFormMapping(ProtocolFormMapping protocolFormMapping);
    }
}
