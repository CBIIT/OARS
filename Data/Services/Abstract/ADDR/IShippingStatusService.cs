using TheradexPortal.Data.Models;
using TheradexPortal.Data.Models.ADDR;

namespace TheradexPortal.Data.Services.Abstract.ADDR
{
    public interface IShippingStatusService
    {
        Task<List<ShippingStatus>?> GetShippingStatus(string protocalNumber);
        
    }
}
