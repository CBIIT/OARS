using TheradexPortal.Data.Models;

namespace TheradexPortal.Data.Services.Abstract
{
    public interface IEmailService
    {
        public Task<bool> SendNewUserEmail(string siteName, string siteAcronym, string siteNameShort, string baseURL, string primaryColor, User curUser, string activationLink);
        public Task<bool> SendNewSystemEmail(string siteName, string siteAcronym, string siteNameShort, string baseURL, string primaryColor, User curUser);
        public Task<bool> SendContactUsEmail(string siteName, string baseURL, string primaryColor, string emailTo, string subject, string category, string description, string userName, string dateTime, string location, List<string> lstAttachments, List<string> unattachedFiles);
        public Task<bool> SendEmail(string toAddress, string subject, string htmlBody);
        public Task<bool> SendEmail(List<string> toAddresses, List<string> ccAddresses, List<string> bccAddresses, string subject, string htmlBody, List<string> attachmeents);        
        public Task<bool> UploadFileToS3(string fileName, MemoryStream memoryStream);
        public Task<bool> CheckS3FileTag(string fileName);
    }
}
