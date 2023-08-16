using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TheradexPortal.Data.Models
{
    [Table("WRUSER_ACTIVITY_LOG", Schema = "WRUSER")]
    public class UserActivityLog
    {
        [Key]
        [Column("WRUSER_ACTIVITYLOG_ID")]
        public int WRUserActivityLogId { get; set; }
        public int UserId { get; set; }
        public DateTime? ActivityDate { get; set; }
        public string ActivityType { get; set; }
        public string Data1 { get; set; }
        public string Data2 { get; set; }
    }
}
