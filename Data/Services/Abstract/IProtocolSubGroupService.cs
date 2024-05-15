using TheradexPortal.Data.Models;

namespace TheradexPortal.Data.Services.Abstract
{
    public interface IProtocolSubGroupService
    {
        public Task<List<ProtocolSubGroup>> GetSubGroupsByMappingId(int mappingId);
        public Task<bool> SaveSubGroup(ProtocolSubGroup subGroup, int mappingId);
        public Task<bool> DeleteSubGroup(ProtocolSubGroup subGroup);
    }
}
