using Microsoft.EntityFrameworkCore;
using TheradexPortal.Data.Models;
using TheradexPortal.Data.Services.Abstract;

namespace TheradexPortal.Data.Services
{
    public class DashboardService : BaseService, IDashboardService
    {
        public DashboardService(IDbContextFactory<WrDbContext> dbFactory) : base(dbFactory) { }

        public async Task<IList<Dashboard>> GetAllDashboardsAsync()
        {
            return await context.Dashboards.OrderBy(d => d.DisplayOrder).ToListAsync();
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
