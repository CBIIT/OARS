using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TheradexPortal.Data.Models
{
    [Table("REVIEW", Schema = "THOR_USER")]
    public class Review
    {
        [Key]
        public int ReviewId { get; set; }
        public int ProtocolId { get; set; }
        public string? ReviewType { get; set; }
        public int UserId { get; set; }
        public string? EmailAddress { get; set; }
        public int ReviewPeriod { get; set; }
        public string? ReviewStatus { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? NextDueDate { get; set; }
    }
}
