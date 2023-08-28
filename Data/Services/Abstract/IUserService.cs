using TheradexPortal.Data.Models;

namespace TheradexPortal.Data.Services.Abstract
{
    public interface IUserService
    {
        public Task<IList<User>> GetAllUsersAsync();
        public Task<User?> GetUserAsync(int userId);
        public Task<User?> GetUserByEmailAsync(string emailAddress);
        public bool SaveUser(User user);
        public bool SaveLastLoginDate(int userId);
        public bool SaveSelectedStudies(int userId, string studies, bool saveRecent);
        public bool SaveCurrentStudy(int userId, string study);
        public bool DeactivateUser(int userId);
        public Task<IList<string>> GetProtocolHistoryAsync(int userId, int count);
        public bool SaveActivityLog(int userId, string activityType);
        public bool SaveActivityLog(int userId, string activityType, string data1);
        public bool SaveActivityLog(int userId, string activityType, string data1, string data2);
    }
}