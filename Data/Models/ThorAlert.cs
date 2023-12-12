using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TheradexPortal.Data.Models
{
    [Table("ALERT", Schema = "THOR_USER")]
    public class ThorAlert
    {
        [Key]
        public int AlertId { get; set; }
        public string? PageName { get; set; }
        public int? DashboardId { get; set; }
        public string? AlertType { get; set; }
        public string? AlertText { get; set; }
        public Boolean IsActive { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
