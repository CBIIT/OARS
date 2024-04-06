using Blazorise;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Amazon.DynamoDBv2.DataModel;

namespace TheradexPortal.Data.Models
{
    public class ETCTNUploadRequest //: IValidatableObject
    {
        public string RequestId { get; set; }

        [Required]
        public string Protocol { get; set; }

        [Required]
        public string ReceivingSite { get; set; }

        [Required]
        public string SourceSite { get; set; }

        [Required]
        public string CRF { get; set; }

        //[RequiredIf(nameof(CRF), "RECEIVING_STATUS")]
        public string Assay { get; set; }
    }

    public class RequiredIfAttribute : ValidationAttribute
    {
        private readonly string _propertyName;
        private readonly object? _isValue;

        public RequiredIfAttribute(string propertyName, object? isValue)
        {
            _propertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
            _isValue = isValue;
        }

        public override string FormatErrorMessage(string name)
        {
            var errorMessage = $"{name} is required when {_propertyName} is {_isValue}";
            return ErrorMessage ?? errorMessage;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            ArgumentNullException.ThrowIfNull(validationContext);
            var property = validationContext.ObjectType.GetProperty(_propertyName);

            if (property == null)
            {
                throw new NotSupportedException($"Can't find {_propertyName} on searched type: {validationContext.ObjectType.Name}");
            }

            var requiredIfTypeActualValue = property.GetValue(validationContext.ObjectInstance);

            if (requiredIfTypeActualValue == null && _isValue != null)
            {
                return ValidationResult.Success;
            }

            if (requiredIfTypeActualValue == null || requiredIfTypeActualValue.Equals(_isValue))
            {
                return value == null
                    ? new ValidationResult(FormatErrorMessage(validationContext.DisplayName))
                    : ValidationResult.Success;
            }

            return ValidationResult.Success;
        }
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
        [JsonProperty("requestId")]
        public string RequestId { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("protocol")]
        public string Protocol { get; set; }

        [JsonProperty("sourceSite")]
        public string SourceSite { get; set; }

        [JsonProperty("receivingSite")]
        public string ReceivingSite { get; set; }

        [JsonProperty("assay")]
        public string Assay { get; set; }

        [JsonProperty("crf")]
        public string CRF { get; set; }

        [JsonProperty("filePath")]
        public string FilePath { get; set; }

        [JsonProperty("originalFileName")]
        public string OriginalFileName { get; set; }

        [JsonProperty("bucket")]
        public string Bucket { get; set; }
    }

    public class FileIngestRequest
    {
        [DynamoDBHashKey]
        public string RequestId { get; set; }

        [DynamoDBProperty]
        public string UserId { get; set; }

        [DynamoDBProperty]
        public string Protocol { get; set; }

        [DynamoDBProperty]
        public string MetadataFilePath { get; set; }

        [DynamoDBProperty]
        public string Bucket { get; set; }

        [DynamoDBProperty]
        public FileMetadata Metadata { get; set; }

        [DynamoDBProperty]
        public string Status { get; set; }

        [DynamoDBProperty]
        public string Error { get; set; }

        [DynamoDBProperty]
        public string ClientError { get; set; }

        [DynamoDBProperty]
        public int RetryCount { get; set; }

        [DynamoDBProperty]
        public DateTime CreatedDate { get; set; }

        [DynamoDBProperty]
        public DateTime UpdatedDate { get; set; }
    }

    [DynamoDBTable("ReceivingStatusFileData")]
    public class ReceivingStatusFileData : BaseFileData
    {
        [DynamoDBProperty(AttributeName = "RowData")]
        public ReceivingStatusRowData RowData { get; set; }
    }

    public class ReceivingStatusRowData
    {
        [DynamoDBProperty(AttributeName = "Protocol")]
        public string Protocol { get; set; }

        [DynamoDBProperty(AttributeName = "SubjectID")]
        public string SubjectID { get; set; }

        [DynamoDBProperty(AttributeName = "SpecimenID")]
        public string SpecimenId { get; set; }

        [DynamoDBProperty(AttributeName = "ShippedDate")]
        public string ShippedDate { get; set; }

        [DynamoDBProperty(AttributeName = "DateReceived")]
        public string DateReceived { get; set; }

        [DynamoDBProperty(AttributeName = "SubspecimenID")]
        public string SubspecimenID { get; set; }

        [DynamoDBProperty(AttributeName = "SubmissionIsAdequate")]
        public string SubmissionIsAdequate { get; set; }

        [DynamoDBProperty(AttributeName = "ExplanatoryReason")]
        public string ExplanatoryReason { get; set; }

        [DynamoDBProperty(AttributeName = "OtherReason")]
        public string OtherReason { get; set; }
    }

    public class BaseFileData
    {
        [DynamoDBHashKey]
        public string RequestItemId { get; set; }

        [DynamoDBProperty]
        public string RequestId { get; set; }

        [DynamoDBProperty(AttributeName = "RowNo")]
        public int RowNo { get; set; }

        [DynamoDBProperty]
        public DateTime CreatedDate { get; set; }

        [DynamoDBProperty]
        public DateTime UpdatedDate { get; set; }

        [DynamoDBProperty(AttributeName = "Status")]
        public string Status { get; set; }

        [DynamoDBProperty(AttributeName = "Error")]
        public string Error { get; set; }

        [DynamoDBProperty(AttributeName = "ClientError")]
        public string ClientError { get; set; }

        [DynamoDBProperty(AttributeName = "MedidataUpdateReference")]
        public string MedidataUpdateReference { get; set; }

        [DynamoDBProperty(AttributeName = "MessageId")]
        public string MessageId { get; set; }

        [DynamoDBProperty(AttributeName = "RetryCount")]
        public int RetryCount { get; set; }

        [DynamoDBIgnore]
        public bool IsSent { get; set; }

        [DynamoDBIgnore]
        public string ODMXml { get; set; }
    }
}