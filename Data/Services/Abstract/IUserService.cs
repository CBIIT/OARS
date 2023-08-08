using TheradexPortal.Data.Models;

namespace TheradexPortal.Data.Services.Abstract
{
    public interface IUserService
    {
        public Task<IList<User>> GetAllUsersAsync();
        public Task<User?> GetUserAsync(int userId);
        public Task<User?> GetUserByEmailAsync(string emailAddress);

        public bool SaveSelectedStudies(int userId, string studies);
    }
}