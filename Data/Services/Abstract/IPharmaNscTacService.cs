using TheradexPortal.Data.Models;

namespace TheradexPortal.Data.Services.Abstract
{
    public interface IPharmaNscTacService
    {
        Task<IEnumerable<PharmaNscTac>> GetAllAsync();
        Task<PharmaNscTac> GetByIdAsync(string protocolNumber);
        Task AddAsync(PharmaNscTac entity, int userId);
        Task UpdateAsync(PharmaNscTac entity, int userId);
        Tuple<bool, string> DeleteAsync(string protocolNumber, int userId);
        Tuple<bool, string> DeleteAsync(int id, int userId);

        Task<IEnumerable<PharmaNscTac>> GetByAgreementNumberAsync(string agreementNumber);
        Task<IEnumerable<PharmaNscTac>> GetByNscAsync(string nsc);
        Task<IEnumerable<PharmaNscTac>> GetByTrtAsgnmtCodeAsync(string trtAsgnmtCode);
    }
}
