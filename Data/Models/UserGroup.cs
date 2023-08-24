using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheradexPortal.Data.Models
{
    [Table("WRUSER_GROUP", Schema = "WRUSER")]
    public class UserGroup
    {
        [Key]
        [Column("WRUser_Group_Id")]
        public int WRUserGroupId { get; set; }
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
