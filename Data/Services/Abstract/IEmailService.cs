using TheradexPortal.Data.Models;

namespace TheradexPortal.Data.Services.Abstract
{
    public interface IEmailService
    {
        public Task<bool> SendNewUserEmail(string siteName, string baseURL, string primaryColor, User curUser, string activationLink);
        public Task<bool> SendNewSystemEmail(string siteName, string baseURL, string primaryColor, User curUser);
        public Task<bool> SendEmail(string toAddress, string subject, string htmlBody);
        public Task<bool> SendEmail(List<string> toAddresses, List<string> ccAddresses, List<string> bccAddresses, string subject, string htmlBody, List<string> attachmeents);
    }
}
