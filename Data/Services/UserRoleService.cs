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

        public async Task<IList<Role>> GetAllRolesAsync()
        {
            return await context.Roles.ToListAsync();
        }

        public async Task<Role> GetRoleByIdAsync(int id)
        {
            return await context.Roles.Where(r=>r.RoleId==id).Include(rd=>rd.RoleDashboards).Include(rr=>rr.RoleReports).FirstOrDefaultAsync();
            //return await context.Roles.Where(r => r.RoleId == id).FirstOrDefaultAsync();
        }

        public async Task<IList<Role>> GetRoleByLevel(bool isPrimary)
        {
            return await context.Roles.Where(r=>r.IsPrimary==isPrimary).ToListAsync();
        }

        public async Task<List<Role>> GetUserRolesAsync(int userId)
        {
            return await context.User_Roles.Include(ur => ur.Role).Where(ur => ur.UserId == userId).Select(ur => ur.Role).ToListAsync();
        }

        public async Task<List<RoleDashboard>> GetRoleDashboards(int roleId)
        {
            return await context.Role_Dashboards.Where(ur => ur.RoleId == roleId).ToListAsync();
        }

        public async Task<List<RoleReport>> GetRoleReports(int roleId)
        {
            return await context.Role_Reports.Where(ur => ur.RoleId == roleId).ToListAsync();
        }

        public bool CheckRoleName(string roleName, int roleId)
        {
            Role foundRole = context.Roles.FirstOrDefault(r => r.RoleName == roleName && r.RoleId != roleId);
            return foundRole == null;
        }

        public bool SaveRole(Role role)
        {
            DateTime curDateTime = DateTime.UtcNow;
            try
            {
                if (role.RoleId == 0)
                {
                    context.Roles.Add(role);
                    context.SaveChanges();
                }
                else
                {
                    Role dbRole = context.Roles.FirstOrDefault(r => r.RoleId == role.RoleId);
                    if (dbRole != null)
                    {
                        dbRole.RoleName = role.RoleName;
                        dbRole.AdminType = role.AdminType;
                        dbRole.IsPrimary = role.IsPrimary;
                        dbRole.UpdateDate = curDateTime;

                        // iterate through dashboards & reports - add as needed-  no updates needed
                        foreach (RoleDashboard rd in role.RoleDashboards)
                        {
                            RoleDashboard foundRD = dbRole.RoleDashboards.FirstOrDefault(db => db.DashboardId == rd.DashboardId);
                            if (foundRD == null)
                            {
                                RoleDashboard newRD = new RoleDashboard();
                                newRD.DashboardId = rd.DashboardId;
                                newRD.CreateDate = curDateTime;
                                dbRole.RoleDashboards.Add(newRD);
                            }
                        }

                        // iterate through reports - add as needed
                        foreach (RoleReport rr in role.RoleReports)
                        {
                            RoleReport foundRR = dbRole.RoleReports.FirstOrDefault(db => db.ReportId == rr.ReportId);
                            if (foundRR == null)
                            {
                                RoleReport newRR = new RoleReport();
                                newRR.ReportId = rr.ReportId;
                                newRR.CreateDate = curDateTime;
                                dbRole.RoleReports.Add(newRR);
                            }
                        }

                        // Remove dashboards no longer selected
                        List<RoleDashboard> rdToRemove = new List<RoleDashboard>();
                        foreach (RoleDashboard rd in dbRole.RoleDashboards)
                        {
                            RoleDashboard foundRD = role.RoleDashboards.FirstOrDefault(od => od.DashboardId == rd.DashboardId);
                            if (foundRD == null)
                                rdToRemove.Add(rd);
                        }
                        // Remove reports no longer selected
                        List<RoleReport> rrToRemove = new List<RoleReport>();
                        foreach (RoleReport rr in dbRole.RoleReports)
                        {
                            RoleReport foundRR = role.RoleReports.FirstOrDefault(or => or.ReportId == rr.ReportId);
                            if (foundRR == null)
                                rrToRemove.Add(rr);
                        }

                        // Remove dash and reps from collection then save.
                        foreach (RoleDashboard delD in rdToRemove)
                            dbRole.RoleDashboards.Remove(delD);

                        foreach (RoleReport delR in rrToRemove)
                            dbRole.RoleReports.Remove(delR);

                        context.SaveChanges();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool CanDeleteRole(int roleId)
        {
            return context.User_Roles.Where(ur => ur.RoleId == roleId).Count() == 0;
        }

        public Tuple<bool, string> DeleteRole(int roleId)
        {
            try
            {
                var role = context.Roles.Where(r => r.RoleId == roleId).First();
                context.Remove(role);
                context.SaveChanges();
                return new Tuple<bool, string>(true, "Role deleted successfully");
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string>(false, "Failed to delete role");
            }
        }
    }
}
