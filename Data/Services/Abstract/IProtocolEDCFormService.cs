using TheradexPortal.Data.Models;

namespace TheradexPortal.Data.Services.Abstract
{
    public interface IProtocolEDCFormService
    {
        public Task<bool> BulkSaveForms(List<ProtocolEDCForm> forms);
    }
}
