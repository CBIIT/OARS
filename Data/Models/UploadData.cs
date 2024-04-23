using Blazorise;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Amazon.DynamoDBv2.DataModel;
using System.ComponentModel;
using Microsoft.PowerBI.Api.Models;

namespace TheradexPortal.Data.Models
{
    public class ETCTNUploadRequest
    {
        public string RequestId { get; set; }

        [Required(ErrorMessage = "Protocol is required")]
        public string Protocol { get; set; }

        [Required(ErrorMessage = "Receiving Site is required")]
        public string ReceivingSite { get; set; }

        [Required(ErrorMessage = "Source Site is required")]
        public string SourceSite { get; set; }

        [Required(ErrorMessage = "CRF is required")]
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

    public class ProtocolData
    {
        public string Protocol { get; set; }
        public List<MedidataDictionaryModel> Sites { get; set; }
        public List<MedidataDictionaryModel> Assays { get; set; }
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

        [JsonProperty("environment")]
        public string Environment { get; set; }

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
        public int Status { get; set; }

        public string InternalStatus { get; set; }

        [DynamoDBProperty]
        public string Error { get; set; }

        [DynamoDBProperty]
        public string ClientError { get; set; }

        public string ClientStatus { get; set; }

        [DynamoDBProperty]
        public int RetryCount { get; set; }

        [DynamoDBProperty]
        public DateTime CreatedDate { get; set; }

        [DynamoDBProperty]
        public DateTime UpdatedDate { get; set; }
    }

    //101 to 110 Received Statuses
    //111 to 120 InProgress Statuses
    //121 to 130 Final Statuses
    public enum RequestStatusV2
    {
        [Description("Received")]
        Received = 101,

        [Description("InProgress")]
        InProgress = 111,

        [Description("Validating")]
        Validating = 112,

        [Description("Validated")]
        Validated = 113,

        [Description("CreatedMedidataODM")]
        CreatedMedidataODM = 116,

        [Description("Retry")]
        Retry = 118,

        [Description("Success")]
        Success = 121,

        [Description("PartialSuccess")]
        PartialSuccess = 122,

        [Description("Failed")]
        Failed = 125
    }


    //201 to 210 Received Statuses
    //211 to 220 InProgress Statuses
    //221 to 230 Final Statuses
    public enum RequestItemStatusV2
    {
        [Description("Received")]
        Received = 201,

        [Description("InProgress")]
        InProgress = 211,

        [Description("Validating")]
        Validating = 212,

        [Description("Validated")]
        Validated = 213,

        [Description("CreatedMedidataODM")]
        CreatedMedidataODM = 216,

        [Description("Retry")]
        Retry = 218,

        [Description("Success")]
        Success = 221,

        [Description("Failed")]
        Failed = 225
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
        public int Status { get; set; }

        public string InternalStatus { get; set; }

        [DynamoDBProperty(AttributeName = "Error")]
        public string Error { get; set; }

        [DynamoDBProperty(AttributeName = "ClientError")]
        public string ClientError { get; set; }

        public string ClientStatus { get; set; }

        [DynamoDBProperty(AttributeName = "MedidataUpdateReference")]
        public string MedidataUpdateReference { get; set; }

        [DynamoDBProperty(AttributeName = "MessageId")]
        public string MessageId { get; set; }

        [DynamoDBProperty(AttributeName = "RetryCount")]
        public int RetryCount { get; set; }
    }
}