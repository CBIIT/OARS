using TheradexPortal.Data.Models;

namespace TheradexPortal.Data.Services.Abstract
{
    public interface IUserRoleService
    {
        public Task<List<Role>> GetUserRolesAsync(int userId);
    }
}