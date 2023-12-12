using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheradexPortal.Data.Models
{
    [Table("USER_PROTOCOL", Schema = "THOR_USER")]
    public class UserProtocol
    {
        [Key]
        [Column("User_Protocol_Id")]
        public int UserProtocolId { get; set; }
        public int UserId { get; set; }
        public string StudyId { get; set; }
        [Column("Expiration_Date")]
        public DateTime? ExpirationDate { get; set; }
        [Column("Create_Date")]
        public DateTime? CreateDate { get; set; }
        [Column("Update_Date")]
        public DateTime? UpdateDate { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
    }
}
