using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TheradexPortal.Data.Models
{
    [Table("THORDataCategory", Schema = "DMU")]
    public class WRCategory
    {
        [Key]
        public int ThorDataCategoryId { get; set; }
        public string? CategoryName { get; set; }
        public Boolean IsMultiForm { get; set; }
        public int? SortOrder { get; set; }
        public Boolean IsActive { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set;}
    }
}
