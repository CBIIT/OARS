using TheradexPortal.Data.Models;
using TheradexPortal.Pages.Admin.dmu;

namespace TheradexPortal.Data.Services.Abstract
{
    public interface IProtocolTACService
    {
        public Task<IList<ProtocolTac>> GetProtocolTACByProtocolMappingId(int protocolMappingId);
        public Task<bool> SaveProtocolTAC(ProtocolTac protocolTac);
        public Task<bool> DeleteProtocolTAC(int protocolTacId);
    }
}
