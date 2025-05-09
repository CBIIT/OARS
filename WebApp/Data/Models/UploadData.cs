﻿using Blazorise;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Amazon.DynamoDBv2.DataModel;
using System.ComponentModel;
using Microsoft.PowerBI.Api.Models;
using System.Linq;

namespace OARS.Data.Models
{
    public class ETCTNUploadRequest
    {
        public string RequestId { get; set; }

        [Required(ErrorMessage = "Protocol is required")]
        public string Protocol { get; set; }

        //[Required(ErrorMessage = "Receiving Site is required")]
        [RequiredIf(nameof(CRF), "RECEIVING_STATUS,SHIPPING_STATUS")]
        public string ReceivingSite { get; set; }

        [Required(ErrorMessage = "Source Site is required")]
        public string SourceSite { get; set; }

        [Required(ErrorMessage = "CRF is required")]
        public string CRF { get; set; }

        [RequiredIf(nameof(CRF), "BIOSPECIMEN_ROADMAP_ASSAY")]
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

        //public override string FormatErrorMessage(string name)
        //{
        //    var errorMessage = $"{name} is required when {_propertyName} is {_isValue}";
        //    return ErrorMessage ?? errorMessage;
        //}

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

            //if (requiredIfTypeActualValue == null || requiredIfTypeActualValue.Equals(_isValue))
            if (requiredIfTypeActualValue == null || _isValue.ToString().Contains(requiredIfTypeActualValue.ToString()))
            {
                return value == null
                    //? new ValidationResult(FormatErrorMessage(validationContext.DisplayName))
                    ? new ValidationResult(null)
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

        public string Environment { get; set; }
        public List<MedidataDictionaryModel> Sites { get; set; }
        public List<MedidataDictionaryModel> Assays { get; set; }
    }

    public class CRFRule
    {
        public string CRF { get; set; }
        public List<CRFRuleInfo> Rules { get; set; }
    }

    public class CRFRuleInfo
    {
        public string Column { get; set; }

        public string Description { get; set; }
    }

    public class CRFModel
    {
        public string FormOID { get; set; }
        public string FormName { get; set; }
    }

    public class UploadConfiguration
    {
        public List<ProtocolData> ProtocolData { get; set; }

        public List<CRFRule> CRFRules { get; set; }
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

        [JsonProperty("crfDescription")]
        public string CRFDescription { get; set; }
    }

    public class FileIngestRequest
    {
        [DynamoDBHashKey]
        public string RequestId { get; set; }

        [DynamoDBProperty]
        public string UserId { get; set; }

        [DynamoDBProperty]
        public string Environment { get; set; }

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
        public string SpecimenID { get; set; }

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

    [DynamoDBTable("BiospecimenRoadmapFileData")]
    public class BiospecimenRoadmapFileData : BaseFileData
    {
        [DynamoDBProperty(AttributeName = "RowData")]
        public BiospecimenRoadmapRowData RowData { get; set; }
    }

    public class BiospecimenRoadmapRowData
    {
        [DynamoDBProperty(AttributeName = "Protocol")]
        public string Protocol { get; set; }

        [DynamoDBProperty(AttributeName = "SubspecimenID")]
        public string SubspecimenID { get; set; }

        [DynamoDBProperty(AttributeName = "Outcome")]
        public string Outcome { get; set; }

        [DynamoDBProperty(AttributeName = "Details")]
        public string Details { get; set; }

        [DynamoDBProperty(AttributeName = "Date")]
        public string Date { get; set; }

        [DynamoDBProperty(AttributeName = "AssayVersion")]
        public string AssayVersion { get; set; }

        [DynamoDBProperty(AttributeName = "Active")]
        public string Active { get; set; }
    }

    [DynamoDBTable("ShippingStatusFileData")]
    public class ShippingStatusFileData : BaseFileData
    {
        [DynamoDBProperty(AttributeName = "RowData")]
        public ShippingStatusRowData RowData { get; set; }
    }

    public class ShippingStatusRowData
    {
        [DynamoDBProperty(AttributeName = "Protocol")]
        public string Protocol { get; set; }

        [DynamoDBProperty(AttributeName = "SubjectID")]
        public string SubjectID { get; set; }

