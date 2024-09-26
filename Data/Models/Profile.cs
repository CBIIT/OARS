
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheradexPortal.Data.Models
{
    [Table("PROFILE", Schema = "DMU")]
    public class Profile
    {
        [Column("PROFILE_ID"), Key]
        public int ProfileId { get; set; }
        
        [Column("PROFILE_NAME")]
        public string? ProfileName { get; set; }

        [Column("PROFILE_VERSION")]
        public int? ProfileVersion { get; set; }

        [Column("PROFILE_ID_SOURCE")]
        public int? ProfileIdSource { get; set; }

        [Column("CREATE_DATE")]
        public DateTime? CreateDate { get; set; }

        [Column("UPDATE_DATE")]
        public DateTime? UpdateDate { get; set; }
    }
}

