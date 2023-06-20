using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheradexPortal.Data.Models
{
    [Table("WRVISUAL", Schema = "WRUSER")]
    public class Visual
    {
        [Key]
        public int WRVisualId { get; set; }
        [Column("Visual_Name")]
        public string? VisualName { get; set; }
        [Column("Visual_Description")]
        public string? VisualDescription { get; set; }
        [Column("Report_Id")]
        public int ReportId { get; set; }
        [Column("Page_Name")]
        public string? PageName { get; set; }
        [Column("PowerBI_Visual_Id")]
        public string? PowerBIVisualId { get; set; }
        [Column("Create_Date")]
        public DateTime? CreateDate { get; set; }
        [Column("Update_Date")]
        public DateTime? UpdateDate { get; set; }
        [Column("Visual_Type")]
        public string? VisualType { get; set; }
        [Column("Display_Order")]
        public int? DisplayOrder { get; set; }
    }
}
