using OARS.Data.Models;

namespace OARS.Data.Services.Abstract
{
    public interface IOktaService
    {
        public Task<Tuple<bool, string>> CreateUser(bool isProd, User curUser, bool isCTEP, bool isActive, string initialURL, string initialSite);
        public Task<Tuple<bool, string>> UpdateUserInfo(string origEmail, User curUser);
        public Task<Tuple<bool, string>> UpdateUserCTEP(string emailAddress, bool isCTEPUser);
        public Task<Tuple<bool, string>> UpdateActiveStatus(string userId, bool isActive, bool isCTEP);
        public Task<Tuple<bool, string>> ReActivateUser(string emailAddress);
        public Task<Tuple<bool, string>> AddUserGroup(string emailAddress, bool isCTEPUser);
        public Task<Tuple<bool, string>> FindUser(string emailAddress);
        public Task<Tuple<bool, string>> GetUserStatus(string emailAddress);
        public Task<Tuple<bool, string>> UnlockUser(string emailAddress);
        public Task<Tuple<bool, string>> GetGroupList();

    }
}
