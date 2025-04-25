using OARS.Data.Models;
using OARS.Pages.Admin.dmu;

namespace OARS.Data.Services.Abstract
{
    public interface IProtocolTACService
    {
        public Task<IList<ProtocolTac>> GetProtocolTACByProtocolMappingId(int protocolMappingId);
        public Task<bool> SaveProtocolTAC(ProtocolTac protocolTac);
        public Task<bool> DeleteProtocolTAC(int protocolTacId);
    }
}
