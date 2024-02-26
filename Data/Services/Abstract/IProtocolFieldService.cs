using TheradexPortal.Data.Models;

namespace TheradexPortal.Data.Services.Abstract
{
    public interface IProtocolFieldService
    {
        public Task<IList<ProtocolField>> GetProtocolFieldsByMappingId(int mappingId);
        public Task<bool> SaveProtocolField(ProtocolField protocolField);
    }
}
