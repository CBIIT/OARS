using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OARS.Data.Models
{
    [Table("USER_ACTIVITY_LOG", Schema = "THOR_USER")]
    public class UserActivityLog
    {
        [Key]
        [Column("USER_ACTIVITYLOG_ID")]
        public int UserActivityLogId { get; set; }
        public int UserId { get; set; }
        public DateTime? ActivityDate { get; set; }
        public string ActivityType { get; set; }
        public string Data1 { get; set; }
        public string Data2 { get; set; }
    }
}
