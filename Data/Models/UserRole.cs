using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheradexPortal.Data.Models
{
    [Table("WRUSER_ROLE", Schema = "WRUSER")]
    public class UserRole
    {
        [Key]
        [Column("WRUser_Role_Id")]
        public int WRUserRoleId { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
        [Column("Is_Admin")]
        public Boolean IsAdmin { get; set; }
        [Column("Expiration_Date")]
        public DateTime? ExpirationDate { get; set; }
        [Column("Create_Date")]
        public DateTime? CreateDate { get; set; }
        [Column("Update_Date")]
        public DateTime? UpdateDate { get; set; }

    }
}
