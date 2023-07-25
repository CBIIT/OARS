using TheradexPortal.Data.Models;

namespace TheradexPortal.Data.Services.Abstract
{
    public interface IDashboardService
    {
        public Task<IList<Dashboard>> GetAllDashboardsAsync();
        public Task<String> GetDashboardIdsForUser(int userId, bool isAdmin);
        public Task<IList<Dashboard>> GetDashboardsForUserAsync(int userId, bool isAdmin);
        public Task<Dashboard?> GetDashboardByIdAsync(int id, string userDashboards);
        public Task<IList<Report>> GetReportsByDashboardIdAsync(int id);
        public Task<String> GetReportIdsForUser(int userId, bool isAdmin);
        public Task<IList<Report>> GetReportsByDashboardIdForUserAsync(int id, int userId, bool isAdmin);
        public Task<Report?> GetReportByIdAsync(int id, string userReports);
        public Task<IList<Visual>> GetAllVisualsByReportIdAsync(int id);
    }
}