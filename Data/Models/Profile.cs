
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheradexPortal.Data.Models
{
    [Table("Profile", Schema = "DMU")]
    public class Profile
    {
        [Column("Profile_Id"), Key]
        public int ProfileId { get; set; }
        
        [Column("Profile_Name")]
        public string? ProfileName { get; set; }

        [Column("Profile_Version")]
        public int? ProfileVersion { get; set; }

        [Column("Profile_Id_Source")]
        public int? ProfileIdSource { get; set; }

        [Column("Create_Date")]
        public DateTime? CreateDate { get; set; }

        [Column("Update_Date")]
        public DateTime? UpdateDate { get; set; }
    }
}