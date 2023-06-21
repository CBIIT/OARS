using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheradexPortal.Data.Models
{
    [Table("WRROLE_VISUAL", Schema = "WRUSER")]
    public class RoleVisual
    {
        [Key]
        [Column("WRole_VISUAL_ID")]
        public int WRRoleVisualId { get; set; }
        public int RoleId { get; set; }
        public int VisualId { get; set; }
        [Column("Create_Date")]
        public DateTime? CreateDate { get; set; }
        [Column("Update_Date")]
        public DateTime? UpdateDate { get; set; }
    }
}
