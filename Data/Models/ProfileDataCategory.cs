using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Bcpg;

namespace TheradexPortal.Data.Models
{
    [Table("Profile_DataCategory", Schema = "DMU")]
    public class ProfileDataCategory
    {
        [Column("Profile_Data_Cateogy_Id"), Key]
        public int ProfileDataCategoryId { get; set; }
        [Column("Profile_Id")]
        public int? ProfileId { get; set; }

        [ForeignKey(nameof(ProfileId))]
        public Profile? Profile { get; set; }

        [Column("THOR_Data_Category_Id")]
        public string? ThorDataCategoryId { get; set; }

        [ForeignKey(nameof(ThorDataCategoryId))]
        public ThorCategory ThorCategory { get; set; }
        [Column("Create_Date")]
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
