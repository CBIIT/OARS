namespace OARS.Data.Models.Configuration
{
    public class UploadSettings
    {
        public string AWSBucketName { get; set; }
        public string FilesUploadPath { get; set; }
        public string MetadataUploadPath { get; set; }

        public string TemplatesPath { get; set; }

        public string ProtocolDataPath { get; set; }

        public string CRFRulesPath { get; set; }
    }
}