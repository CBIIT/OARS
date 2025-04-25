using OARS.Data.Models.Pharma;

namespace OARS.Data.Services.Abstract.Pharma
{
    public interface IPharmaCdrdmStudyAgentService
    {
        public Task<IEnumerable<string>> GetDistinctNscCodesAsync();
        public Task<IEnumerable<string>> GetDocumentNumbersByNscAsync(string nscCode);

        public Task<CdrdmStudyAgent> GetStudyAgentByNscAndDocumentNumberAsync(string nscCode, string documentNumber);
    }
}