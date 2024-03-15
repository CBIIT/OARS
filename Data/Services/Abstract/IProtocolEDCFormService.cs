using TheradexPortal.Data.Models;

namespace TheradexPortal.Data.Services.Abstract
{
    public interface IProtocolEDCFormService
    {
        public Task<IList<ProtocolEDCForm>> GetProtocolEDCFormsByProtocolMappingId(int protocolMappingId);
        public Task<bool> SaveProtocolEDCForm(ProtocolEDCForm protocolEDCForm);
    }
}
