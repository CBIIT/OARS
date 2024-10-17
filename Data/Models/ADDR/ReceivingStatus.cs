
using Amazon.DynamoDBv2.DataModel;
using Microsoft.AspNetCore.Hosting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheradexPortal.Data.Models.ADDR
{
    public class ReceivingStatus : ADDRBase
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
}

