using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheradexPortal.Data.Models
{
    [Table("WRROLE", Schema = "WRUSER")]
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

        public Role() 
        {
            CreateDate = DateTime.Now;
            UpdateDate = DateTime.Now;

            RoleName = "";
        }
    }
}
