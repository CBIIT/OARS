using TheradexPortal.Data.Models;
using TheradexPortal.Data.Models.ADDR;

namespace TheradexPortal.Data.Services.Abstract.ADDR
{
    public interface IReceivingStatusService
    {
        Task<List<ReceivingStatus>?> GetReceivingStatus(string protocalNumber);
    }
}