        [DynamoDBProperty(AttributeName = "SpecimenID")]
        public string SpecimenID { get; set; }

        [DynamoDBProperty(AttributeName = "SubspecimenID")]
        public string SubspecimenID { get; set; }

        [DynamoDBProperty(AttributeName = "CourierName")]
        public string CourierName { get; set; }

        [DynamoDBProperty(AttributeName = "ShippingTrackingNumber")]
        public string ShippingTrackingNumber { get; set; }

        [DynamoDBProperty(AttributeName = "SendersName")]
        public string SendersName { get; set; }

        [DynamoDBProperty(AttributeName = "SendersTelephoneNumber")]
        public string SendersTelephoneNumber { get; set; }

        [DynamoDBProperty(AttributeName = "SendersEmailAddress")]
        public string SendersEmailAddress { get; set; }

        [DynamoDBProperty(AttributeName = "NumberOfSamplesSent")]
        public string NumberOfSamplesSent { get; set; }

        [DynamoDBProperty(AttributeName = "ShippingConditions")]
        public string ShippingConditions { get; set; }

        [DynamoDBProperty(AttributeName = "ShippedDate")]
        public string ShippedDate { get; set; }

        [DynamoDBProperty(AttributeName = "RecipientsName")]
        public string RecipientsName { get; set; }

        [DynamoDBProperty(AttributeName = "RecipientsEmailAddress")]
        public string RecipientsEmailAddress { get; set; }
    }

    [DynamoDBTable("TSO500SequencingQCFileData")]
    public class TSO500SequencingQCFileData : BaseFileData
    {
        [DynamoDBProperty(AttributeName = "RowData")]
        public TSO500SequencingQCRowData RowData { get; set; }
    }

    public class TSO500SequencingQCRowData
    {
        [DynamoDBProperty(AttributeName = "Protocol")]
        public string Protocol { get; set; }

        [DynamoDBProperty(AttributeName = "SubspecimenID")]
        public string SubspecimenID { get; set; }

        [DynamoDBProperty(AttributeName = "RunID")]
        public string RunID { get; set; }

        [DynamoDBProperty(AttributeName = "SequencingID")]
        public string SequencingID { get; set; }

        [DynamoDBProperty(AttributeName = "ContaminationScore")]
        public string ContaminationScore { get; set; }

        [DynamoDBProperty(AttributeName = "ContaminationLevel")]
        public string ContaminationLevel { get; set; }

        [DynamoDBProperty(AttributeName = "MeanFamilyDepth")]
        public string MeanFamilyDepth { get; set; }

        [DynamoDBProperty(AttributeName = "PassFilterReads")]
        public string PassFilterReads { get; set; }

        [DynamoDBProperty(AttributeName = "noiseAF")]
        public string noiseAF { get; set; }

        [DynamoDBProperty(AttributeName = "MAD")]
        public string MAD { get; set; }

        [DynamoDBProperty(AttributeName = "MedianInsertSize")]
        public string MedianInsertSize { get; set; }

        [DynamoDBProperty(AttributeName = "Uniformity20Percent")]
        public string Uniformity20Percent { get; set; }

        [DynamoDBProperty(AttributeName = "PercentExonGreaterthan500X")]
        public string PercentExonGreaterthan500X { get; set; }

        [DynamoDBProperty(AttributeName = "MedianExonCoverage")]
        public string MedianExonCoverage { get; set; }

        [DynamoDBProperty(AttributeName = "TumorMutationBurden")]
        public string TumorMutationBurden { get; set; }

        [DynamoDBProperty(AttributeName = "UsableMSISites_QC")]
        public string UsableMSISites_QC { get; set; }

        [DynamoDBProperty(AttributeName = "PercentMSISitesUnstable")]
        public string PercentMSISitesUnstable { get; set; }

        [DynamoDBProperty(AttributeName = "MedianBinCount")]
        public string MedianBinCount { get; set; }

        [DynamoDBProperty(AttributeName = "QCResult")]
        public string QCResult { get; set; }

        [DynamoDBProperty(AttributeName = "Comments")]
        public string Comments { get; set; }

        [DynamoDBProperty(AttributeName = "Input_ng")]
        public string Input_ng { get; set; }

