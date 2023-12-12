using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheradexPortal.Data.Models
{
    [Table("ROLE", Schema = "THOR_USER")]
    public class Role
    {
        [Key]
        public int RoleId { get; set; }
        [Column("Role_Name")]
        public string RoleName { get; set; }

        [Column("Create_Date")]
        public DateTime? CreateDate { get; set; }
        [Column("Update_Date")]
        public DateTime? UpdateDate { get; set; }
        [Column("AdminType")]
        public string  AdminType { get; set; }
        [Column("Is_Primary")]
        public bool IsPrimary { get; set;}
        public Role() 
        {
            CreateDate = DateTime.Now;

            RoleName = "";
        }
        public ICollection<RoleDashboard> RoleDashboards { get; } = new List<RoleDashboard>();
        public ICollection<RoleReport> RoleReports { get; } = new List<RoleReport>();
    }
}
