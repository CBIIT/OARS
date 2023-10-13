using Amazon.SimpleEmail.Model;
using Amazon.SimpleEmail;
using System.Net.Mail;
using TheradexPortal.Data.Services;
using Microsoft.AspNetCore.Components;
using TheradexPortal.Data.Services.Abstract;
using TheradexPortal.Data.Models;
using Microsoft.Extensions.Options;
using TheradexPortal.Data.Models.Configuration;
using System.Drawing;
using System.Net.Http;
using TheradexPortal.Data.PowerBI.Models;
using MimeKit;
using Amazon.S3;
using static TheradexPortal.Data.Services.OktaUser;
using Amazon;

namespace TheradexPortal.Data.Services
{
    public class EmailService : IEmailService
    {
        private readonly IOptions<EmailSettings> emailSettings;
        private readonly ILogger<EmailService> logger;

        public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
        {
            this.emailSettings = emailSettings;
            this.logger = logger;
        }

        public async Task<bool> SendNewUserEmail(string siteName, string baseURL, string primaryColor, User curUser, string activationLink)
        {
            try
            {
                logger.LogInformation("*** Email - NewUserEmail - " + curUser.EmailAddress + " ***");
                // Get the template to email
                var emailText = "";
                if (!curUser.IsCtepUser)
                {
                    emailText = System.IO.File.ReadAllText($"{System.IO.Directory.GetCurrentDirectory()}{@"\wwwroot\emails\NewTheradexUser.txt"}");
                }
                else
                    emailText = System.IO.File.ReadAllText($"{System.IO.Directory.GetCurrentDirectory()}{@"\wwwroot\emails\NewCTEPUser.txt"}");

                // Populate the specific fields
                emailText = emailText.Replace("[[Color]]", primaryColor);
                emailText = emailText.Replace("[[System]]", siteName);
                emailText = emailText.Replace("[[ActivationLink]]", activationLink);
                emailText = emailText.Replace("[[URL]]", baseURL);
                emailText = emailText.Replace("[[FullName]]", curUser.FirstName + " " + curUser.LastName);
                emailText = emailText.Replace("[[Email]]", curUser.EmailAddress);
                emailText = emailText.Replace("[[SupportEmail]]", emailSettings.Value.SupportEmail);

                await SendEmail(curUser.EmailAddress, "Welcome " + curUser.FirstName + " " + curUser.LastName, emailText);

                return true;
            }
            catch (Exception ex)
            {
                logger.LogInformation("Email Error: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> SendNewSystemEmail(string siteName, string baseURL, string primaryColor, User curUser)
        {
            try
            {
                logger.LogInformation("*** Email - NewSystemEmail - " + curUser.EmailAddress + " ***");
                // Get the template to email
                var emailText = "";
                if (!curUser.IsCtepUser)
                {
                    emailText = System.IO.File.ReadAllText($"{System.IO.Directory.GetCurrentDirectory()}{@"\wwwroot\emails\NewSystemTheradexUser.txt"}");
                }
                else
                    emailText = System.IO.File.ReadAllText($"{System.IO.Directory.GetCurrentDirectory()}{@"\wwwroot\emails\NewCTEPUser.txt"}");

                // Populate the specific fields
                emailText = emailText.Replace("[[Color]]", primaryColor);
                emailText = emailText.Replace("[[System]]", siteName);
                emailText = emailText.Replace("[[URL]]", baseURL);
                emailText = emailText.Replace("[[FullName]]", curUser.FirstName + " " + curUser.LastName);
                emailText = emailText.Replace("[[Email]]", curUser.EmailAddress);
                emailText = emailText.Replace("[[SupportEmail]]", emailSettings.Value.SupportEmail);

                await SendEmail(curUser.EmailAddress, "Welcome " + curUser.FirstName + " " + curUser.LastName, emailText);

                return true;
            }
            catch (Exception ex)
            {
                logger.LogInformation("Email Error: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> SendEmail(string toAddress, string subject, string htmlBody)
        {
            logger.LogInformation("*** Email - Unspecified - " + toAddress + " ***");
            return await SendEmail(new List<String> { toAddress }, null, null, subject, htmlBody, null);
        }

        public async Task<bool> SendEmail(List<string> toAddresses, List<string> ccAddresses, List<string> bccAddresses, string subject, string htmlBody, List<string> attachments)
        {
            bool emailSuccess = true;
            try
            {
                logger.LogInformation("*** Email - Send ***");
                using (var client = new AmazonSimpleEmailServiceClient(RegionEndpoint.USEast1))
                {
                    // Build the body with attachements
                    var bodyBuilder = new BodyBuilder();
                    bodyBuilder.HtmlBody = htmlBody;

                    using (var s3Client = new AmazonS3Client(RegionEndpoint.USEast1))
                    {
                        if (attachments != null)
                        {
                            foreach (string attachment in attachments)
                            {
                                //var s3Object = await s3Client.GetObjectAsync("theradex-nci-webreporting-dev", attachment);
                                var s3Object = await s3Client.GetObjectAsync(emailSettings.Value.AWSBucketName, attachment);
                                bodyBuilder.Attachments.Add(attachment, s3Object.ResponseStream);
                            }
                        }
                    }

                    // Build the mime message with the from, to, cc and bcc addresses (if applicable)
                    var mimeMessage = new MimeMessage();
                    mimeMessage.From.Add(new MailboxAddress(string.Empty, emailSettings.Value.FromEmail));

                    if (toAddresses != null)
                    {
                        foreach (string toAddress in toAddresses)
                            mimeMessage.To.Add(new MailboxAddress(string.Empty, toAddress));
                    }
                    if (ccAddresses != null)
                    {
                        foreach (string ccAddress in ccAddresses)
                            mimeMessage.Cc.Add(new MailboxAddress(string.Empty, ccAddress));
                    }
                    if (bccAddresses != null)
                    {
                        foreach (string bccAddress in bccAddresses)
                            mimeMessage.Bcc.Add(new MailboxAddress(string.Empty, bccAddress));
                    }

                    mimeMessage.Subject = subject;
                    mimeMessage.Body = bodyBuilder.ToMessageBody();

                    using (var messageStream = new MemoryStream())
                    {
                        await mimeMessage.WriteToAsync(messageStream);
                        var sendRequest = new SendRawEmailRequest { RawMessage = new RawMessage(messageStream) };
                        var response = await client.SendRawEmailAsync(sendRequest);
                    }
                }

                logger.LogInformation("*** Email - Success ***");
                return true;
            }
            catch (Exception ex)
            {
                logger.LogInformation("Email Error: " + ex.Message);
                return false;
            }
        }
    }
}
