using Microsoft.EntityFrameworkCore;
using TheradexPortal.Data.Models;
using TheradexPortal.Data.Services.Abstract;
using TheradexPortal.Data.Static;

namespace TheradexPortal.Data.Services
{
    public class DashboardService : BaseService, IDashboardService
    {
        public DashboardService(IDbContextFactory<WrDbContext> dbFactory) : base(dbFactory) { }

        public async Task<IList<Dashboard>> GetAllDashboardsAsync()
        {
            return await context.Dashboards.OrderBy(d => d.DisplayOrder).ToListAsync();
        }

        public async Task<IList<Dashboard>> GetAllDashboardsForAdminAsync()
        {
            return await context.Dashboards.OrderBy(d => d.DisplayOrder).Include(r => r.Reports.OrderBy(r2 => r2.DisplayOrder)).ToListAsync();
        }

        public async Task<String> GetDashboardIdsForUser(int userId, bool isAdmin)
        {
            string dashboardList = "";
            List<int> dashboards;

            if (isAdmin)
            {
                dashboards = await context.Dashboards.OrderBy(d=>d.DisplayOrder).Select(d=>d.WRDashboardId).ToListAsync();
            }
            else
            {
                dashboards = (from ur in context.User_Roles
                              join rd in context.Role_Dashboards on ur.RoleId equals rd.RoleId
                              join d in context.Dashboards on rd.DashboardId equals d.WRDashboardId
                              where ur.UserId == userId && (ur.ExpirationDate == null || ur.ExpirationDate.Value.Date >= DateTime.UtcNow.Date)
                              orderby d.DisplayOrder
                              select d.WRDashboardId).ToList();
            }

            foreach (int dashboard in dashboards)
                dashboardList += "|" + dashboard.ToString();
            dashboardList += "|";

            return dashboardList;
        }

        public async Task<IList<Dashboard>> GetDashboardsForUserAsync(int userId, bool isAdmin)
        {
            if (isAdmin)
            {
                return await context.Dashboards.OrderBy(d => d.DisplayOrder).ToListAsync();
            }
            else
            {
                var dashboards = (from ur in context.User_Roles
                                    join rd in context.Role_Dashboards on ur.RoleId equals rd.RoleId
                                    join d in context.Dashboards on rd.DashboardId equals d.WRDashboardId
                                    where ur.UserId == userId && (ur.ExpirationDate == null || ur.ExpirationDate.Value.Date >= DateTime.UtcNow.Date)
                                  select d).OrderBy(d => d.DisplayOrder).ToList();

                return dashboards;
            }
        }

        public async Task<Dashboard?> GetDashboardByIdAsync(int id, string userDashboards)
        {
            if (!userDashboards.Contains("|" + id.ToString() + "|"))
                return null;
            else
                return await context.Dashboards.FindAsync(id);
        }

        public async Task<IList<Report>> GetReportsByDashboardIdAsync(int id)
        {
            var list =  await context.Reports.Where(r => r.DashboardId == id).OrderBy(r => r.DisplayOrder).ToListAsync();
            return list;
        }
        
        public async Task<String> GetReportIdsForUser(int userId, bool isAdmin)
        {
            string reportList = "";
            List<int> reports;

            if (isAdmin)
            {
                reports = await context.Reports.Select(d => d.WRReportId).ToListAsync();
            }
            else
            {
                reports = (from ur in context.User_Roles
                           join rr in context.Role_Reports on ur.RoleId equals rr.RoleId
                           join r in context.Reports on rr.ReportId equals r.WRReportId
                           where ur.UserId == userId && (ur.ExpirationDate == null || ur.ExpirationDate.Value.Date >= DateTime.UtcNow.Date)
                           select r.WRReportId).ToList();
            }

            foreach (int report in reports)
                reportList += "|" + report.ToString();
            reportList += "|";

            return reportList;
        }
        
        public async Task<IList<Report>> GetReportsByDashboardIdForUserAsync(int id, int userId, bool isAdmin)
        {
            if (isAdmin)
            {
                return await context.Reports.Where(r => r.DashboardId == id).OrderBy(r => r.DisplayOrder).ToListAsync();
            }
            else
            {
                var reports = (from ur in context.User_Roles
                                  join rr in context.Role_Reports on ur.RoleId equals rr.RoleId
                                  join r in context.Reports on rr.ReportId equals r.WRReportId
                                  where ur.UserId == userId && (ur.ExpirationDate == null || ur.ExpirationDate.Value.Date >= DateTime.UtcNow.Date) && r.DashboardId == id
                                  select r).OrderBy(r => r.DisplayOrder).ToList();

                return reports;
            }
        }
        
        public async Task<Report?> GetReportByIdAsync(int id, string userReports)
        {
            if (!userReports.Contains("|" + id.ToString() + "|"))
                return null;
            else
                return await context.Reports.FindAsync(id);
        }
        
        public async Task<IList<Visual>> GetAllVisualsByReportIdAsync(int id)
        {
            var list = await context.Visuals.Where(v => v.ReportId == id).OrderBy(v => v.DisplayOrder).ToListAsync();
            return list;
        }

        public bool HasDashboardAccess(int dashboardId)
        {
            if (dashboardId == 0)
                return false;
            else
                return false;
        }
    }
}
