
using Amazon.DynamoDBv2.DataModel;
using Microsoft.AspNetCore.Hosting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OARS.Data.Models.ADDR
{
    public class ReceivingStatus : ADDRBase
    {
        [DynamoDBProperty]
        public string? ProtocolNumber { get; set; }
        [DynamoDBProperty]
        public string? StudyId { get; set; }
        [DynamoDBProperty]
        public string? SubjectUrl { get; set; }

        [DynamoDBProperty]
        public string? ReceivingStatusUrl { get; set; }

        [DynamoDBProperty]
        public string? VariComments { get; set; }

        [DynamoDBProperty]
        public int? FeedCount { get; set; }

        [DynamoDBProperty]
        public int? RaveCount { get; set; }

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
        public string? Notes { get; set; }

    }
}