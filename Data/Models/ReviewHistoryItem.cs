using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TheradexPortal.Data.Models
{
    [Table("REVIEW_HISTORY_ITEM", Schema = "THOR_USER")]
    public class ReviewHistoryItem
    {
        [Key, Column("REVIEW_HISTORY_ITEM_ID")]
        public int ReviewHistoryItemId { get; set; }
        [Column("REVIEW_HISTORY_ID")]
        public int ReviewHistoryId { get; set; }
        [Column("REVIEW_ITEM_ID")]
        public int ReviewItemId { get; set; }
        public char? IsCompleted { get; set; }
        [Column("CREATE_DATE")]
        public DateTime? CreateDate { get; set; }
        [Column("UPDATE_DATE")]
        public DateTime? UpdateDate { get; set; }
    }
}
