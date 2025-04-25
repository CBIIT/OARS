using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace OARS.Data.Models
{
    [Table("DASHBOARD", Schema = "THOR_USER")]
    public class Dashboard
    {
        [Key]
        public int DashboardId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        [Column("Display_Order")]
        public int DisplayOrder { get; set; }
        [Column("Create_Date")]
        public DateTime? CreateDate { get; set; }
        [Column("Update_Date")]
        public DateTime? UpdateDate { get; set; }
        [Column("Custom_Page_Path")]
        public string? CustomPagePath { get; set; }
        [Column("PowerBI_Report_Id")]
        public string? PowerBIReportId { get; set; }
        [Column("Help_FileName")]
        public string? HelpFileName { get; set; }
        [Column("SpecialDash")]
        public Boolean SpecialDash {  get; set; }

        public ICollection<Report> Reports { get; } = new List<Report>();
    }
}
