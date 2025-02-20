using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TheradexPortal.Data.Models
{
    [Table("REPORT_FILTER_ITEM", Schema = "THOR_USER")]
    public class ReportFilterItem
    {
        [Key, Column("REPORT_FILTER_ITEM_ID")]
        public int ReportFilterItemId { get; set; }

        [Column("REPORT_FILTER_ID")]
        public int ReportFilterId { get; set; }
        [Column("TABLE_NAME")]
        public string? TableName { get; set; }
        [Column("FIELD_NAME")]
        public string? FieldName { get; set; }
        [Column("DISPLAY_NAME")]
        public string? DisplayName { get; set; }
        [Column("DISPLAY_TYPE")]
        public string? DisplayType { get; set; }
        [Column("DATA")]
        public string? Data { get; set; }
        [Column("CREATE_DATE")]
        public DateTime? CreateDate { get; set; }
        [Column("UPDATE_DATE")]
        public DateTime? UpdateDate { get; set; }
    }
}
