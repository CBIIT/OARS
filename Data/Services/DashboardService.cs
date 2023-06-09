using Microsoft.EntityFrameworkCore;
using TheradexPortal.Data.Models;

namespace TheradexPortal.Data.Services
{
    public class DashboardService : BaseService
    {
        public DashboardService(IDbContextFactory<WrDbContext> dbFactory) : base(dbFactory) { }

        public async Task<IList<Dashboard>> GetAllDashboardsAsync()
        {
            return await context.Dashboards.ToListAsync();
        }

        public async Task<Dashboard?> GetDashboardByIdAsync(int id)
        {
            return await context.Dashboards.FindAsync(id);
        }

        public async Task<IList<Report>> GetReportsByDashboardId(int id)
        {
            return await context.Reports.Where(r => r.DashboardId == id).ToListAsync();
        }
    }
}
