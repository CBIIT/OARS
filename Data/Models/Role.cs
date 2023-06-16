using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheradexPortal.Data.Models
{
    [Table("WRROLE", Schema = "WRUSER")]
    [Keyless]
    public class Role
    {
        public int RoleId { get; set; }
        public string? Role_Name { get; set; }
        public DateTime? Create_Date { get; set; }
        public DateTime? Update_Date { get; set; }
    }
}
