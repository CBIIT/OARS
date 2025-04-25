using OARS.Data.Models.Pharma;

namespace OARS.Data.Services.Abstract.Pharma
{
    public interface IPharmaProtocolTacService
    {
        public Task<IEnumerable<ProtocolTac>> GetAllAsync();
    }
}