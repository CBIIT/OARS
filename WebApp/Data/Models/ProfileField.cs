using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace OARS.Data.Models
{
    [Table("PROFILE_FIELD", Schema = "DMU")]
    public class ProfileField
    {
        [Column("PROFILE_FIELD_ID"), Key]
        public int ProfileFieldId { get; set; }

        [Column("PROFILE_ID")]
        public int? ProfileId { get; set; }

        [Column("THOR_FIELD_ID")]
        public string THORFieldId { get; set; }

        [ForeignKey(nameof(THORFieldId))]
        public ThorField ThorField { get; set; }

        [Column("CREATE_DATE")]
        public DateTime? CreateDate { get; set; }

        [NotMapped]
        public string ThorFieldDisplay
        {
            get
            {
                if (ThorField == null)
                {
                    return "";
                }
                return ThorField.FieldDisplay;
            }
        }

        [NotMapped]
        public string ThorFieldCategoryDisplay
        {
            get
            {
                if (ThorField == null)
                {
                    return "";
                }
                return ThorField.Category?.CategoryDisplay ?? "";
            }
        }
    }
}


