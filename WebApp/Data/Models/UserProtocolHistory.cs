using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OARS.Data.Models
{
    [Table("USER_PROTOCOLHISTORY", Schema = "THOR_USER")]
    public class UserProtocolHistory
    {
        [Key]
        [Column("USER_PROTOCOLHISTORY_ID")]
        public int UserProtocolHistoryId { get; set; }
        public int UserId { get; set; }
        public string StudyId { get; set; }
        [Column("Create_Date")]
        public DateTime? CreateDate { get; set; }
    }
}
