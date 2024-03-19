using TheradexPortal.Data.Models;

namespace TheradexPortal.Data.Services.Abstract
{
    public interface IProtocolEDCFormService
    {
        public Task<bool> BulkSaveForms(List<ProtocolEDCForm> forms);
        public Task<List<int>> GetFormIdsForMappingId(int mappingId);
        public Task<bool> DeleteAllFormsForMappingId(int mappingId);
        public Task<List<ProtocolEDCForm>> GetFormsForMappingId(int mappingId);
    }
}
