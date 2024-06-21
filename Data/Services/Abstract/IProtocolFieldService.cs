using TheradexPortal.Data.Models;

namespace TheradexPortal.Data.Services.Abstract
{
    public interface IProtocolFieldService
    {
        public Task<IList<ProtocolField>> GetProtocolDateFieldsByMappingId(int protocolMappingId);
        public Task<bool> SaveProtocolField(int protocolMappingId, ProtocolField protocolField);
        public Task<bool> DeleteField(int fieldId);
        public Task<bool> DeleteAllFieldsForMappingId(int protocolMappingId);
        public Task<IList<ProtocolField>> GetAllProtocolFieldsByMappingId(int protocolMappingId);
        public Task<IList<ProtocolField>> GetAllProtocolFieldsByMappingIdForMapping(int protocolMappingId, string dataCategoryId);
        public Task<IList<ProtocolField>> GetAllProtocolDisabledFieldsByMappingIdForMapping(int protocolMappingId, string dataCategoryId);
        public Task<bool> CreateProtocolFieldsFromProfile(int? profileId, int protocolMappingId);
    }
}
