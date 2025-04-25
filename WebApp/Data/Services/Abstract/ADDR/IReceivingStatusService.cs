using OARS.Data.Models;
using OARS.Data.Models.ADDR;

namespace OARS.Data.Services.Abstract.ADDR
{
    public interface IReceivingStatusService
    {
        Task<List<ReceivingStatus>?> GetReceivingStatus(string protocalNumber);
    }
}
