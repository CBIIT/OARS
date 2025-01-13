using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TheradexPortal.Data.Models
{
    [Table("REVIEW_HISTORY", Schema = "THOR_USER")]
    public class ReviewHistory
    {
        [Key, Column("REVIEW_HISTORY_ID")]
        public int ReviewHistoryId { get; set; }
        public int UserId { get; set; }
        [Column("EMAIL_ADDRESS")]
        public string? EmailAddress { get; set; }
        [Column("REVIEW_TYPE")]
        public string? ReviewType { get; set; }
        [Column("DUE_DATE")]
        public DateTime? DueDate { get; set; }
        [Column("REVIEW_COMPLETE_DATE")]
        public DateTime? ReviewCompleteDate { get; set; }
        [Column("REVIEW_LATE")]
        public char ReviewLate { get; set; }
        [Column("REVIEW_STATUS")]
        public string? ReviewStatus { get; set; }
        [Column("DAYS_LATE")]
        public int? DaysLate { get; set; }
        [Column("CREATE_DATE")]
        public DateTime? CreateDate { get; set; }
        [Column("UPDATE_DATE")]
        public DateTime? UpdateDate { get; set; }
        [Column("PROTOCOL_ID")]
        public int? ProtocolId { get; set; }
        [Column("REVIEW_ID")]
        public int? ReviewId { get; set; }
        [Column("REVIEW_PERIOD_NAME")]
        public string? ReviewPeriodName { get; set; }
        [Column("IS_WEBREPORTING")]
        public char? IsWebReporting { get; set; }
    }
}
