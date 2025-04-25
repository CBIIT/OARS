using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OARS.Data.Models
{
    [Table("REPORT_FILTER", Schema = "THOR_USER")]
    public class ReportFilter
    {
        [Key, Column("REPORT_FILTER_ID")]
        public int ReportFilterId { get; set; }

        [Column("FILTER_NAME")]
        public string? FilterName { get; set; }
        [Column("FILTER_DESCRIPTION")]
        public string? FilterDescription { get; set; }
        [Column("CREATE_DATE")]
        public DateTime? CreateDate { get; set; }
        [Column("UPDATE_DATE")]
        public DateTime? UpdateDate { get; set; }
        public ICollection<ReportFilterItem> FilterItems { get; } = new List<ReportFilterItem>();
    }
}
