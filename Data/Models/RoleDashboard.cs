using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheradexPortal.Data.Models
{
    [Table("WRROLE_DASHBOARD", Schema = "WRUSER")]
    public class RoleDashboard
    {
        [Key]
        [Column("WROLE_DASHBOARD_ID")]
        public int WRRoleDashboardId { get; set; }
        public int RoleId { get; set; }
        public int DashboardId { get; set; }
        [Column("Create_Date")]
        public DateTime? CreateDate { get; set; }
        [Column("Update_Date")]
        public DateTime? UpdateDate { get; set; }
    }
}
