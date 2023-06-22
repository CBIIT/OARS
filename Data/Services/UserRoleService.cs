using Microsoft.EntityFrameworkCore;
using TheradexPortal.Data.Models;
using TheradexPortal.Data.Services.Abstract;

namespace TheradexPortal.Data.Services
{
    public class UserRoleService : BaseService, IUserRoleService
    {
        public UserRoleService(IDbContextFactory<WrDbContext> dbFactory) : base(dbFactory)
        {
        }

        public async Task<List<Role>> GetUserRolesAsync(int userId)
        {
            return await context.User_Roles.Include(ur => ur.Role).Where(ur => ur.UserId == userId).Select(ur => ur.Role).ToListAsync();
        }

    }
}
