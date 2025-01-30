using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TheradexPortal.Data.Models
{
    [Table("REVIEW", Schema = "THOR_USER")]
    public class Review
    {
        [Key, Column("REVIEW_ID")]
        public int ReviewId { get; set; }
        [Column("PROTOCOL_ID")]
        public int ProtocolId { get; set; }
        [Column("REVIEW_TYPE")]
        public string? ReviewType { get; set; }
        public int UserId { get; set; }
        [Column("EMAIL_ADDRESS")]
        public string? EmailAddress { get; set; }
        [Column("REVIEW_PERIOD")]
        public int ReviewPeriod { get; set; }
        [Column("REVIEW_STATUS")]
        public string? ReviewStatus { get; set; }
        [Column("CREATE_DATE")]
        public DateTime? CreateDate { get; set; }
        [Column("UPDATE_DATE")]
        public DateTime? UpdateDate { get; set; }
        [Column("NEXT_DUE_DATE")]
        public DateTime? NextDueDate { get; set; }
        [Column("AGENT_NAME")]
        public string? AgentName { get; set; }
        [Column("NSC")]
        public string? NSCNumber { get; set; }
        [Column("LEAD_AGENT")]
        public string? LeadAgent { get;set; }
        [Column("REVIEW_PERIOD_NAME")]
        public string? ReviewPeriodName { get;set; }
        [Column("REVIEW_PERIOD_UPCOMING")]
        public int ReviewPeriodUpcoming { get; set; }
    }
}
