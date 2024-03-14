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

        [Required]
        public UploadFileModel UploadFile { get; set; }
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
}
