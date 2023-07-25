using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheradexPortal.Data.Models
{
    [Table("WRROLE_REPORT", Schema = "WRUSER")]
    public class RoleReport
    {
        [Key]
        [Column("WROLE_REPORT_ID")]
        public int WRRoleReportId { get; set; }
        public int RoleId { get; set; }
        public int ReportId { get; set; }
        [Column("Create_Date")]
        public DateTime? CreateDate { get; set; }
        [Column("Update_Date")]
        public DateTime? UpdateDate { get; set; }
    }
}
