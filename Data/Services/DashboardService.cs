using Microsoft.EntityFrameworkCore;
using TheradexPortal.Data.Models;
using TheradexPortal.Data.Services.Abstract;
using Microsoft.AspNetCore.Components;
using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;

namespace TheradexPortal.Data.Services
{
    public class DashboardService : BaseService, IDashboardService
    {
        private readonly IErrorLogService _errorLogService;
        private readonly NavigationManager _navManager;

        public DashboardService(IDatabaseConnectionService databaseConnectionService, IErrorLogService errorLogService, NavigationManager navigationManager) : base(databaseConnectionService)
        {
            _errorLogService = errorLogService;
            _navManager = navigationManager;
        }

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
                dashboards = await context.Dashboards.OrderBy(d=>d.DisplayOrder).Select(d=>d.DashboardId).ToListAsync();
            }
            else
            {
                dashboards = (from ur in context.User_Roles
                              join rd in context.Role_Dashboards on ur.RoleId equals rd.RoleId
                              join d in context.Dashboards on rd.DashboardId equals d.DashboardId
                              where ur.UserId == userId && (ur.ExpirationDate == null || ur.ExpirationDate.Value.Date >= DateTime.UtcNow.Date)
                              orderby d.DisplayOrder
                              select d.DashboardId).ToList();
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
                                    join d in context.Dashboards on rd.DashboardId equals d.DashboardId
                                    where ur.UserId == userId && (ur.ExpirationDate == null || ur.ExpirationDate.Value.Date >= DateTime.UtcNow.Date)
                                  select d).OrderBy(d => d.DisplayOrder).ToList();

