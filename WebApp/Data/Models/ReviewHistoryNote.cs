using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OARS.Data.Models
{
    [Table("REVIEW_HISTORY_NOTE", Schema = "THOR_USER")]
    public class ReviewHistoryNote
    {
        [Key, Column("REVIEW_HISTORY_NOTE_ID")]
        public int ReviewHistoryNoteId { get; set; }
        [Column("REVIEW_HISTORY_ID")]
        public int ReviewHistoryId { get; set; }
        [Column("NOTE_TEXT")]
        public string? NoteText { get; set; }
        [Column("CREATE_DATE")]
        public DateTime? CreateDate { get; set; }
    }
}
