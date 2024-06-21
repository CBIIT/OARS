using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace TheradexPortal.Data.Models
{
    [Table("THORDataCategory", Schema = "DMU")]
    public class ThorCategory
    {
        [Column("THOR_Data_Category_Id"), Key]
        public string ThorDataCategoryId { get; set; }
        [Column("Category_Name")]
        public string? CategoryName { get; set; }
        [NotMapped]
        public string CategoryDisplay
        {
            get
            {
                return CategoryName + " (" + ThorDataCategoryId + ")";
            }
        }
        [Column("Is_Multi_Form")]
        public Boolean IsMultiForm { get; set; }
        [Column("Sort_Order")]
        public int? SortOrder { get; set; }
        [Column("Is_Active")]
        public Boolean IsActive { get; set; }
        [Column("Create_Date")]
        public DateTime? CreateDate { get; set; }
        [Column("Update_Date")]
        public DateTime? UpdateDate { get; set;}
    }

}
