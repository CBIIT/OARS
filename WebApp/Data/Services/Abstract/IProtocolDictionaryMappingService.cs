using OARS.Data.Models;

namespace OARS.Data.Services.Abstract
{
    public interface IProtocolDictionaryMappingService
    {
        public Task<List<ProtocolDictionaryMapping>> GetProtocolDictionaryMappings();
        public Task<List<ProtocolDictionaryMapping>> GetProtocolDictionaryMappings(int protocolMappingId); // , int fieldId
        public Task<ProtocolDictionaryMapping> GetProtocolDictionaryMapping(int id);
        public Task<bool> SaveProtocolDictionaryMapping(ProtocolDictionaryMapping protocolDictionaryMapping);
    }
}
