using TheradexPortal.Data.Models;

namespace TheradexPortal.Data.Services.Abstract
{
    public interface IProtocolDataSystemService
    {
        public Task<IList<ProtocolDataSystem>> GetProtocolDataSystems();
        public Task<bool> SaveProtocolDataSystem(ProtocolDataSystem protocolDataSystem);
    }
}
