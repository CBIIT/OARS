using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OARS.Data.Models
{
    [Table("ROLE_DASHBOARD", Schema = "THOR_USER")]
    public class RoleDashboard
    {
        [Key]
        [Column("ROLE_DASHBOARD_ID")]
        public int RoleDashboardId { get; set; }
        public int RoleId { get; set; }
        public int DashboardId { get; set; }
        [Column("Create_Date")]
        public DateTime? CreateDate { get; set; }
        [Column("Update_Date")]
        public DateTime? UpdateDate { get; set; }
    }
}
