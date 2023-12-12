using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheradexPortal.Data.Models
{
    [Table("USER_ROLE", Schema = "THOR_USER")]
    public class UserRole
    {
        [Key]
        [Column("User_Role_Id")]
        public int UserRoleId { get; set; }
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
            //CreateDate = DateTime.Now;
            //UpdateDate = DateTime.Now;

            //User = new User();
            //Role = new Role();
        }

    }
}
