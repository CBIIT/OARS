using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TheradexPortal.Data.Models
{
    [Table("REVIEW_ITEM", Schema = "THOR_USER")]
    public class ReviewItem
    {
        [Key, Column("REVIEW_ITEM_ID")]
        public int ReviewItemId { get; set; }
        [Column("REVIEW_ITEM")]
        public string? ReviewItemName { get; set; }
        [Column("REVIEW_TYPE")]
        public string? ReviewType { get; set; }
        public char? IsActive { get; set; }
        [Column("CREATE_DATE")]
        public DateTime? CreateDate { get; set; }
        [Column("UPDATE_DATE")]
        public DateTime? UpdateDate { get; set; }
    }
}
