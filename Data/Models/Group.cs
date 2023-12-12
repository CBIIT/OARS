using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheradexPortal.Data.Models
{
    [Table("GROUP", Schema = "THOR_USER")]
    public class Group
    {
        [Key]
        [Column("GroupId")]
        public int GroupId { get; set; }
        [Column("Group_Name")]
        public string? GroupName { get; set; }
        [Column("Create_Date")]
        public DateTime? CreateDate { get; set; }
        [Column("Update_Date")]
        public DateTime? UpdateDate { get; set; }
        public ICollection<GroupProtocol> GroupProtocols { get; } = new List<GroupProtocol>();
    }
}
