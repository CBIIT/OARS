using Blazorise;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace TheradexPortal.Data.Models
{
    public class ETCTNUploadRequest
    {
        public Guid ID { get; set; }

        [Required]
        public string Protocol { get; set; }

        [Required]
        public string Laboratory { get; set; }

        [Required]
        public string CRF { get; set; }

        [Required]
        public string Assay { get; set; }

        //public UploadFileModel? UploadFile { get; set; }
    }

    public class UploadFileModel
    {
        public string OriginalFileName { get; set; }
        public string S3Key { get; set; }
    }

    public class MedidataDictionaryModel
    {
        public string UserData { get; set; }
        public string CodedData { get; set; }
    }

    public class CRFModel
    {
        public string FormOID { get; set; }
        public string FormName { get; set; }
    }

    public class FileMetadata
    {
        [JsonProperty("id")]
        public Guid ID { get; set; }

        [JsonProperty("userId")]
        public int UserId { get; set; }

        [JsonProperty("protocol")]
        public string Protocol { get; set; }

        [JsonProperty("laboratory")]
        public string Laboratory { get; set; }

        [JsonProperty("assay")]
        public string Assay { get; set; }

        [JsonProperty("crf")]
        public string CRF { get; set; }

        [JsonProperty("fileName")]
        public string FileName { get; set; }

        [JsonProperty("filePath")]
        public string FilePath { get; set; }
    }
}
