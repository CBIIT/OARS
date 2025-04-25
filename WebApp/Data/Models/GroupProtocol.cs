using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OARS.Data.Models
{
    [Table("GROUP_PROTOCOL", Schema = "THOR_USER")]
    public class GroupProtocol
    {
        [Key]
        [Column("Group_Protocol_Id")]
        public int GroupProtocolId { get; set; }
        public int GroupId { get; set; }
        public Boolean IsActive { get; set; }
        public string? StudyId { get; set; }
        [Column("Create_Date")]
        public DateTime? CreateDate { get; set; }
        [Column("Update_Date")]
        public DateTime? UpdateDate { get; set; }
    }
}
