using Microsoft.EntityFrameworkCore;
using TheradexPortal.Data.Models;

namespace TheradexPortal.Data.Services
{
    public class DashboardService : BaseService
    {
        public DashboardService(IDbContextFactory<WrDbContext> dbFactory) : base(dbFactory) { }

        public async Task<IList<Dashboard>> GetAllDashboardsAsync()
        {
            return await context.Dashboards.OrderBy(d => d.Display_Order).ToListAsync();
        }

        public async Task<Dashboard?> GetDashboardByIdAsync(int id)
        {
            return await context.Dashboards.FindAsync(id);
        }

        public async Task<IList<Report>> GetAllReportsByDashboardIdAsync(int id)
        {
            var list =  await context.Reports.Where(r => r.DashboardId == id).OrderBy(r => r.Display_Order).ToListAsync();
            return list;
        }

        public async Task<IList<Visual>> GetAllVisualsByReportIdAsync(int id)
        {
            var list = await context.Visuals.Where(v => v.Report_Id == id).OrderBy(v => v.Display_Order).ToListAsync();
            return list;
        }
    }
}
