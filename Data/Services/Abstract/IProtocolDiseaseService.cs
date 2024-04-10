using TheradexPortal.Data.Models;
using TheradexPortal.Pages.Admin.dmu;

namespace TheradexPortal.Data.Services.Abstract
{
    public interface IProtocolDiseaseService
    {
        public Task<IList<ProtocolDisease>> GetProtocolDiseaseByProtocolMappingId(int protocolMappingId);
        public Task<bool> SaveProtocolDisease(ProtocolDisease protocolDisease);
        public Task<bool> DeleteProtocolDisease(int protocolDiseaseId);
    }
}
