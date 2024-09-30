namespace TheradexPortal.Data.Models.Configuration
{
    public class EmailSettings
    {
        public string AWSProfileName { get; set; }
        public string AWSBucketName { get; set; }
        public string EmailTemplate { get; set; }
        public string UploadFolder { get; set; }
        public string FromEmail { get; set; }
        public string SupportEmail { get; set; }
        public List<string> BccAddresses { get; set; }
        public EmailSettings()
        {
            BccAddresses = new List<string>();
        }
    }
}
