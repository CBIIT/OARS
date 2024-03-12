using TheradexPortal.Data.Models;

namespace TheradexPortal.Data.Services.Abstract
{
    public interface IProtocolEDCFieldService
    {
        public Task<bool> BulkSaveFields(List<ProtocolEDCField> fields);
    }
}
