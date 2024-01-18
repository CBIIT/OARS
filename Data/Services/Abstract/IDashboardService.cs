using Amazon.S3.Model;
using TheradexPortal.Data.Models;

namespace TheradexPortal.Data.Services.Abstract
{
    public interface IDashboardService
    {
        public Task<IList<Dashboard>> GetAllDashboardsAsync();
        public Task<IList<Dashboard>> GetAllDashboardsForAdminAsync();
        public Task<String> GetDashboardIdsForUser(int userId, bool isAdmin);
        public Task<IList<Dashboard>> GetDashboardsForUserAsync(int userId, bool isAdmin);
        public Task<Dashboard?> GetDashboardByIdAsync(int id, string userDashboards, bool isAdmin);
        public Task<IList<Report>> GetReportsByDashboardIdAndName(int id, string reportName, int userId, bool isAdmin);
        public Task<IList<Report>> GetReportsByDashboardIdAsync(int id);
        public Task<String> GetReportIdsForUser(int userId, bool isAdmin);
        public Task<IList<Report>> GetReportsByDashboardIdForUserAsync(int id, int userId, bool isAdmin);
        public Task<IList<Report>> GetReportsForUserAsync(int userId, bool isAdmin);
        public Task<Report?> GetReportByIdAsync(int id, string userReports);
        public Task<IList<Visual>> GetAllVisualsByReportIdAsync(int id);
        public bool CheckDashboardName(string dashboardName, int groupId);
        public bool SaveDashboard(Dashboard dashboard, int userId);
        public Tuple<bool, string> CanDeleteDashboard(int dashboardId);
        public Tuple<bool, string> DeleteDashboard(int dashboardId, int userId);
        public bool CanDeleteReport(int reportId);
        public bool SaveDashboardOrder(List<int> dashIds, int userId);
        public Task<string> GetDashboardHelpFileName(int dashboardId);
        public Task<bool> UploadFileToS3(string folderName, string fileName, string awsBucketName, MemoryStream memoryStream);
    }
}