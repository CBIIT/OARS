using TheradexPortal.Data.Models;

namespace TheradexPortal.Data.Services.Abstract
{
    public interface IUserService
    {
        public Task<IList<User>> GetAllUsersAsync();
        public Task<User?> GetUserAsync(int userId);
        public Task<User?> GetUserByEmailAsync(string emailAddress);
        public bool CheckEmailAddress(string emailAddress, int userId);
        public bool SaveUser(User user, int loggedInUserId);
        public bool SaveLastLoginDate(int userId);
        public bool SaveSelectedStudies(int userId, string selectedStudies, bool saveRecent);
        public bool SaveCurrentStudy(int userId, string currentStudies);
        public Task<bool> SetStartingStudies(int userId, int count);
        public bool DeactivateUser(int userId, int loggedInUserId);
        public Task<IList<string>> GetProtocolHistoryAsync(int userId, int count);
        public void SaveTimeZoneInfo(int userId, string timeZoneAbbrev, TimeSpan currentOffset);
        public bool SaveActivityLog(int userId, string activityType);
        public bool SaveActivityLog(int userId, string activityType, string data1);
        public bool SaveActivityLog(int userId, string activityType, string data1, string data2);
        public Tuple<bool, String> SaveFavorite(int userId, int dashboardId, int reportId, string reportName);
        public List<FavoriteReportItem> GetUserFavoriteList(int userId, bool isAdmin);
        public UserFavorite GetUserFavoriteFirstDashboardReport(int userId, bool isAdmin);
        public bool IsReportFavorite(int userId, int reportId);
        public bool RemoveFavorite(int userId,int favoriteId);
        public string GetUserFavoriteNamesByID(int userFavoriteId);
        public bool HasUserFavorite(int userId, bool isAdmin);
        List<string> GetProtocolAccessForUser(int userId);
    }
}