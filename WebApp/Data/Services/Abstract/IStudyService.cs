using OARS.Data.Models;

namespace OARS.Data.Services.Abstract
{
    public interface IStudyService
    {
        public Task<IList<Protocol>> GetAllProtocolsAsync();
        public IList<Protocol> GetProtocolsForUserAsync(int userId, bool allStudies);
        public string GetCurrentStudiesForUser(int userId);
        public string GetSelectedStudyIdsForUser(int userId);
        public List<Protocol> GetSelectedStudiesForUser(int userId);
        public Task<String> GetStudyTitleAsync(string protocolID);
    }
}