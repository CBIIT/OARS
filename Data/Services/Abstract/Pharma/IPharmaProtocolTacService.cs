using TheradexPortal.Data.Models.Pharma;

namespace TheradexPortal.Data.Services.Abstract.Pharma
{
    public interface IPharmaProtocolTacService
    {
        public Task<IEnumerable<ProtocolTac>> GetAllAsync();
    }
}