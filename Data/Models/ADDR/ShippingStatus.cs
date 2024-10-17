
using Amazon.DynamoDBv2.DataModel;
using Microsoft.AspNetCore.Hosting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheradexPortal.Data.Models.ADDR
{
    public class ShippingStatus : ADDRBase
    {
        [DynamoDBProperty]
        public string? CarrierTrackingNo { get; set; }

        [DynamoDBProperty]
        public string? ShippedDate { get; set; }


        [DynamoDBProperty]
        public string? TrackingNumber { get; set; }

        [DynamoDBProperty]
        public string? SpecimenReturnedToSourceSite { get; set; }

        [DynamoDBProperty]
        public string? NoticeSentName { get; set; }

        [DynamoDBProperty]
        public string? Source { get; set; }

        [DynamoDBProperty]
        public string? SubjectId { get; set; }

        [DynamoDBProperty]
        public string? SenderEmail { get; set; }

        [DynamoDBProperty]
        public string? Destination { get; set; }

        [DynamoDBProperty]
        public string? NumberOfSamples { get; set; }

        [DynamoDBProperty]
        public string? CourierName { get; set; }

        [DynamoDBProperty]
        public string? SenderPhone { get; set; }

        [DynamoDBProperty]
        public string? DataSnapshotDate { get; set; }

        [DynamoDBProperty]
        public string? DateModified { get; set; }

        [DynamoDBProperty]
        public string? NoticeSentTo { get; set; }

        [DynamoDBProperty]
        public string? ShippingConditions { get; set; }

        [DynamoDBProperty]
        public string? SenderName { get; set; }

        [DynamoDBProperty]
        public string? DataSnapshotTime { get; set; }

        [DynamoDBProperty]
        public string? VariCount { get; set; }

        [DynamoDBProperty]
        public string? RaveCount { get; set; }

    }
}

