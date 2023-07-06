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

        public async Task<IList<Dashboard>> GetDashboardsForUserAsync()
        {
            var httpContext = new HttpContextAccessor().HttpContext;

            if (httpContext is not null && httpContext.User is not null)
            {

                bool isAdmin = httpContext.User.HasClaim(WRClaimType.Role, "Administrator");

                if (isAdmin)
                {
                    return await context.Dashboards.OrderBy(d => d.DisplayOrder).ToListAsync();
                }
                else
                {
                    // Get user id from context
                    int userId = Convert.ToInt32(httpContext.User.FindFirst(WRClaimType.UserId).Value);

                    var dashboards = (from ur in context.User_Roles
                                     join rd in context.Role_Dashboards on ur.RoleId equals rd.RoleId
                                     join d in context.Dashboards on rd.DashboardId equals d.WRDashboardId
                                     where ur.UserId == userId
                                     select d).OrderBy(d => d.DisplayOrder).ToList();

                    return dashboards;
                }
            }
            else
                return null;
        }

            public async Task<Dashboard?> GetDashboardByIdAsync(int id)
        {
            return await context.Dashboards.FindAsync(id);
        }

        public async Task<IList<Report>> GetAllReportsByDashboardIdAsync(int id)
        {
            var list =  await context.Reports.Where(r => r.DashboardId == id).OrderBy(r => r.DisplayOrder).ToListAsync();
            return list;
        }

        public async Task<IList<Visual>> GetAllVisualsByReportIdAsync(int id)
        {
            var list = await context.Visuals.Where(v => v.ReportId == id).OrderBy(v => v.DisplayOrder).ToListAsync();
            return list;
        }
    }
}
