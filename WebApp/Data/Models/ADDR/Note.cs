
using Amazon.DynamoDBv2.DataModel;
using Microsoft.AspNetCore.Hosting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography.Pkcs;

namespace OARS.Data.Models.ADDR
{
    //public class Note<T> : ADDRBase
    //{       
    //    [DynamoDBProperty]
    //    public int? UserId { get; set; }

    //    [DynamoDBProperty]
    //    public string? FirstName { get; set; }

    //    [DynamoDBProperty] 
    //    public string? LastName { get; set; }

    //    [DynamoDBProperty] 
    //    public string? EmailAddress { get; set; }

    //    [DynamoDBProperty]
    //    public DateTime CreateDate { get; set; }
    //}

    [DynamoDBTable("OARS-Addr-Notes")]
    public class AddrNotes<T>
    {
        [DynamoDBHashKey]
        public string Id { get; set; }

        [DynamoDBProperty]
        public string UserId { get; set; }

        [DynamoDBProperty]
        public string Protocol { get; set; }

        [DynamoDBProperty]
        public string DataSource { get; set; }

        [DynamoDBProperty]
        public string FormId { get; set; }

        [DynamoDBProperty]
        public string SubjectId { get; set; }

        [DynamoDBProperty]
        public string SpecimenId { get; set; }

        [DynamoDBProperty]
        public string SubSpecimentId { get; set; }

        [DynamoDBProperty]
        public string Priority { get; set; }

        [DynamoDBProperty]
        public string Notes { get; set; }

        [DynamoDBProperty]
        public DateTime CreatedOn { get; set; }

        [DynamoDBProperty]
        public string CreatedBy { get; set; }

        [DynamoDBProperty]
        public DateTime UpdatedOn { get; set; }

        [DynamoDBProperty]
        public string UpdatedBy { get; set; }

        [DynamoDBProperty]
        public bool IsActive { get; set; }

        [DynamoDBProperty]
        public bool IsDeleted { get; set; }

        [DynamoDBGlobalSecondaryIndexHashKey("SearchKey-index")]
        public string SearchKey { get; set; }

        public static string GenerateSearchKey(string dataSource, string study, string formId, string subjectId)
        {
            return $"{study}#{dataSource}#{formId}#{subjectId}";
        }
    }
}