        [DynamoDBProperty(AttributeName = "PercentcfDNA")]
        public string PercentcfDNA { get; set; }

        [DynamoDBProperty(AttributeName = "Active")]
        public string Active { get; set; }

        [DynamoDBProperty(AttributeName = "SpecimenID")]
        public string SpecimenID { get; set; }

        [DynamoDBProperty(AttributeName = "SubjectKey")]
        public string SubjectKey { get; set; }

        [DynamoDBProperty(AttributeName = "StudyEventRepeatKey")]
        public string StudyEventRepeatKey { get; set; }
    }

    [DynamoDBTable("TSO500LibraryQCFileData")]
    public class TSO500LibraryQCFileData : BaseFileData
    {
        [DynamoDBProperty(AttributeName = "RowData")]
        public TSO500LibraryQCRowData RowData { get; set; }
    }

    public class TSO500LibraryQCRowData
    {
        [DynamoDBProperty(AttributeName = "Protocol")]
        public string Protocol { get; set; }

        [DynamoDBProperty(AttributeName = "LibraryStartDate")]
        public string LibraryStartDate { get; set; }

        [DynamoDBProperty(AttributeName = "SubspecimenID")]
        public string SubspecimenID { get; set; }

        [DynamoDBProperty(AttributeName = "LibraryID")]
        public string LibraryID { get; set; }

        [DynamoDBProperty(AttributeName = "AverageSize_bp")]
        public string AverageSize_bp { get; set; }

        [DynamoDBProperty(AttributeName = "QCResult")]
        public string QCResult { get; set; }

        [DynamoDBProperty(AttributeName = "LibraryConc_ngul")]
        public string LibraryConc_ngul { get; set; }

        [DynamoDBProperty(AttributeName = "Active")]
        public string Active { get; set; }

        [DynamoDBProperty(AttributeName = "SpecimenID")]
        public string SpecimenID { get; set; }

        [DynamoDBProperty(AttributeName = "SubjectKey")]
        public string SubjectKey { get; set; }

        [DynamoDBProperty(AttributeName = "StudyEventRepeatKey")]
        public string StudyEventRepeatKey { get; set; }
    }


    [DynamoDBTable("IFAFileData")]
    public class IFAFileData : BaseFileData
    {
        [DynamoDBProperty(AttributeName = "RowData")]
        public IFARowData RowData { get; set; }
    }

    public class IFARowData
    {
        [DynamoDBProperty(AttributeName = "Protocol")]
        public string Protocol { get; set; }

        [DynamoDBProperty(AttributeName = "SubjectID")]
        public string SubjectID { get; set; }

        [DynamoDBProperty(AttributeName = "Date")]
        public string Date { get; set; }

        [DynamoDBProperty(AttributeName = "LabTestName")]
        public string LabTestName { get; set; }

        [DynamoDBProperty(AttributeName = "ResultType")]
        public string ResultType { get; set; }

        [DynamoDBProperty(AttributeName = "TheradexSpecimenID")]
        public string TheradexSpecimenID { get; set; }

        [DynamoDBProperty(AttributeName = "PADISSpecimenID")]
        public string PADISSpecimenID { get; set; }

        [DynamoDBProperty(AttributeName = "SubspecimenID")]
        public string SubspecimenID { get; set; }

        [DynamoDBProperty(AttributeName = "SlideNo")]
        public string SlideNo { get; set; }

        [DynamoDBProperty(AttributeName = "AssessmentTimepoint")]
        public string AssessmentTimepoint { get; set; }

        [DynamoDBProperty(AttributeName = "PassNo")]
        public string PassNo { get; set; }

        [DynamoDBProperty(AttributeName = "ROINo")]
        public string ROINo { get; set; }

        [DynamoDBProperty(AttributeName = "Result")]
        public string Result { get; set; }

        [DynamoDBProperty(AttributeName = "NucleiNumber")]
        public string NucleiNumber { get; set; }

        [DynamoDBProperty(AttributeName = "OperatorName")]
        public string OperatorName { get; set; }

        [DynamoDBProperty(AttributeName = "RunDataFileName")]
        public string RunDataFileName { get; set; }

