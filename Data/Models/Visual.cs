using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheradexPortal.Data.Models
{
    [Table("WRVISUAL", Schema = "WRUSER")]
    [Keyless]
    public class Visual
    {
        public int WRVisualId { get; set; }
        public string? Visual_Name { get; set; }
        public string? Visual_Description { get; set; }
        public int Report_Id { get; set; }
        public string? Page_Name { get; set; }
        public string? PowerBI_Visual_Id { get; set; }
        public DateTime? Create_Date { get; set; }
        public DateTime? Update_Date { get; set; }
        public string? Visual_Type { get; set; }
        public int? Display_Order { get; set; }
    }
}
