using OARS.Data.Models;
using OARS.Pages.Admin.dmu;

namespace OARS.Data.Services.Abstract
{
    public interface IProtocolDiseaseService
    {
        public Task<IList<ProtocolDisease>> GetProtocolDiseaseByProtocolMappingId(int protocolMappingId);
        public Task<bool> SaveProtocolDisease(ProtocolDisease protocolDisease);
        public Task<bool> DeleteProtocolDisease(int protocolDiseaseId);
    }
}
