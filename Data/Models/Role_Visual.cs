using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheradexPortal.Data.Models
{
    [Table("WRROLE_VISUAL", Schema = "WRUSER")]
    [Keyless]
    public class Role_Visual
    {
        public int WRRole_Visual_Id { get; set; }
        public int RoleId { get; set; }
        public int VisualId { get; set; }
        public DateTime? Create_Date { get; set; }
        public DateTime? Update_Date { get; set; }
    }
}
