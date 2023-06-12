using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace TheradexPortal.Data.Models
{
    [Table("WRREPORT", Schema = "WRUSER")]
    public class Report
    {
        [Key]
        public int WRReportId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int DashboardId { get; set; }
        public int Display_Order { get; set; }
        public string? Display_Icon_Name { get; set; }
        public DateTime? Create_Date { get; set; }
        public DateTime? Update_Date { get; set; }
        public bool Is_Full_Page { get; set; }
        public string? Custom_Page_Path { get; set; }
        public string? PowerBI_Page_Name { get; set; }

        public Dashboard? Dashboard { get; init; }

    }
}
