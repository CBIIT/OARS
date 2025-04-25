using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OARS.Data.Models
{
    [Table("ROLE_REPORT", Schema = "THOR_USER")]
    public class RoleReport
    {
        [Key]
        [Column("ROLE_REPORT_ID")]
        public int RoleReportId { get; set; }
        public int RoleId { get; set; }
        public int ReportId { get; set; }
        [Column("Create_Date")]
        public DateTime? CreateDate { get; set; }
        [Column("Update_Date")]
        public DateTime? UpdateDate { get; set; }
    }
}
