using TheradexPortal.Data.Models.Pharma;

namespace TheradexPortal.Data.Services.Abstract.Pharma
{
    public interface IPharmaCdrdmStudyAgentService
    {
        public Task<IEnumerable<string>> GetDistinctNscCodesAsync();
        public Task<IEnumerable<string>> GetDocumentNumbersByNscAsync(string nscCode);

        public Task<CdrdmStudyAgent> GetStudyAgentByNscAndDocumentNumberAsync(string nscCode, string documentNumber);
    }
}