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
        [Column("Expiration_Date")]
        public DateTime? ExpirationDate { get; set; }
        [Column("Create_Date")]
        public DateTime? CreateDate { get; set; }
        [Column("Update_Date")]
        public DateTime? UpdateDate { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        //[ForeignKey(nameof(RoleId))]
        public Role Role { get; set; }

        public UserRole()
        {
            CreateDate = DateTime.Now;
            UpdateDate = DateTime.Now;

            //User = new User();
            //Role = new Role();
        }

    }
}
