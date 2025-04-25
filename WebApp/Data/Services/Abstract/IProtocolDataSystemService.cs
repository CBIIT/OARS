using OARS.Data.Models;

namespace OARS.Data.Services.Abstract
{
    public interface IProtocolDataSystemService
    {
        public Task<IList<ProtocolDataSystem>> GetProtocolDataSystems();
        public Task<bool> SaveProtocolDataSystem(ProtocolDataSystem protocolDataSystem);

        public Task<bool> ProtocolDataSystemExists(ProtocolDataSystem protocolDataSystem);
    }
}
