using Microsoft.EntityFrameworkCore;

namespace TheradexPortal.Data.Services
{
    public class UserService
    {
        private WrDbContext context;
        public UserService(IDbContextFactory<WrDbContext> dbFactory)
        {
            context = dbFactory.CreateDbContext();
        }

        public async Task<int> GetUserCountAsync()
        {
            return await context.Users.CountAsync();
        }
    }
}
