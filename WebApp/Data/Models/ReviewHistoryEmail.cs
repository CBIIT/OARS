using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OARS.Data.Models
{
    [Table("REVIEW_HISTORY_EMAIL", Schema = "THOR_USER")]
    public class ReviewHistoryEmail
    {
        [Key, Column("REVIEW_HISTORY_EMAIL_ID")]
        public int ReviewHistoryEmailId { get; set; }
        [Column("REVIEW_HISTORY_ID")]
        public int ReviewHistoryId { get; set; }
        [Column("EMAIL_TO_ADDRESS")]
        public string? EmailToAddress { get; set; }
        [Column("EMAIL_TEXT")]
        public string? EmailText { get; set; }
        [Column("CREATE_DATE")]
        public DateTime? CreateDate { get; set; }
    }
}
