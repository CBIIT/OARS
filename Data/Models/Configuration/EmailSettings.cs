namespace TheradexPortal.Data.Models.Configuration
{
    public class EmailSettings
    {
        public string AWSProfileName { get; set; }
        public string AWSBucketName { get; set; }
        public string FromEmail { get; set; }
        public string SupportEmail { get; set; }
    }
}
