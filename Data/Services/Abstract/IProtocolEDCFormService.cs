using TheradexPortal.Data.Models;

namespace TheradexPortal.Data.Services.Abstract
{
    public interface IProtocolEDCFormService
    {
        public Task<IList<ProtocolEDCForm>> GetProtocolEDCFormsByProtocolMappingId(int protocolMappingId);
        public Task<bool> SaveProtocolEDCForm(ProtocolEDCForm protocolEDCForm);
        public Task<bool> BulkSaveForms(List<ProtocolEDCForm> forms);
        public Task<List<int>> GetFormIdsForMappingId(int mappingId);
        public Task<bool> DeleteAllFormsForMappingId(int mappingId);
        public Task<bool> DeleteProtocolEDCFormId(int protocolEDCFormId);
        public Task<List<ProtocolEDCForm>> GetFormsForMappingId(int mappingId);
    }
}
