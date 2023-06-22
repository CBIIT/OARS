using TheradexPortal.Data.Models;

namespace TheradexPortal.Data.Services.Abstract
{
    public interface IStudyService
    {
        public Task<IList<Protocol>> GetAllProtocolsAsync();
    }
}