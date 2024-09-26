using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace TheradexPortal.Data.Models
{
    [Table("THORDATACATEGORY", Schema = "DMU")]
    public class ThorCategory
    {
        [Column("THOR_DATA_CATEGORY_ID"), Key]
        public string ThorDataCategoryId { get; set; }
        [Column("CATEGORY_NAME")]
        public string? CategoryName { get; set; }
        [NotMapped]
        public string CategoryDisplay
        {
            get
            {
                return CategoryName + " (" + ThorDataCategoryId + ")";
            }
        }
        [Column("IS_MULTI_FORM")]
        public Boolean IsMultiForm { get; set; }
        [Column("SORT_ORDER")]
        public int? SortOrder { get; set; }
        [Column("IS_ACTIVE")]
        public Boolean IsActive { get; set; }
        [Column("CREATE_DATE")]
        public DateTime? CreateDate { get; set; }
        [Column("UPDATE_DATE")]
        public DateTime? UpdateDate { get; set;}
    }

}


