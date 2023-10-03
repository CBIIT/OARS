using TheradexPortal.Data.Models;

namespace TheradexPortal.Data.Services.Abstract
{
    public interface IUserRoleService
    {
        public Task<IList<Role>> GetAllRolesAsync();
        public Task<Role> GetRoleByIdAsync(int id);
        public Task<IList<Role>> GetRoleByLevel(bool isPrimary);
        public Task<List<Role>> GetUserRolesAsync(int userId);
        public Task<List<RoleDashboard>> GetRoleDashboards(int roleId);
        public Task<List<RoleReport>> GetRoleReports(int roleId);
        public bool CheckRoleName(string roleName, int roleId);
        public bool SaveRole(Role role);
        public bool CanDeleteRole(int roleId);
        public Tuple<bool, string> DeleteRole(int roleId);
    }
}