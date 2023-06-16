using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheradexPortal.Data.Models
{
    [Table("WRUSER_ROLE", Schema = "WRUSER")]
    [Keyless]
    public class User_Role
    {
        public int WRUser_Role_Id { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public Boolean Is_Admin { get; set; }
        public DateTime? Expiration_Date { get; set; }
        public DateTime? Create_Date { get; set; }
        public DateTime? Update_Date { get; set; }

    }
}
