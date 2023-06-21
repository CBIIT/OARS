using TheradexPortal.Data.Models;

namespace TheradexPortal.Data.Services.Abstract
{
    public interface IUserService
    {
        public Task<User?> GetUserAsync(int userId);
        public Task<User?> GetUserByEmailAsync(string emailAddress);
        public Task<int> GetUserCountAsync();

    }
}