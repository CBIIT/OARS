using Microsoft.EntityFrameworkCore;

namespace TheradexPortal.Data.Services
{
    public class UserService : BaseService
    {
        public UserService(IDbContextFactory<WrDbContext> dbFactory) : base(dbFactory) { }

        public async Task<int> GetUserCountAsync()
        {
            return await context.Users.CountAsync();
        }
    }
}
