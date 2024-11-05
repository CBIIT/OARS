
using Amazon.DynamoDBv2.DataModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheradexPortal.Data.Models.ADDR
{
    public class ADDRBase
    {
        [DynamoDBHashKey]
        public string Id { get; set; }
        [DynamoDBProperty]
        public string? ProtocolNumber { get; set; }
        [DynamoDBProperty]
        public string? DataSource { get; set; }

        [DynamoDBProperty]
        public string? SystemComments { get; set; }

        [DynamoDBProperty]
        public string? SubjectKey { get; set; }

        [DynamoDBProperty]
        public string? SiteId { get; set; }

        [DynamoDBProperty]
        public string? SpecimenId { get; set; }

        [DynamoDBProperty]
        public string? SubSpecimenId { get; set; }

        [DynamoDBProperty]
        public string? RegisteredPatientId { get; set; }

        [DynamoDBProperty]
        public string? Comments { get; set; }

        [DynamoDBProperty]
        public string? Action { get; set; }

        [DynamoDBProperty]
        public string? StudyEventRepeatKey { get; set; }

        [DynamoDBProperty]
        public string? ItemGroupRepeatKey { get; set; }

        [DynamoDBProperty]
        public string? TransactionType { get; set; }

        [DynamoDBProperty]
        public string? Active { get; set; }
    }

}

