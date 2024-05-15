using Microsoft.EntityFrameworkCore;
using TheradexPortal.Data.Models;
using TheradexPortal.Data.Services.Abstract;
using TheradexPortal.Data.Static;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using TheradexPortal.Data.Models.Configuration;
using Blazorise;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using Polly;


namespace TheradexPortal.Data.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly IErrorLogService _errorLogService;
        private readonly NavigationManager _navManager;

        public UserService(IDatabaseConnectionService databaseConnectionService, IErrorLogService errorLogService, NavigationManager navigationManager) : base(databaseConnectionService)
        {
            _errorLogService = errorLogService;
            _navManager = navigationManager;
        }

        public async Task<IList<User>> GetAllUsersAsync()
        {
            return await context.Users.ToListAsync();
        }
        public async Task<User?> GetUserAsync(int userId)
        {
            return await context.Users.Include(ur=>ur.UserRoles).ThenInclude(r=>r.Role).Include(up=>up.UserProtocols).Include(ug=>ug.UserGroups).ThenInclude(g=>g.Group).FirstOrDefaultAsync(u => u.UserId == userId);
        }
        public async Task<User?> GetUserByEmailAsync(string emailAddress)
        {
            return await context.Users.FirstOrDefaultAsync(u => u.EmailAddress == emailAddress);
        }
        public bool CheckEmailAddress(string emailAddress, int userId)
        {
            User foundUser = context.Users.FirstOrDefault(u=>u.EmailAddress.ToUpper() == emailAddress.ToUpper() && u.UserId != userId);
            return foundUser == null;
        }
        public bool SaveUser(User user, int loggedInUserId)
        {
            try
            {
                DateTime curDateTime = DateTime.UtcNow;
                DateTime updDateTime = curDateTime;
                var primaryTable = context.Model.FindEntityType(typeof(User)).ToString().Replace("EntityType: ", "");

                // if user.UserId is 0 or null, save new user, else edit user
                if (user.UserId == 0)
                {
                    user.CreateDate = curDateTime;
                    user.TimeZoneAbbreviation = "GMT";
                    user.TimeOffset = 0;
                    context.Users.Add(user);

                    context.SaveChangesAsync(loggedInUserId, primaryTable);
                }
                else
                {
                    // Get the user, roles, protocols & groups, update the values & save
                    User dbUser = context.Users.FirstOrDefault(u => u.UserId == user.UserId);
                    if (dbUser != null)
                    {
                        // User Info
                        dbUser.FirstName = user.FirstName;
                        dbUser.LastName = user.LastName;
                        dbUser.EmailAddress = user.EmailAddress;
                        dbUser.Title = user.Title;
                        dbUser.IsActive = user.IsActive;
                        dbUser.IsCtepUser = user.IsCtepUser;
                        dbUser.CtepUserId = user.CtepUserId;
                        dbUser.AllStudies = user.AllStudies;

                        // User Roles
                        foreach (UserRole ur in user.UserRoles)
                        {
                            UserRole foundUR = dbUser.UserRoles.FirstOrDefault(ur2 => ur2.RoleId == ur.RoleId);
                            if (foundUR != null)
                            {
                                // UserRole already exists
                                foundUR.ExpirationDate = ur.ExpirationDate;
                                if (context.Entry(foundUR).State == EntityState.Modified)
                                {
                                    dbUser.UpdateDate = curDateTime;
                                    foundUR.UpdateDate = curDateTime;
                                }
                            }
                            else
                            {
                                // UserRole doesn't exist in db
                                UserRole newUR = new UserRole();
                                newUR.RoleId = ur.RoleId;
                                newUR.ExpirationDate = ur.ExpirationDate;
                                newUR.CreateDate = curDateTime;
                                dbUser.UpdateDate = curDateTime;
                                dbUser.UserRoles.Add(newUR);
                            }
                        }
                        // Remove UserRoles in db not in new list
                        List<UserRole> urToRemove = new List<UserRole>();
                        foreach (UserRole dur in dbUser.UserRoles)
                        {
                            UserRole foundUR = user.UserRoles.FirstOrDefault(ur2 => ur2.RoleId == dur.RoleId);
                            if (foundUR == null)
                            {
                                dbUser.UpdateDate = curDateTime;
                                urToRemove.Add(dur);
                            }
                        }
                        foreach (UserRole del in urToRemove)
                            dbUser.UserRoles.Remove(del);

                        // User Protocols
                        foreach (UserProtocol up in user.UserProtocols)
                        {
                            UserProtocol foundUP = dbUser.UserProtocols.FirstOrDefault(up2 => up2.StudyId == up.StudyId);
                            if (foundUP != null)
                            {
                                // UserProtocol already exists
                                foundUP.ExpirationDate = up.ExpirationDate;
                                if (context.Entry(foundUP).State == EntityState.Modified)
                                {
                                    dbUser.UpdateDate = curDateTime;
                                    foundUP.UpdateDate = curDateTime;
                                }
                            }
                            else
                            {
                                // UserProtocol doesn't exist in db
                                UserProtocol newUP = new UserProtocol();
                                newUP.StudyId = up.StudyId;
                                newUP.ExpirationDate = up.ExpirationDate;
                                newUP.CreateDate = curDateTime;
                                dbUser.UpdateDate = curDateTime;
                                dbUser.UserProtocols.Add(newUP);
                            }
                        }
                        // Remove UserProtocols in db not in new list
                        List<UserProtocol> upToRemove = new List<UserProtocol>();
                        foreach (UserProtocol dup in dbUser.UserProtocols)
                        {
                            UserProtocol foundUP = user.UserProtocols.FirstOrDefault(up2 => up2.StudyId == dup.StudyId);
                            if (foundUP == null)
                            {
                                dbUser.UpdateDate = curDateTime;
                                upToRemove.Add(dup);
                            }
                        }
                        foreach (UserProtocol del in upToRemove)
                            dbUser.UserProtocols.Remove(del);

                        // User Groups
                        foreach (UserGroup ug in user.UserGroups)
                        {
                            UserGroup foundUG = dbUser.UserGroups.FirstOrDefault(ug2 => ug2.GroupId == ug.GroupId);
                            if (foundUG != null)
                            {
                                // UserGroup already exists
                                foundUG.ExpirationDate = ug.ExpirationDate;
                                if (context.Entry(foundUG).State == EntityState.Modified)
                                {
                                    dbUser.UpdateDate = curDateTime;
                                    foundUG.UpdateDate = curDateTime;
                                }
                            }
                            else
                            {
                                // UserGroup doesn't exist in db
                                UserGroup newUG = new UserGroup();
                                newUG.GroupId = ug.GroupId;
                                newUG.ExpirationDate = ug.ExpirationDate;
                                newUG.CreateDate = curDateTime;
                                dbUser.UpdateDate = curDateTime;
                                dbUser.UserGroups.Add(newUG);
                            }
                        }
                        // Remove UserGroups in db not in new list
                        List<UserGroup> ugToRemove = new List<UserGroup>();
                        foreach (UserGroup dug in dbUser.UserGroups)
                        {
                            UserGroup foundUG = user.UserGroups.FirstOrDefault(ug2 => ug2.GroupId == dug.GroupId);
                            if (foundUG == null)
                            {
                                dbUser.UpdateDate = curDateTime;
                                ugToRemove.Add(dug);
                            }
                        }
                        foreach (UserGroup del in ugToRemove)
                            dbUser.UserGroups.Remove(del);

                        if (context.Entry(dbUser).State == EntityState.Modified)
                        {
                            dbUser.UpdateDate = curDateTime;
                        }

                        context.SaveChangesAsync(loggedInUserId, primaryTable);
                    }
                }

                return true;

            }
            catch (Exception ex)
            {
                _errorLogService.SaveErrorLogAsync(loggedInUserId, _navManager.Uri, ex.InnerException, ex.Source, ex.Message, ex.StackTrace);
                return false;
            }
        }
        public bool SaveLastLoginDate(int userId)
        {
            try
            {
                User user = context.Users.FirstOrDefault(u => u.UserId == userId);
                user.LastLoginDate = DateTime.UtcNow;
                context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                _errorLogService.SaveErrorLogAsync(userId, _navManager.Uri, ex.InnerException, ex.Source, ex.Message, ex.StackTrace);
                return false;
            }
        }
        public bool SaveSelectedStudies(int userId, string selectedStudies, bool saveRecent)
        {
            try
            {
                // Get the user, update and save.
                var userSelectedProtocols = context.User_Selected_Protocols.FirstOrDefault(u => u.UserId == userId);
                if (userSelectedProtocols == null)
                {
                    userSelectedProtocols = new UserSelectedProtocols();
                    userSelectedProtocols.UserId = userId;
                    userSelectedProtocols.Selected_Protocols = selectedStudies;
                    userSelectedProtocols.Current_Protocols = selectedStudies;
                    context.User_Selected_Protocols.Add(userSelectedProtocols);
                    userSelectedProtocols.CreateDate = DateTime.UtcNow;
                }
                else
                {
                    userSelectedProtocols.Selected_Protocols = selectedStudies;
                    userSelectedProtocols.Current_Protocols = selectedStudies;
                    userSelectedProtocols.UpdateDate = DateTime.UtcNow;
                }

                string[] newSelectedStudies = selectedStudies.Split(',');
                if (saveRecent)
                {
                    // Delete old entry in UserProtocolHistory for any newly selected study
                    foreach (string selectedStudy in newSelectedStudies)
                    {
                        List<UserProtocolHistory> histsToDelete = context.User_ProtocolHistory.Where(uph => uph.UserId == userId && uph.StudyId == selectedStudy).ToList();
                        foreach (UserProtocolHistory history in histsToDelete)
                            context.User_ProtocolHistory.Remove(history);

                    }
                    List<UserProtocolHistory> newHistory = new List<UserProtocolHistory>();
                    foreach (string selectedStudy in newSelectedStudies)
                    {
                        UserProtocolHistory newUPH = new UserProtocolHistory();
                        newUPH.UserId = userId;
                        newUPH.StudyId = selectedStudy;
                        newUPH.CreateDate = DateTime.UtcNow;
                        context.User_ProtocolHistory.Add(newUPH);
                    }
                }

                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                _errorLogService.SaveErrorLogAsync(userId, _navManager.Uri, ex.InnerException, ex.Source, ex.Message, ex.StackTrace);
                return false;
            }
        }
        public bool SaveCurrentStudy(int userId, string currentStudies)
        {
            try
            {
                // Get the user's selected protocols, update and save.
                UserSelectedProtocols userSelectedProtocols = context.User_Selected_Protocols.FirstOrDefault(u => u.UserId == userId);
                userSelectedProtocols.Current_Protocols = currentStudies;

                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                _errorLogService.SaveErrorLogAsync(userId, _navManager.Uri, ex.InnerException, ex.Source, ex.Message, ex.StackTrace);
                return false;
            }
        }
        public async Task<bool> SetStartingStudies(int userId, int count)
        {
            try
            {
                string studyList = "";
                bool saved = true;
                IList<string> studies = await GetProtocolHistoryAsync(userId, count);
                // Reverse it

                if (studies != null && studies.Count != 0)
                {
                    for (int i = studies.Count - 1; i >= 0; i--)
                        studyList += studies[i] + ",";
                    studyList = studyList.Trim(',');
                    saved = SaveSelectedStudies(userId, studyList, true);
                }

                saved = SaveActivityLog(userId, ThorActivityType.Study, "Filter Studies-Login", studyList);

                return saved;
            }
            catch (Exception ex)
            {
                _errorLogService.SaveErrorLogAsync(userId, _navManager.Uri, ex.InnerException, ex.Source, ex.Message, ex.StackTrace);
                return false;
            }
        }
        public bool DeactivateUser(int userId, int loggedInUserId)
        {
            try
            {
                // Get the user, update and save.
                var primaryTable = context.Model.FindEntityType(typeof(User)).ToString().Replace("EntityType: ", "");
                User user = context.Users.FirstOrDefault(u => u.UserId == userId);
                user.IsActive = false;
                user.UpdateDate = DateTime.UtcNow;
                context.SaveChangesAsync(loggedInUserId, primaryTable);

                return true;
            }
            catch (Exception ex)
            {
                _errorLogService.SaveErrorLogAsync(loggedInUserId, _navManager.Uri, ex.InnerException, ex.Source, ex.Message, ex.StackTrace);
                return false;
            }
        }
        public async Task<IList<string>> GetProtocolHistoryAsync(int userId, int count)
        {
            return await context.User_ProtocolHistory.Where(p1=>p1.UserId == userId).OrderByDescending(p=>p.UserProtocolHistoryId).Select(p=>p.StudyId).Take(count).ToListAsync();
        }
        public void SaveTimeZoneInfo(int userId, string timeZoneAbbrev, TimeSpan currentOffset)
        {
            try
            {
                var primaryTable = context.Model.FindEntityType(typeof(User)).ToString().Replace("EntityType: ", "");
                User user = context.Users.FirstOrDefault(u => u.UserId == userId);
                if (user.TimeZoneAbbreviation != timeZoneAbbrev)
                {
                    user.TimeZoneAbbreviation = timeZoneAbbrev;
                    user.TimeOffset = Convert.ToInt32(currentOffset.TotalMinutes);

                    context.SaveChangesAsync(userId, primaryTable);
                }
            }
            catch (Exception ex)
            {
                _errorLogService.SaveErrorLogAsync(userId, _navManager.Uri, ex.InnerException, ex.Source, ex.Message, ex.StackTrace);
            }
        }
        public bool SaveActivityLog(int userId, string activityType)
        {
            return SaveActivityLog(userId, activityType, null);
        }
        public bool SaveActivityLog(int userId, string activityType, string data1)
        {
            return SaveActivityLog(userId, activityType, data1, null);
        }
        public bool SaveActivityLog(int userId, string activityType, string data1, string data2)
        {
            try
            {
                UserActivityLog newUal = new UserActivityLog();
                newUal.UserId = userId;
                newUal.ActivityType = activityType;
                newUal.ActivityDate = DateTime.UtcNow;
                newUal.Data1 = data1;
                newUal.Data2 = data2;

                context.User_ActivityLog.Add(newUal);
                context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                _errorLogService.SaveErrorLogAsync(userId, _navManager.Uri, ex.InnerException, ex.Source, ex.Message, ex.StackTrace);
                return false;
            }
        }
        public bool CheckActivityLogForTimeout(int userId, int timespanMS)
        {
            // Get the lost recent non-Timeout-check record from Activity log based on the timespan
            var mostRecentActivity = ( from ua in context.User_ActivityLog
                                       where ua.UserId == userId && ua.Data1 != "Timeout-Check"
                                       orderby ua.ActivityDate descending
                                       select ua).FirstOrDefault();

            DateTime curDateTime = DateTime.UtcNow;

            // Return true to force timeout
            return mostRecentActivity.ActivityDate.Value.AddMilliseconds(timespanMS) < curDateTime;
        }
        public Tuple<bool, string> SaveFavorite(int userId, int dashboardId, int reportId, string reportName)
        {
            try
            {
                List<UserFavorite> currentItem =
                    context.User_Favorite.Where(u => u.UserId == userId && u.DashboardId == dashboardId && u.ReportId == reportId).ToList();
                if (currentItem.Count == 0)
                {
                    var configuration = new ConfigurationBuilder().AddJsonFile($"appsettings.json");
                    var config = configuration.Build();
                    int maxReportNumber = Convert.ToInt32(config["MyFavoriteMaxPerDashboard"]);

                    int reportCount = context.User_Favorite.Where(u => u.UserId == userId && u.DashboardId == dashboardId).Count();
                    if (reportCount >= maxReportNumber)
                    {
                        return new Tuple<bool, string>(false, "Already reached the maximum number of favorite reports under the same Dashboard.");
                    }

                    UserFavorite newUserFavorite = new UserFavorite();
                    newUserFavorite.UserId = userId;
                    newUserFavorite.DashboardId = dashboardId;
                    newUserFavorite.ReportId = reportId;
                    newUserFavorite.ReportName = reportName;
                    Report report = context.Reports.Where(r => r.ReportId == reportId).SingleOrDefault();
                    if (report != null)
                    {
                        newUserFavorite.DisplayIconName = report.DisplayIconName;
                    }
                    newUserFavorite.CreateDate = DateTime.UtcNow;

                    context.User_Favorite.Add(newUserFavorite);

                    var primaryTable = context.Model.FindEntityType(typeof(UserFavorite)).ToString().Replace("EntityType: ", "");
                    context.SaveChangesAsync(userId, primaryTable);
                    return new Tuple<bool, string>(true, "");
                }
                else
                {
                    return new Tuple<bool, string>(false, "This report has already been added as the favorite report.");
                }
            }
            catch (Exception ex)
            {
                _errorLogService.SaveErrorLogAsync(userId, _navManager.Uri, ex.InnerException, ex.Source, ex.Message, ex.StackTrace);
                return new Tuple<bool, string>(false,"");
            }
            return new Tuple<bool, string>(false, "");
        }
        public List<FavoriteReportItem> GetUserFavoriteList(int userId, bool isAdmin)
        {
            List<FavoriteReportItem> lstUserFavorite = new List<FavoriteReportItem>();
            FavoriteReportItem reportItem;
            string reportName = string.Empty;
            List<UserFavorite> userSavedFavorites = (  from uf in context.User_Favorite 
                                                       join dashboardlist  in context.Dashboards on uf.DashboardId equals dashboardlist.DashboardId
                                                       join reportList in context.Reports on uf.ReportId equals reportList.ReportId
                                                       where uf.UserId == userId
                                                       orderby dashboardlist.DisplayOrder, reportList.DisplayOrder
                                                       select uf).ToList();
            List<Dashboard> dashboards = new List<Dashboard>();
            if (isAdmin)
            {
                dashboards = context.Dashboards.OrderBy(d => d.DisplayOrder).ToList();
            }
            else
            {
                dashboards = (from ur in context.User_Roles
                              join rd in context.Role_Dashboards on ur.RoleId equals rd.RoleId
                              join d in context.Dashboards on rd.DashboardId equals d.DashboardId
                              where ur.UserId == userId && (ur.ExpirationDate == null || ur.ExpirationDate.Value.Date >= DateTime.UtcNow.Date)
                              orderby d.DisplayOrder
                              select d).ToList();
            }
            List<Report> reports = new List<Report>();
            if (isAdmin)
            {
                reports =  context.Reports.ToList();
            }
            else
            {
                reports = (from ur in context.User_Roles
                           join rr in context.Role_Reports on ur.RoleId equals rr.RoleId
                           join r in context.Reports on rr.ReportId equals r.ReportId
                           where ur.UserId == userId && (ur.ExpirationDate == null || ur.ExpirationDate.Value.Date >= DateTime.UtcNow.Date)
                           select r).ToList();
            }

            if (userSavedFavorites != null)
            {
                foreach (UserFavorite uf in userSavedFavorites)
                {
                    if (uf.DashboardId != null)
                    {
                        Dashboard saveDashboard = dashboards.Where(d => d.DashboardId == uf.DashboardId).SingleOrDefault();
                        if (saveDashboard != null)
                        {
                            FavoriteReportItem currentItem = lstUserFavorite.Where(l => l.DashboardId == Convert.ToInt32(uf.DashboardId)).FirstOrDefault();
                            if (currentItem == null)
                            {
                                reportItem = new FavoriteReportItem();
                                reportItem.DashboardId = saveDashboard.DashboardId;
                                reportItem.DisplayName = saveDashboard.Name;
                                reportItem.isDashboard = true;
                                if (uf.ReportId != null)
                                {
                                    Report report = reports.Where(r => r.ReportId == uf.ReportId).SingleOrDefault();
                                    if (report != null)
                                    {
                                        reportItem.ReportList = new List<Report>();
                                        reportItem.ReportList.Add(report);
                                        reportItem.UserFavoriteIdList = new List<int>();
                                        reportItem.UserFavoriteIdList.Add(uf.UserFavoriteId);
                                    }
                                }
                                lstUserFavorite.Add(reportItem);
                            }
                            else
                            {
                                if (uf.ReportId != null)
                                {
                                    Report report = reports.Where(r => r.ReportId == uf.ReportId).SingleOrDefault();
                                    if (report != null)
                                    {

                                        if (currentItem.ReportList != null)
                                        {
                                            currentItem.ReportList.Add(report);
                                            currentItem.UserFavoriteIdList.Add(uf.UserFavoriteId);
                                        }
                                        else
                                        {
                                            currentItem.ReportList = new List<Report>();
                                            currentItem.ReportList.Add(report);
                                            currentItem.UserFavoriteIdList = new List<int>();
                                            currentItem.UserFavoriteIdList.Add(uf.UserFavoriteId);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return lstUserFavorite;
        }

        public bool HasUserFavorite(int userId, bool isAdmin) 
        {
        
            List<UserFavorite> userSavedFavorites = (from uf in context.User_Favorite
                                                     join dashboardlist in context.Dashboards on uf.DashboardId equals dashboardlist.DashboardId
                                                     join reportList in context.Reports on uf.ReportId equals reportList.ReportId
                                                     where uf.UserId == userId
                                                     orderby dashboardlist.DisplayOrder, reportList.DisplayOrder
                                                     select uf).ToList();
            List<Dashboard> dashboards = new List<Dashboard>();
            if (isAdmin)
            {
                dashboards = context.Dashboards.OrderBy(d => d.DisplayOrder).ToList();
            }
            else
            {
                dashboards = (from ur in context.User_Roles
                              join rd in context.Role_Dashboards on ur.RoleId equals rd.RoleId
                              join d in context.Dashboards on rd.DashboardId equals d.DashboardId
                              where ur.UserId == userId && (ur.ExpirationDate == null || ur.ExpirationDate.Value.Date >= DateTime.UtcNow.Date)
                              orderby d.DisplayOrder
                              select d).ToList();
            }
            List<Report> reports = new List<Report>();
            if (isAdmin)
            {
                reports = context.Reports.ToList();
            }
            else
            {
                reports = (from ur in context.User_Roles
                           join rr in context.Role_Reports on ur.RoleId equals rr.RoleId
                           join r in context.Reports on rr.ReportId equals r.ReportId
                           where ur.UserId == userId && (ur.ExpirationDate == null || ur.ExpirationDate.Value.Date >= DateTime.UtcNow.Date)
                           select r).ToList();
            }

            if (userSavedFavorites != null)
            {
                foreach (UserFavorite uf in userSavedFavorites)
                {
                    if (uf.DashboardId != null)
                    {
                        Dashboard saveDashboard = dashboards.Where(d => d.DashboardId == uf.DashboardId).SingleOrDefault();
                        if (saveDashboard != null)
                        {
                            if (uf.ReportId != null)
                            {
                                Report report = reports.Where(r => r.ReportId == uf.ReportId).SingleOrDefault();
                                if (report != null)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }
        public UserFavorite GetUserFavoriteFirstDashboardReport(int userId, bool isAdmin)
        {
            UserFavorite ufItem;
            if (isAdmin)
            {
                ufItem = (from uf in context.User_Favorite
                          join dashboardlist in context.Dashboards on uf.DashboardId equals dashboardlist.DashboardId
                          join reportList in context.Reports on uf.ReportId equals reportList.ReportId
                          where uf.UserId == userId                     
                          orderby dashboardlist.DisplayOrder, reportList.DisplayOrder
                          select uf).FirstOrDefault();
            }
            else
            {
                ufItem = (from uf in context.User_Favorite
                          join dashboardlist in context.Dashboards on uf.DashboardId equals dashboardlist.DashboardId
                          join reportList in context.Reports on uf.ReportId equals reportList.ReportId
                          join ur in context.User_Roles on uf.UserId equals ur.UserId
                          join rd in context.Role_Dashboards on ur.RoleId equals rd.RoleId
                          join rr in context.Role_Reports on ur.RoleId equals rr.RoleId
                          where uf.UserId == userId && rd.DashboardId == uf.DashboardId && rr.ReportId == uf.ReportId
                          && (ur.ExpirationDate == null || ur.ExpirationDate.Value.Date >= DateTime.UtcNow.Date)
                          orderby dashboardlist.DisplayOrder, reportList.DisplayOrder
                          select uf).FirstOrDefault();
            }
            if (ufItem != null)
            {
                return ufItem;
            }
            return null;
        }

        public bool IsReportFavorite(int userId, int reportId)
        {
            UserFavorite ufItem = context.User_Favorite.Where(uf => uf.UserId == userId && uf.ReportId == reportId).SingleOrDefault();
            if (ufItem != null)
            {
                return true;
            }
            return false;
        }
        public string GetUserFavoriteNamesByID(int userFavoriteId)
        {
            string name = string.Empty;
            UserFavorite ufItem = context.User_Favorite.Where(u => u.UserFavoriteId == userFavoriteId).FirstOrDefault();
            if (ufItem != null)
            {
                Dashboard dashboard = context.Dashboards.Where(d => d.DashboardId == ufItem.DashboardId).FirstOrDefault();
                Report report = context.Reports.Where(r => r.ReportId == ufItem.ReportId).FirstOrDefault();
                if (dashboard != null && report != null)
                {
                    name = dashboard.Name + "-" + report.Name;
                }
            }
            return name;
        }
        public bool RemoveFavorite(int userId ,int userFavoriteId)
        {
            UserFavorite currentItem =
                  context.User_Favorite.Where(u => u.UserFavoriteId == userFavoriteId).FirstOrDefault();
            if (currentItem != null)
            {
                context.User_Favorite.Remove(currentItem);
                var primaryTable = context.Model.FindEntityType(typeof(UserFavorite)).ToString().Replace("EntityType: ", "");
                context.SaveChangesAsync(userId, primaryTable);              
                return true;
            }
            return false ;
        }

        public List<string> GetProtocolAccessForUser(int userId)
        {
            List<string> protocolIds = new List<string>();

            var userProtocols = context.User_Protocols.Where(up => (up.UserId == userId && (up.ExpirationDate == null || up.ExpirationDate > DateTime.Now))).ToList();
            var groupProtocols = context.Group_Protocols.Where(gp => (gp.GroupId == context.User_Groups.Where(ug => ug.UserId == userId).Select(ug => ug.GroupId).FirstOrDefault()) && gp.IsActive).ToList();
            protocolIds.AddRange(userProtocols.Select(up => up.StudyId));
            protocolIds.AddRange(groupProtocols.Select(gp => gp.StudyId));

            return protocolIds;
        }
    }
}
