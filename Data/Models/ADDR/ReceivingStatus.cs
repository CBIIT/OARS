
using Amazon.DynamoDBv2.DataModel;
using Microsoft.AspNetCore.Hosting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheradexPortal.Data.Models.ADDR
{
    public class ReceivingStatus2 : ADDRBase
    {
        [DynamoDBProperty]
        public string? CarrierTrackingNo { get; set; }

        [DynamoDBProperty]
        public string? ShippedDate { get; set; }

        [DynamoDBProperty]
        public string? CarrierName { get; set; }
        [DynamoDBProperty]
        public string? PackagingCondition { get; set; }
        [DynamoDBProperty]
        public string? ReceivingSite { get; set; }
        [DynamoDBProperty]
        public string? ReceivedDateTime { get; set; }
        [DynamoDBProperty]
        public string? PackagingDetailCondition { get; set; }
        [DynamoDBProperty]
        public string? Inadreas { get; set; }
        [DynamoDBProperty]
        public string? Notes { get; set; }

        [DynamoDBProperty]
        public string? VariCount { get; set; }

        [DynamoDBProperty]
        public string? RaveCount { get; set; }

    }

    public class ReceivingStatus : ADDRBase
    {
        [DynamoDBProperty]
        public string? SubjectUrl { get; set; }

        [DynamoDBProperty]
        public string? ReceivingStatusUrl { get; set; }

        [DynamoDBProperty]
        public string? Datasource { get; set; }

        [DynamoDBProperty]
        public string? VariComments { get; set; }

        [DynamoDBProperty]
        public int? Varicount { get; set; }

        [DynamoDBProperty]
        public int? Ravecount { get; set; }

        [DynamoDBProperty]
        public string? Subjectkey { get; set; }

        [DynamoDBProperty]
        public string? Siteid { get; set; }

        [DynamoDBProperty]
        public string? SpecimenSponsorGroupSpecimenId { get; set; }

        [DynamoDBProperty]
        public string? RegPatientId { get; set; }

        [DynamoDBProperty]
        public string? SubmissionCarrierTrackingNo { get; set; }

        [DynamoDBProperty]
        public string? SubmissionCarrierName { get; set; }

        [DynamoDBProperty]
        public string? SubmissionConditionPackaging { get; set; }

        [DynamoDBProperty]
        public string? SubSpecimenId { get; set; }

        [DynamoDBProperty]
        public string? ReceivingSite { get; set; }

        [DynamoDBProperty]
        public string? ShippedDate { get; set; }

        [DynamoDBProperty]
        public string? SubmissionReceivedDatetime { get; set; }

        [DynamoDBProperty]
        public string? SubmissionConditionPackagingDetail { get; set; }

        [DynamoDBProperty]
        public string? Inadreas { get; set; }

        [DynamoDBProperty]
        public string? Comments { get; set; }

        [DynamoDBProperty]
        public string? Notes { get; set; }

        [DynamoDBProperty]
        public string? Action { get; set; }

        [DynamoDBProperty]
        public string? Studyeventrepeatkey { get; set; }

        [DynamoDBProperty]
        public int? Itemgrouprepeatkey { get; set; }

        [DynamoDBProperty]
        public string? Transactiontype { get; set; }

        [DynamoDBProperty]
        public int? Active { get; set; }
    }
}