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
                    // Delete old entry for any newly selected study
                    foreach (string selectedStudy in studyList)
                    {
                        List<UserProtocolHistory> histsToDelete = context.User_ProtocolHistory.Where(uph => uph.StudyId == selectedStudy).ToList();
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

        public async Task<IList<string>> GetProtocolHistoryAsync(int userId, int count)
        {
            return await context.User_ProtocolHistory.Where(p1=>p1.UserId == userId).OrderByDescending(p=>p.WRUserProtocolHistoryId).Select(p=>p.StudyId).Take(count).ToListAsync();
        }
    }
}
