using TheradexPortal.Data.Models.Pharma;

namespace TheradexPortal.Data.Services.Abstract.Pharma
{
    public interface IPharmaDrugListService
    {
        public Task<IEnumerable<DrugList>> GetAllAsync();
    }
}