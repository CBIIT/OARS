using TheradexPortal.Data.Models.Pharma;

namespace TheradexPortal.Data.Services.Abstract.Pharma
{
    public interface IPharmaNscTacService
    {
        Task<IEnumerable<PharmaNscTac>> GetAllAsync();
        Task<PharmaNscTac> GetByProtocolNumberAsync(string protocolNumber);
        Task<PharmaNscTac> GetByIdAsync(int id);
        Task<bool> AddAsync(PharmaNscTac entity, int userId);
        Task UpdateAsync(PharmaNscTac entity, int userId);
        Task<Tuple<bool, string>> DeleteAsync(int id, int userId, bool isHardDelete=false);
        Task<IEnumerable<PharmaNscTac>> GetByAgreementNumberAsync(string agreementNumber);
        Task<IEnumerable<PharmaNscTac>> GetByNscAsync(string nsc);
        Task<IEnumerable<PharmaNscTac>> GetByTrtAsgnmtCodeAsync(string trtAsgnmtCode);
        Task<bool> IsUniqueCombinationAsync(string agreementNumber, string nsc, string protocolNumber, string trtAsgnmtCode);
        Task<bool> CanDelete(int id);

    }
}
