using Microsoft.EntityFrameworkCore;
using TheradexPortal.Data.Models;
using TheradexPortal.Data.Services.Abstract;

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
            return await context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task<User?> GetUserByEmailAsync(string emailAddress)
        {
            return await context.Users.FirstOrDefaultAsync(u => u.EmailAddress == emailAddress);
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
