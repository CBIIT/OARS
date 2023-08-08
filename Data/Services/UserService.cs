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

        public bool SaveSelectedStudies(int userId, string studies)
        {
            try
            {
                // Get the user, update and save.
                User user = context.Users.FirstOrDefault(u => u.UserId == userId);
                user.CurrentStudy = studies;
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
