using Microsoft.EntityFrameworkCore;
using TheradexPortal.Data.Models;
using TheradexPortal.Data.Services.Abstract;
using TheradexPortal.Data.Static;

namespace TheradexPortal.Data.Services
{
    public class UserService : BaseService, IUserService
    {
        public UserService(IDbContextFactory<WrDbContext> dbFactory) : base(dbFactory) { }

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
        public bool SaveUser(User user)
        {
            try
            {
                DateTime curDateTime = DateTime.UtcNow;
                DateTime updDateTime = curDateTime;

                // if user.UserId is 0 or null, save new user, else edit user
                if (user.UserId == 0)
                {
                    user.CreateDate = curDateTime;
                    context.Users.Add(user);

                    context.SaveChanges();
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
                        dbUser.IsLockedOut = user.IsLockedOut;
                        dbUser.UpdateDate = curDateTime;

                        // User Roles
                        foreach (UserRole ur in user.UserRoles)
                        {
                            UserRole foundUR = dbUser.UserRoles.FirstOrDefault(ur2 => ur2.RoleId == ur.RoleId);
                            if (foundUR != null)
                            {
                                // UserRole already exists
                                foundUR.ExpirationDate = ur.ExpirationDate;
                                foundUR.UpdateDate = updDateTime;
                            }
                            else
                            {
                                // UserRole doesn't exist in db
                                UserRole newUR = new UserRole();
                                newUR.RoleId = ur.RoleId;
                                newUR.ExpirationDate = ur.ExpirationDate;
                                newUR.CreateDate = curDateTime;
                                dbUser.UserRoles.Add(newUR);
                            }
                        }
                        // Remove UserRoles in db not in new list
                        List<UserRole> urToRemove = new List<UserRole>();
                        foreach (UserRole dur in dbUser.UserRoles)
                        {
                            UserRole foundUR = user.UserRoles.FirstOrDefault(ur2 => ur2.RoleId == dur.RoleId);
                            if (foundUR == null)
                                urToRemove.Add(dur);
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
                                foundUP.UpdateDate = updDateTime;
                            }
                            else
                            {
                                // UserProtocol doesn't exist in db
                                UserProtocol newUP = new UserProtocol();
                                newUP.StudyId = up.StudyId;
                                newUP.ExpirationDate = up.ExpirationDate;
                                newUP.CreateDate = curDateTime;
                                dbUser.UserProtocols.Add(newUP);
                            }
                        }
                        // Remove UserProtocols in db not in new list
                        List<UserProtocol> upToRemove = new List<UserProtocol>();
                        foreach (UserProtocol dup in dbUser.UserProtocols)
                        {
                            UserProtocol foundUP = user.UserProtocols.FirstOrDefault(up2 => up2.StudyId == dup.StudyId);
                            if (foundUP == null)
                                upToRemove.Add(dup);
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
                                foundUG.UpdateDate = updDateTime;
                            }
                            else
                            {
                                // UserGroup doesn't exist in db
                                UserGroup newUG = new UserGroup();
                                newUG.GroupId = ug.GroupId;
                                newUG.ExpirationDate = ug.ExpirationDate;
                                newUG.CreateDate = curDateTime;
                                dbUser.UserGroups.Add(newUG);
                            }
                        }
                        // Remove UserGroups in db not in new list
                        List<UserGroup> ugToRemove = new List<UserGroup>();
                        foreach (UserGroup dug in dbUser.UserGroups)
                        {
                            UserGroup foundUG = user.UserGroups.FirstOrDefault(ug2 => ug2.GroupId == dug.GroupId);
                            if (foundUG == null)
                                ugToRemove.Add(dug);
                        }
                        foreach (UserGroup del in ugToRemove)
                            dbUser.UserGroups.Remove(del);

                        context.SaveChanges();
                    }
                }

                return true;

            }
            catch (Exception ex)
            {
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
                return false;
            }
        }
        public bool SaveSelectedStudies(int userId, string studies, bool saveRecent)
        {
            try
            {
                // Get the user, update and save.
                User user = context.Users.FirstOrDefault(u => u.UserId == userId);

                string[] studyList = studies.Split(',');

                if (!studyList.Contains(user.CurrentStudy))
                    user.CurrentStudy = studyList[0];

                user.SelectedStudies = studies;

                if (saveRecent)
                {
                    // Delete old entry in UserProtocolHistory for any newly selected study
                    foreach (string selectedStudy in studyList)
                    {
                        List<UserProtocolHistory> histsToDelete = context.User_ProtocolHistory.Where(uph => uph.UserId == userId && uph.StudyId == selectedStudy).ToList();
                        foreach (UserProtocolHistory history in histsToDelete)
                            context.User_ProtocolHistory.Remove(history);

                    }
                    List<UserProtocolHistory> newHistory = new List<UserProtocolHistory>();
                    foreach (string selectedStudy in studyList)
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
                return false;
            }
        }
        public bool SaveCurrentStudy(int userId, string study)
        {
            try
            {
                // Get the user, update and save.
                User user = context.Users.FirstOrDefault(u => u.UserId == userId);
                user.CurrentStudy = study;

                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> SetStartingStudies(int userId, int count)
        {
            string studyList = "";
            bool saved = true;
            IList<string> studies = await GetProtocolHistoryAsync(userId, count);
            // Reverse it

            if (studies != null && studies.Count != 0)
            {
                for (int i = studies.Count-1; i >= 0; i--)
                    studyList += studies[i] + ",";
                studyList = studyList.Trim(',');
                saved = SaveSelectedStudies(userId, studyList, true);
            }

            saved = SaveActivityLog(userId, WRActivityType.Study, "Filter Studies-Login", studyList);

            return saved;
        }
        public bool DeactivateUser(int userId)
        {
            try
            {
                // Get the user, update and save.
                User user = context.Users.FirstOrDefault(u => u.UserId == userId);
                user.IsActive = false;
                user.UpdateDate = DateTime.UtcNow;
                context.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public async Task<IList<string>> GetProtocolHistoryAsync(int userId, int count)
        {
            return await context.User_ProtocolHistory.Where(p1=>p1.UserId == userId).OrderByDescending(p=>p.WRUserProtocolHistoryId).Select(p=>p.StudyId).Take(count).ToListAsync();
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
                return false;
            }
        }
    }
}
