using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace TheradexPortal.Data.Models
{
    [Table("WRDASHBOARD", Schema = "WRUSER")]
    public class Dashboard
    {
        [Key]
        public int WRDashboardId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int Display_Order { get; set; }
        public DateTime? Create_Date { get; set; }
        public DateTime? Update_Date { get; set; }
        public string? Custom_Page_Path { get; set; }
        public string? PowerBI_Report_Id { get; set; }

        public ICollection<Report> Reports { get; } = new List<Report>();
    }
}
