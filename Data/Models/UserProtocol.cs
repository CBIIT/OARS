using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheradexPortal.Data.Models
{
    [Table("WRUSER_PROTOCOL", Schema = "WRUSER")]
    public class UserProtocol
    {
        [Key]
        [Column("WRUser_Protocol_Id")]
        public int WRUserProtocolId { get; set; }
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
