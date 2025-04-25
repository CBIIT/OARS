using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Bcpg;

namespace OARS.Data.Models
{
    [Table("PROFILE_DATACATEGORY", Schema = "DMU")]
    public class ProfileDataCategory
    {
        [Column("PROFILE_DATA_CATEOGY_ID"), Key]
        public int ProfileDataCategoryId { get; set; }
        [Column("PROFILE_ID")]
        public int? ProfileId { get; set; }

        [ForeignKey(nameof(ProfileId))]
        public Profile? Profile { get; set; }

        [Column("THOR_DATA_CATEGORY_ID")]
        public string? ThorDataCategoryId { get; set; }

        [ForeignKey(nameof(ThorDataCategoryId))]
        public ThorCategory ThorCategory { get; set; }
        [Column("CREATE_DATE")]
        public DateTime? CreateDate { get; set; }

        [NotMapped]
        public string ThorCategoryDisplay {
            get
            {
                if (ThorCategory == null)
                {
                    return "";
                }
                return ThorCategory.CategoryDisplay;
            }
        }
    }
}


