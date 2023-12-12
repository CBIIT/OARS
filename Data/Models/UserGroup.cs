using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheradexPortal.Data.Models
{
    [Table("USER_GROUP", Schema = "THOR_USER")]
    public class UserGroup
    {
        [Key]
        [Column("User_Group_Id")]
        public int UserGroupId { get; set; }
        public int UserId { get; set; }
        public int GroupId { get; set; }
        [Column("Expiration_Date")]
        public DateTime? ExpirationDate { get; set; }
        [Column("Create_Date")]
        public DateTime? CreateDate { get; set; }
        [Column("Update_Date")]
        public DateTime? UpdateDate { get; set; }
        public Group Group { get; set; }
    }
}