        [DynamoDBProperty(AttributeName = "Active")]
        public string Active { get; set; }
    }

    [DynamoDBTable("IFAResultSummaryFileData")]
    public class IFAResultSummaryFileData : BaseFileData
    {
        [DynamoDBProperty(AttributeName = "RowData")]
        public IFAResultSummaryRowData RowData { get; set; }
    }

    public class IFAResultSummaryRowData
    {
        [DynamoDBProperty(AttributeName = "Protocol")]
        public string Protocol { get; set; }

        [DynamoDBProperty(AttributeName = "SubjectID")]
        public string SubjectID { get; set; }

        [DynamoDBProperty(AttributeName = "LabTestName")]
        public string LabTestName { get; set; }

        [DynamoDBProperty(AttributeName = "TheradexSpecimenID")]
        public string TheradexSpecimenID { get; set; }

        [DynamoDBProperty(AttributeName = "PADISSpecimenID")]
        public string PADISSpecimenID { get; set; }

        [DynamoDBProperty(AttributeName = "SubspecimenID")]
        public string SubspecimenID { get; set; }

        [DynamoDBProperty(AttributeName = "PassNo")]
        public string PassNo { get; set; }

        [DynamoDBProperty(AttributeName = "AssessmentTimepoint")]
        public string AssessmentTimepoint { get; set; }

        [DynamoDBProperty(AttributeName = "ResultType")]
        public string ResultType { get; set; }

        [DynamoDBProperty(AttributeName = "Result")]
        public string Result { get; set; }

        [DynamoDBProperty(AttributeName = "StandardDeviation")]
        public string StandardDeviation { get; set; }

        [DynamoDBProperty(AttributeName = "TotalNuclei")]
        public string TotalNuclei { get; set; }

        [DynamoDBProperty(AttributeName = "OperatorName")]
        public string OperatorName { get; set; }

        [DynamoDBProperty(AttributeName = "RunDataFileName")]
        public string RunDataFileName { get; set; }

        [DynamoDBProperty(AttributeName = "Active")]
        public string Active { get; set; }
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

    [DynamoDBTable("PathologyEvaluationReportFileData")]
    public class PathologyEvaluationReportFileData : BaseFileData
    {
        [DynamoDBProperty(AttributeName = "RowData")]
        public PathologyEvaluationReportRowData RowData { get; set; }
    }

    public class PathologyEvaluationReportRowData
    {
        [DynamoDBProperty(AttributeName = "Protocol")]
        public string Protocol { get; set; }

        [DynamoDBProperty(AttributeName = "SubjectID")]
        public string SubjectID { get; set; }

        [DynamoDBProperty(AttributeName = "BlockID")]
        public string BlockID { get; set; }

        [DynamoDBProperty(AttributeName = "TheradexSpecimenID")]
        public string TheradexSpecimenID { get; set; }

        [DynamoDBProperty(AttributeName = "PADISSpecimenID")]
        public string PADISSpecimenID { get; set; }

        [DynamoDBProperty(AttributeName = "SubspecimenID")]
        public string SubspecimenID { get; set; }

        [DynamoDBProperty(AttributeName = "AssessmentTimepoint")]
        public string AssessmentTimepoint { get; set; }

        [DynamoDBProperty(AttributeName = "PassNo")]
        public string PassNo { get; set; }

        [DynamoDBProperty(AttributeName = "SlideNo")]
        public string SlideNo { get; set; }

        [DynamoDBProperty(AttributeName = "TumorPercentage")]
        public string TumorPercentage { get; set; }

        [DynamoDBProperty(AttributeName = "StromaPercentage")]
        public string StromaPercentage { get; set; }

        [DynamoDBProperty(AttributeName = "NormalPercentage")]
        public string NormalPercentage { get; set; }

        [DynamoDBProperty(AttributeName = "NecrosisPercentage")]
        public string NecrosisPercentage { get; set; }

        [DynamoDBProperty(AttributeName = "OtherFindings")]
        public string OtherFindings { get; set; }

        [DynamoDBProperty(AttributeName = "Analyzable")]
        public string Analyzable { get; set; }

        [DynamoDBProperty(AttributeName = "Active")]
        public string Active { get; set; }
    }
}