                return dashboards;
            }
        }

        public async Task<Dashboard?> GetDashboardByIdAsync(int id, string userDashboards, bool isAdmin)
        {
            if (!isAdmin && !userDashboards.Contains("|" + id.ToString() + "|"))
                return null;
            else
                return await context.Dashboards.FindAsync(id);
        }

        public async Task<IList<Report>> GetReportsByDashboardIdAsync(int id)
        {
            var list =  await context.Reports.Where(r => r.DashboardId == id).OrderBy(r => r.DisplayOrder).ToListAsync();
            return list;
        }
        public async Task<IList<Report>> GetReportsForUserAsync(int userId, bool isAdmin)
        {
            if (isAdmin)
            {
                return await context.Reports.OrderBy(r => r.DisplayOrder).ToListAsync();
            }
            else
            {
                var reports = (from ur in context.User_Roles
                               join rr in context.Role_Reports on ur.RoleId equals rr.RoleId
                               join r in context.Reports on rr.ReportId equals r.ReportId
                               where ur.UserId == userId && (ur.ExpirationDate == null || ur.ExpirationDate.Value.Date >= DateTime.UtcNow.Date) 
                               select r).OrderBy(r => r.DisplayOrder).ToList();

                return reports;
            }
        }
        public async Task<String> GetReportIdsForUser(int userId, bool isAdmin)
        {
            string reportList = "";
            List<int> reports;

            if (isAdmin)
            {
                reports = await context.Reports.Select(d => d.ReportId).ToListAsync();
            }
            else
            {
                reports = (from ur in context.User_Roles
                           join rr in context.Role_Reports on ur.RoleId equals rr.RoleId
                           join r in context.Reports on rr.ReportId equals r.ReportId
                           where ur.UserId == userId && (ur.ExpirationDate == null || ur.ExpirationDate.Value.Date >= DateTime.UtcNow.Date)
                           select r.ReportId).ToList();
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
                                  join r in context.Reports on rr.ReportId equals r.ReportId
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

        public async Task<IList<Report?>> GetReportsByDashboardIdAndName(int id, string reportName, int userId, bool isAdmin)
        {
            if (isAdmin)
            {
                return await context.Reports.Where(r => r.DashboardId == id && r.Name == reportName).OrderBy(r => r.DisplayOrder).ToListAsync();
            }
            else
            {
                var reports = (from ur in context.User_Roles
                               join rr in context.Role_Reports on ur.RoleId equals rr.RoleId
                               join r in context.Reports on rr.ReportId equals r.ReportId
                               where ur.UserId == userId && (ur.ExpirationDate == null || ur.ExpirationDate.Value.Date >= DateTime.UtcNow.Date) && r.DashboardId == id && r.Name == reportName
                               select r).OrderBy(r => r.DisplayOrder).ToList();

                return reports;
            }
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
        public bool CheckDashboardName(string dashboardName, int dashboardId)
        {
            Dashboard foundDashboard = context.Dashboards.FirstOrDefault(d => d.Name.ToUpper() == dashboardName.ToUpper() && d.DashboardId != dashboardId);
            return foundDashboard == null;
        }

        public bool SaveDashboard(Dashboard dashboard, int userId)
        {
            DateTime curDateTime = DateTime.UtcNow;
            try
            {
                var primaryTable = context.Model.FindEntityType(typeof(Dashboard)).ToString().Replace("EntityType: ", "");

                if (dashboard.DashboardId == 0)
                {
                    dashboard.CreateDate = curDateTime;
                    // Get the highest DisplayOrder from dashboard list
                    int maxDisplayOrder = context.Dashboards.Max(d => d.DisplayOrder);
                    dashboard.DisplayOrder = maxDisplayOrder+1;
                    foreach (Report rep in dashboard.Reports)
                    {
                        rep.CreateDate = curDateTime;
                    }
                    context.Dashboards.Add(dashboard);
                    context.SaveChangesAsync(userId, primaryTable);
                }
                else
                {
                    Dashboard dbDashboard = context.Dashboards.FirstOrDefault(d => d.DashboardId == dashboard.DashboardId);
                    if (dbDashboard != null)
                    {
                        dbDashboard.Name = dashboard.Name;
                        dbDashboard.Description = dashboard.Description;
                        dbDashboard.CustomPagePath = dashboard.CustomPagePath;
                        dbDashboard.HelpFileName = dashboard.HelpFileName;
                        //context.Dashboards.Update(dbDashboard);

                        // Insert or udpate each report in the list
                        foreach (Report report in dashboard.Reports)
                        {
                            // Determine if it is a new or modified report
                            if (report.ReportId == 0)
                            {
                                report.CreateDate = curDateTime;
                                dbDashboard.UpdateDate = curDateTime;
                                dbDashboard.Reports.Add(report);
                            }
                            else
                            {
                                Report dbReport = dbDashboard.Reports.FirstOrDefault(d => d.ReportId == report.ReportId);
                                dbReport.ReportId = report.ReportId;
                                dbReport.Name = report.Name;
                                dbReport.Description = report.Description;
                                dbReport.DisplayOrder = report.DisplayOrder;
                                dbReport.CustomPagePath = report.CustomPagePath;
                                dbReport.DisplayIconName = report.DisplayIconName;
                                dbReport.SubMenuName = report.SubMenuName;
                                dbReport.PowerBIReportId = report.PowerBIReportId;
                                dbReport.ReportName = report.ReportName;
                                dbReport.PowerBIPageName = report.PowerBIPageName;
                                dbReport.PageName = report.PageName;
                                dbReport.ReportFilterId = report.ReportFilterId;
                                if (context.Entry(dbReport).State == EntityState.Modified)
                                {
                                    dbDashboard.UpdateDate = curDateTime;
                                    dbReport.UpdateDate = curDateTime;
                                }
                                //context.Reports.Update(dbReport);
                            }
                        }

                        // Delete reports no longer in the collection
                        List<Report> reportsToRemove = new List<Report>();
                        foreach (Report dr in dbDashboard.Reports)
                        {
                            Report foundReport = dashboard.Reports.FirstOrDefault(r => r.ReportId == dr.ReportId);
                            if (foundReport == null)
                            {
                                dbDashboard.UpdateDate = curDateTime;
                                reportsToRemove.Add(dr);
                            }
                        }
                        foreach (Report del in reportsToRemove)
                            dbDashboard.Reports.Remove(del);

                        if (context.Entry(dbDashboard).State == EntityState.Modified)
                        {
                            dbDashboard.UpdateDate = curDateTime;
                        }

                        context.SaveChangesAsync(userId, primaryTable);
                    }
                }

                return true;

            }
            catch (Exception ex)
            {
                _errorLogService.SaveErrorLogAsync(userId, _navManager.Uri, ex.InnerException, ex.Source, ex.Message, ex.StackTrace);
                return false;
            }
        }

        public Tuple<bool, string> CanDeleteDashboard(int dashboardId)
        {
            bool canDeleteDash = context.Role_Dashboards.Where(rd => rd.DashboardId == dashboardId).Count() == 0;

            List<Report> reportList = context.Reports.Where(r => r.DashboardId == dashboardId).ToList();

            bool canDeleteReport = true;
            foreach (Report report in reportList)
            {
                canDeleteReport = context.Role_Reports.Where(rr => rr.ReportId == report.ReportId).Count() == 0;
                if (!canDeleteReport)
                    break;
            }

            if (!canDeleteReport)
            {
                return new Tuple<bool, string>(false, "Can not delete. Report(s) in dashboard assigned to role(s).");
            }
            else if (!canDeleteDash)
            {
                return new Tuple<bool, string>(false, "Can not delete. Dashboard assigned to role(s).");
            }
            else
            {
                return new Tuple<bool, string>(true, "");
            }
        }

        public Tuple<bool, string> DeleteDashboard(int dashboardId, int userId)
        {
            try
            {
                var primaryTable = context.Model.FindEntityType(typeof(Dashboard)).ToString().Replace("EntityType: ", "");
                var delDashboard = context.Dashboards.Where(d => d.DashboardId == dashboardId).Include(d => d.Reports).First();
                context.Remove(delDashboard);
                context.SaveChangesAsync(userId, primaryTable);
                return new Tuple<bool, string>(true, "Dashboard deleted successfully");
            }
            catch (Exception ex)
            {
                _errorLogService.SaveErrorLogAsync(userId, _navManager.Uri, ex.InnerException, ex.Source, ex.Message, ex.StackTrace);
                return new Tuple<bool, string>(false, "Failed to delete dashboard");
            }
        }

        public bool CanDeleteReport(int reportId)
        {
            return context.Role_Reports.Where(rr => rr.ReportId == reportId).Count() == 0;
        }

        public bool SaveDashboardOrder(List<int> dashIds, int userId)
        {
            int dashOrder = 0;
            DateTime curDateTime = DateTime.UtcNow;
            try
            {
                var primaryTable = context.Model.FindEntityType(typeof(Dashboard)).ToString().Replace("EntityType: ", "");
                List<Dashboard> dashboards = context.Dashboards.OrderBy(d => d.DisplayOrder).ToList();

                foreach (int dashId in dashIds)
                {
                    dashOrder++;
                    Dashboard dashboard = dashboards.Find(d => d.DashboardId == dashId)!;
                    dashboard.DisplayOrder = dashOrder;
                    if (context.Entry(dashboard).State == EntityState.Modified)
                    {
                        dashboard.UpdateDate = curDateTime;
                    }
                }

                context.SaveChangesAsync(userId, primaryTable);
                return true;
            }
            catch (Exception ex)
            {
                _errorLogService.SaveErrorLogAsync(userId, _navManager.Uri, ex.InnerException, ex.Source, ex.Message, ex.StackTrace);
                return false;
            }
        }

        public async Task<string> GetDashboardHelpFileName(int dashboardId)
        {
            var helpUrl = string.Empty;
            var dashboard = await context.Dashboards.Where(db => db.DashboardId.Equals(dashboardId)).FirstOrDefaultAsync();
            if (dashboard != null && dashboard.HelpFileName != null)
            {
                helpUrl = dashboard.HelpFileName!.ToString();
            }
            return helpUrl;
        }

        public async Task<bool> UploadFileToS3(string folderName, string fileName, string awsBucketName, MemoryStream memoryStream)
        {
            string fileKey = string.Format("{0}/{1}", folderName, fileName);
            using (var client = new AmazonS3Client(RegionEndpoint.USEast1))
            {
                var uploadRequest = new TransferUtilityUploadRequest
                {
                    InputStream = memoryStream,
                    Key = fileKey,
                    BucketName = awsBucketName
                };

                var fileTransferUtility = new TransferUtility(client);
                await fileTransferUtility.UploadAsync(uploadRequest);
                return true;
            }
        }

        public async Task<ReportFilter?> GetReportFilterByIdAsync(int reportFilterId)
        {
            return await context.ReportFilters.Include(rf => rf.FilterItems).OrderBy(r => r.ReportFilterId).Where(r => r.ReportFilterId == reportFilterId).SingleOrDefaultAsync();
        }

        public async Task<List<ReportFilter>> GetReportFilterList()
        {
            return await context.ReportFilters.OrderBy(rf => rf.FilterName).ToListAsync();
        }
    }
}
