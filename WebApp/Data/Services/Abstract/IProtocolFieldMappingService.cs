using OARS.Data.Models;

namespace OARS.Data.Services.Abstract
{
    public interface IProtocolFieldMappingService
    {
        public Task<IList<ProtocolFieldMapping>> GetProtocolFieldMappings();
        public Task<IList<ProtocolFieldMapping>> GetProtocolFieldMappings(string fieldId);
        public Task<bool> HasProtocolFieldMappingsForProtocolMapping(int protocolMappingId);
        public Task<IList<ProtocolFieldMapping>> GetProtocolFieldMappingsForCategory(int protocolMappingId, string categoryId);
        public Task<ProtocolFieldMapping> GetProtocolFieldMapping(int protocolFieldMappingId);
        public Task<bool> SaveProtocolFieldMapping(ProtocolFieldMapping protocolFieldMapping);
        public Task<bool> DeleteProtocolFieldMapping(ProtocolFieldMapping protocolFieldMapping);
    }
}
