using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheradexPortal.Data.Models
{
    [Table("WRGROUP_PROTOCOL", Schema = "WRUSER")]
    public class GroupProtocol
    {
        [Key]
        [Column("WRGroup_Protocol_Id")]
        public int WRGroupProtocolId { get; set; }
        public int GroupId { get; set; }
        public Boolean IsActive { get; set; }
        public string? StudyId { get; set; }
        [Column("Create_Date")]
        public DateTime? CreateDate { get; set; }
        [Column("Update_Date")]
        public DateTime? UpdateDate { get; set; }
    }
}
