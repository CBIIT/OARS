using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheradexPortal.Data.Models
{
    [Table("WRUSER_PROTOCOLHISTORY", Schema = "WRUSER")]
    public class UserProtocolHistory
    {
        [Key]
        [Column("WRUSER_PROTOCOLHISTORY_ID")]
        public int WRUserProtocolHistoryId { get; set; }
        public int UserId { get; set; }
        public string StudyId { get; set; }
        [Column("Create_Date")]
        public DateTime? CreateDate { get; set; }
    }
}
