using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace TheradexPortal.Data.Models
{
    [Table("Profile_Field", Schema = "DMU")]
    public class ProfileField
    {
        [Column("Profile_Field_Id"), Key]
        public int ProfileFieldId { get; set; }

        [Column("Profile_Id")]
        public int? ProfileId { get; set; }

        [Column("THOR_Field_Id")]
        public string THORFieldId { get; set; }

        [ForeignKey(nameof(THORFieldId))]
        public ThorField ThorField { get; set; }

        [Column("Create_Date")]
        public DateTime? CreateDate { get; set; }

    }
}
