
using Amazon.DynamoDBv2.DataModel;
using Microsoft.AspNetCore.Hosting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheradexPortal.Data.Models.ADDR
{
    public class Note<T> : ADDRBase
    {
        [DynamoDBProperty]
        public string Text { get; set; }
       
        [DynamoDBProperty]
        public int? UserId { get; set; }

        [DynamoDBProperty]
        public string? FirstName { get; set; }

        [DynamoDBProperty] 
        public string? LastName { get; set; }

        [DynamoDBProperty] 
        public string? EmailAddress { get; set; }
        
        [DynamoDBProperty]
        public DateTime CreateDate { get; set; }
    }

}

