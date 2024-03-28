using TheradexPortal.Data.Models;

namespace TheradexPortal.Data.Services.Abstract
{
    public interface IProtocolFieldService
    {
        public Task<IList<ProtocolField>> GetProtocolFieldsByMappingId(int mappingId);
        public Task<bool> SaveProtocolField(int protocolMappingId, ProtocolField protocolField);
        public Task<bool> DeleteField(int fieldId);
        public Task<bool> DeleteAllFieldsForMappingId(int mappingId);
        public Task<IList<ProtocolField>> GetAllProtocolFieldsByMappingId(int mappingId);
    }
}
