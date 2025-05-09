﻿using Amazon.SimpleEmail.Model;
using Amazon.SimpleEmail;
using System.Net.Mail;
using OARS.Data.Services;
using Microsoft.AspNetCore.Components;
using OARS.Data.Services.Abstract;
using OARS.Data.Models;
using Microsoft.Extensions.Options;
using OARS.Data.Models.Configuration;
using System.Drawing;
using System.Net.Http;
using OARS.Data.PowerBI.Models;
using MimeKit;
using Amazon.S3;
using static OARS.Data.Services.OktaUser;
using Amazon;
using Amazon.S3.Transfer;
using System.Configuration;
using Amazon.S3.Model;
using System.IO;
using Microsoft.PowerBI.Api.Models;
using System.Net.Mime;
using MimeKit.Utils;
using ITfoxtec.Identity.Saml2.Schemas;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Hosting;
using Blazorise.Extensions;
using Okta.Sdk.Client;

namespace OARS.Data.Services
{
    public class EmailService : IEmailService
    {
        private readonly IOptions<EmailSettings> emailSettings;
        private readonly ILogger<EmailService> logger;
        private readonly IWebHostEnvironment webHostEnvironment;

        public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger, IWebHostEnvironment webHostEnvironment)
        {
            this.emailSettings = emailSettings;
            this.logger = logger;
            this.webHostEnvironment = webHostEnvironment;
        }

