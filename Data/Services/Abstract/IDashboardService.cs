using TheradexPortal.Data.Models;

namespace TheradexPortal.Data.Services.Abstract
{
    public interface IDashboardService
    {
        public Task<IList<Dashboard>> GetAllDashboardsAsync();
        public Task<Dashboard?> GetDashboardByIdAsync(int id);
        public Task<IList<Report>> GetAllReportsByDashboardIdAsync(int id);
        public Task<IList<Visual>> GetAllVisualsByReportIdAsync(int id);
    }
}