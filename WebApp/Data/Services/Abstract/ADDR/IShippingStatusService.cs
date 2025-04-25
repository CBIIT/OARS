using OARS.Data.Models;
using OARS.Data.Models.ADDR;

namespace OARS.Data.Services.Abstract.ADDR
{
    public interface IShippingStatusService
    {
        Task<List<ShippingStatus>?> GetShippingStatus(string protocalNumber);
        
    }
}