        public async Task<bool> SendNewUserEmail(string siteName, string siteAcronym, string siteNameShort, string baseURL, string primaryColor, OARS.Data.Models.User curUser, string activationLink)
        {
            try
            {
                logger.LogInformation("*** Email - NewUserEmail - " + curUser.EmailAddress + " ***");
                using (var s3Client = new AmazonS3Client(RegionEndpoint.USEast1))
                {
                    // Get the template to email
                    var emailText = "";
                    // Get the template to email
                    string emailTemplate = "{0}/NewTheradexUser.txt";
                    if (curUser.IsCtepUser)
                    {
                        emailTemplate = "{0}/NewCTEPUser.txt";
                    }

                    GetObjectResponse response = await s3Client.GetObjectAsync(emailSettings.Value.AWSBucketName, string.Format(emailTemplate, emailSettings.Value.EmailTemplate));

                    using (Stream responseStream = response.ResponseStream)
                    {
                        MemoryStream templateStream = new MemoryStream();
                        responseStream.CopyTo(templateStream);
                        emailText = System.Text.Encoding.UTF8.GetString(templateStream.ToArray());


                        // Populate the specific fields
                        emailText = emailText.Replace("[[Color]]", primaryColor);
                        emailText = emailText.Replace("[[System]]", siteName);
                        emailText = emailText.Replace("[[SystemAcronym]]", siteAcronym);
                        emailText = emailText.Replace("[[SystemShortName]]", siteNameShort);
                        emailText = emailText.Replace("[[ActivationLink]]", activationLink);
                        emailText = emailText.Replace("[[URL]]", baseURL);
                        emailText = emailText.Replace("[[FullName]]", curUser.FirstName + " " + curUser.LastName);
                        emailText = emailText.Replace("[[Email]]", curUser.EmailAddress);
                        emailText = emailText.Replace("[[SupportEmail]]", emailSettings.Value.SupportEmail);

                        await SendEmail(curUser.EmailAddress, "Welcome " + curUser.FirstName + " " + curUser.LastName, emailText);

                        // Email it out
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogInformation("Email Error: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> SendNewSystemEmail(string siteName, string siteAcronym, string siteNameShort, string baseURL, string primaryColor, OARS.Data.Models.User curUser)
        {
            try
            {
                logger.LogInformation("*** Email - NewSystemEmail - " + curUser.EmailAddress + " ***");
                // Get the template to email
                using (var s3Client = new AmazonS3Client(RegionEndpoint.USEast1))
                {
                    // Get the template to email
                    var emailText = "";
                    string emailTemplate = "{0}/NewSystemTheradexUser.txt";
                    if (curUser.IsCtepUser)
                    {
                        emailTemplate = "{0}/NewCTEPUser.txt";
                    }
                    GetObjectResponse response = await s3Client.GetObjectAsync(emailSettings.Value.AWSBucketName, string.Format(emailTemplate, emailSettings.Value.EmailTemplate));

                    using (Stream responseStream = response.ResponseStream)
                    {
                        MemoryStream templateStream = new MemoryStream();
                        responseStream.CopyTo(templateStream);
                        emailText = System.Text.Encoding.UTF8.GetString(templateStream.ToArray());

                        // Populate the specific fields
                        emailText = emailText.Replace("[[Color]]", primaryColor);
                        emailText = emailText.Replace("[[System]]", siteName);
                        emailText = emailText.Replace("[[SystemAcronym]]", siteAcronym);
                        emailText = emailText.Replace("[[SystemShortName]]", siteNameShort);
                        emailText = emailText.Replace("[[URL]]", baseURL);
                        emailText = emailText.Replace("[[FullName]]", curUser.FirstName + " " + curUser.LastName);
                        emailText = emailText.Replace("[[Email]]", curUser.EmailAddress);
                        emailText = emailText.Replace("[[SupportEmail]]", emailSettings.Value.SupportEmail);

                        await SendEmail(curUser.EmailAddress, "Welcome " + curUser.FirstName + " " + curUser.LastName, emailText);

                        // Email it out
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogInformation("Email Error: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> SendContactUsEmail(string siteName, string baseURL, string primaryColor, string emailTo, string subject, string category
         , string description, string userName, string dateTime, string location, List<string> lstAttachments, List<string> unattachedFiles)
        {
            try
            {
                using (var s3Client = new AmazonS3Client(RegionEndpoint.USEast1))
                {
                    // Get the template to email
                    GetObjectResponse response = await s3Client.GetObjectAsync(emailSettings.Value.AWSBucketName, string.Format("{0}/SupportRequest.txt", emailSettings.Value.EmailTemplate));

                    using (Stream responseStream = response.ResponseStream)
                    {
                        MemoryStream templateStream = new MemoryStream();
                        responseStream.CopyTo(templateStream);
                        string emailText = System.Text.Encoding.UTF8.GetString(templateStream.ToArray());
                        // Populate the specific fields
                        emailText = emailText.Replace("[[Color]]", primaryColor);
                        emailText = emailText.Replace("[[System]]", siteName);
                        emailText = emailText.Replace("[[URL]]", baseURL);
                        emailText = emailText.Replace("[[Username]]", userName);
                        emailText = emailText.Replace("[[Subject]]", subject);
                        emailText = emailText.Replace("[[Category]]", category);
                        emailText = emailText.Replace("[[Description]]", description);
                        emailText = emailText.Replace("[[DateTime]]", dateTime);
                        emailText = emailText.Replace("[[Location]]", location);

                        if (unattachedFiles.Count > 0)
                        {
                            string fileList = "";
                            foreach (string fileName in unattachedFiles)
                            {
                                fileList = fileList + fileName + ", ";
                            }
                            fileList = fileList.TrimEnd();
                            fileList = fileList.TrimEnd(',');
                            emailText = emailText.Replace("[[UnattachedFiles]]", "<tr><td colspan='2' style='padding-top:10px'>The following file(s) could not be attached : " + fileList + "</td></tr>");
                        }
                        else
                            emailText = emailText.Replace("[[UnattachedFiles]]", "");

                        return await SendEmail(new List<string> { emailTo }, null, null, subject, emailText, lstAttachments);
                    }

                }
            }
            catch (Exception ex)
            {
                logger.LogInformation("Email Error: " + ex.Message);
                return false;
            }
        }

        public async Task<bool> SendReviewEmail(string senderFullName, string emailTo, string subject, string description, string color)
        {
            try
            {
                using (var s3Client = new AmazonS3Client(RegionEndpoint.USEast1))
                {
                    // Get the template to email
                    GetObjectResponse response = await s3Client.GetObjectAsync(emailSettings.Value.AWSBucketName, string.Format("{0}/PIMOReviewEmail.txt", emailSettings.Value.EmailTemplate));

                    using (Stream responseStream = response.ResponseStream)
                    {
                        MemoryStream templateStream = new MemoryStream();
                        responseStream.CopyTo(templateStream);
                        string emailText = System.Text.Encoding.UTF8.GetString(templateStream.ToArray());
                        // Populate the specific fields

                        emailText = emailText.Replace("[[Color]]", color);
                        emailText = emailText.Replace("[[Subject]]", subject);
                        emailText = emailText.Replace("[[Description]]", description);
                        emailText = emailText.Replace("[[Username]]", senderFullName);
                        emailText = emailText.Replace("[[DateTime]]", DateTime.Now.ToString());

                        return await SendEmail(emailTo, subject, emailText);
                    }

                }
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

        public async Task<bool> SendEmail(List<string> toAddresses, List<string> ccAddresses, List<string> bccAddresses, string subject, string htmlBody, 
            List<string> attachments)
        {
            bool emailSuccess = true;
            try
            {
                logger.LogInformation("*** Email - Send ***");
                using (var client = new AmazonSimpleEmailServiceClient(RegionEndpoint.USEast1))
                {
                    // Build the body with attachements
                    var bodyBuilder = new BodyBuilder();
                    using (var s3Client = new AmazonS3Client(RegionEndpoint.USEast1))
                    {
                        GetObjectResponse response = await s3Client.GetObjectAsync(emailSettings.Value.AWSBucketName, string.Format("{0}/OarsLogo.svg", emailSettings.Value.EmailTemplate));
                        using (Stream responseStream = response.ResponseStream)
                        {
                            MemoryStream templateStream = new MemoryStream();
                            responseStream.CopyTo(templateStream);
                            var imageBytes = templateStream.ToArray();
                            var image = bodyBuilder.LinkedResources.Add("OarsLogo.svg", imageBytes);
                            image.ContentId = MimeUtils.GenerateMessageId();
                            bodyBuilder.HtmlBody = htmlBody.Replace("[[LogoContentId]]", image.ContentId);
                        }                        
                    }
                    try
                    {
                        var s3Client = new AmazonS3Client(RegionEndpoint.USEast1);
                        if (attachments != null && attachments.Count > 0)
                        {
                            foreach (string attachment in attachments)
                            {
                                var s3Object = await s3Client.GetObjectAsync(emailSettings.Value.AWSBucketName, string.Format("{0}/{1}", emailSettings.Value.UploadFolder, attachment));
                                if (s3Object != null)
                                {
                                    bodyBuilder.Attachments.Add(attachment, s3Object.ResponseStream);
                                }
                            }
                        }
                    }
                    catch (Exception AttachmentEx)
                    {
                        logger.LogInformation("Email Attachment Error : " + AttachmentEx.Message);
                        return false;
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
                    else
                    {
                        foreach (string bccAddress in emailSettings.Value.BccAddresses)
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

        private async Task<string> GetEmailTemplate(string templateName)
        {
            string emailText = "";
            try
            {
                using (var s3Client = new AmazonS3Client(RegionEndpoint.USEast1))
                {
                    var s3Object = await s3Client.GetObjectAsync(emailSettings.Value.AWSBucketName, "EmailTemplates/" + templateName);
                    StreamReader stream = new StreamReader(s3Object.ResponseStream);
                    emailText = stream.ReadToEnd();

                    return emailText;
                }
            }
            catch (Exception ex)
            {
                logger.LogInformation("Email Error: Error retrivieng Email Template - " + templateName);
                logger.LogInformation("Email Error: " + ex.Message);
                return "";
            }
        }

        public async Task<bool> UploadFileToS3(string fileName, MemoryStream memoryStream)
        {
            try
            {
                string fileKey = string.Format("{0}/{1}", emailSettings.Value.UploadFolder, fileName);
                string bucket = emailSettings.Value.AWSBucketName;
                using (var client = new AmazonS3Client(RegionEndpoint.USEast1))
                {
                    var uploadRequest = new TransferUtilityUploadRequest
                    {
                        InputStream = memoryStream,
                        Key = fileKey,
                        BucketName = bucket
                    };

                    var fileTransferUtility = new TransferUtility(client);
                    await fileTransferUtility.UploadAsync(uploadRequest);
                    return true;
                }
            }
            catch (Exception ex) {
                logger.LogInformation("Email Attachment Error - Uploading to S3 : " + ex.Message);
                return false;
            }
        }

        public async Task<bool> CheckS3FileTag(string fileName)
        {
            string fileKey = string.Format("{0}/{1}", emailSettings.Value.UploadFolder, fileName);
            string bucket = emailSettings.Value.AWSBucketName;
           
            //check clean tag for the file
            try
            {
                var s3Client = new AmazonS3Client(RegionEndpoint.USEast1);
                var s3Object = await s3Client.GetObjectAsync(bucket, fileKey);
                if (s3Object != null)
                {
                    /*
                    GetObjectTaggingRequest taggingRequest = new GetObjectTaggingRequest();
                    taggingRequest.BucketName = bucket;
                    taggingRequest.Key = fileKey;
                    GetObjectTaggingResponse taggingResponse = await s3Client.GetObjectTaggingAsync(taggingRequest);

                    List<Tag> tagging = taggingResponse.Tagging;
                    bool cleanTag = false;
                    foreach (Tag tagItem in tagging)
                    {
                        if (tagItem.Key == "scan-result" && tagItem.Value == "Clean")
                        {
                            cleanTag = true;
                        }
                    }
                    if (!cleanTag)
                    {
                        errorMessage = "The attachment file doesn't pass the anti virus scanning.";
                    }
                    */
                    return true;
                }
                else
                {
                    logger.LogInformation("Email Attachment Error - : S3 File tag failed");
                    return false;
                }
            }
            catch (Exception AttachmentEx)
            {
                logger.LogInformation("Email Attachment Error : " + AttachmentEx.Message);
                return false;
            }
        }
    }

}
