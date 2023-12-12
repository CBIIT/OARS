using Microsoft.EntityFrameworkCore;
using TheradexPortal.Data.Models;
using TheradexPortal.Data.Services.Abstract;
using TheradexPortal.Data.Static;
using Microsoft.AspNetCore.Components;

namespace TheradexPortal.Data.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly IErrorLogService _errorLogService;
        private readonly NavigationManager _navManager;

        public UserService(IDbContextFactory<ThorDBContext> dbFactory, IErrorLogService errorLogService, NavigationManager navigationManager) : base(dbFactory)
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
            User foundUser = context.Users.FirstOrDefault(u=>u.EmailAddress == emailAddress && u.UserId != userId);
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
    }
}
