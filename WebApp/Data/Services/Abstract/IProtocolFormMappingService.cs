using OARS.Data.Models;

namespace OARS.Data.Services.Abstract
{
    public interface IProtocolFormMappingService
    {
        Task<IList<ProtocolFormMapping>> GetProtocolFormMappings();
        Task<ProtocolFormMapping> GetProtocolFormMapping(int id);
        Task<IList<ProtocolFormMapping>> GetProtocolFormMappings(int formId);
        Task<IList<ProtocolFormMapping>> GetProtocolFormMappingsForCategory(int protocolMappingId, int categoryId);
        Task<bool> SaveProtocolFormMapping(ProtocolFormMapping protocolFormMapping);
        Task<bool> DeleteProtocolFormMapping(ProtocolFormMapping protocolFormMapping);
    }
}
