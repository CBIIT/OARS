using TheradexPortal.Data.Models.Pharma;

namespace TheradexPortal.Data.Services.Abstract.Pharma
{
    public interface IPharmaNscTacService
    {
        Task<IEnumerable<PharmaNscTac>> GetAllAsync();
        Task<PharmaNscTac> GetByProtocolNumberAsync(string protocolNumber);
        Task<PharmaNscTac> GetByIdAsync(int id);
        Task AddAsync(PharmaNscTac entity, int userId);
        Task UpdateAsync(PharmaNscTac entity, int userId);
        Tuple<bool, string> DeleteAsync(string protocolNumber, int userId);
        Tuple<bool, string> DeleteAsync(int id, int userId);

        Task<IEnumerable<PharmaNscTac>> GetByAgreementNumberAsync(string agreementNumber);
        Task<IEnumerable<PharmaNscTac>> GetByNscAsync(string nsc);
        Task<IEnumerable<PharmaNscTac>> GetByTrtAsgnmtCodeAsync(string trtAsgnmtCode);
    }
}
