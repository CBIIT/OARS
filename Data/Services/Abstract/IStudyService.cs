using TheradexPortal.Data.Models;

namespace TheradexPortal.Data.Services.Abstract
{
    public interface IStudyService
    {
        public Task<IList<Protocol>> GetAllProtocolsAsync();
        public IList<Protocol> GetProtocolsForUserAsync(int userId, bool isAdmin);
        public string GetCurrentStudiesForUser(int userId);
        public string GetSelectedStudyIdsForUser(int userId);
        public List<Protocol> GetSelectedStudiesForUser(int userId);
    }
}