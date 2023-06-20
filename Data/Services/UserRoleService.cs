using Microsoft.EntityFrameworkCore;

namespace TheradexPortal.Data.Services
{
    public class UserRoleService : BaseService
    {
        public UserRoleService(IDbContextFactory<WrDbContext> dbFactory) : base(dbFactory)
        {
        }

        //public async Task<UserRole> GetUserRoleCountAsync()
        //{
        //    return await context.UserRoles.CountAsync();
        //}
    }
}
