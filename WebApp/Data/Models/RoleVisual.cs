using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OARS.Data.Models
{
    [Table("ROLE_VISUAL", Schema = "THOR_USER")]
    public class RoleVisual
    {
        [Key]
        [Column("ROLE_VISUAL_ID")]
        public int RoleVisualId { get; set; }
        public int RoleId { get; set; }
        public int VisualId { get; set; }
        [Column("Create_Date")]
        public DateTime? CreateDate { get; set; }
        [Column("Update_Date")]
        public DateTime? UpdateDate { get; set; }
    }
}
