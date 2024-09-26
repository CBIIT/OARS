using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheradexPortal.Data.Models
{
    [Table("PROTOCOLDATASYSTEM", Schema = "DMU")]
    public class ProtocolDataSystem
    {
        [Column("PROTOCOL_DATA_SYSTEM_ID"), Key]
        public int ProtocolDataSystemId { get; set; }

        [Column("DATA_SYSTEM_NAME")]
        public string? DataSystemName { get; set; }

        [Column("SORT_ORDER")]
        public int? SortOrder { get; set; }

        [Column("IS_ACTIVE")]
        public bool IsActive { get; set; }

        [Column("CREATE_DATE")]
        public DateTime? CreateDate { get; set; }

        [Column("UPDATE_DATE")]
        public DateTime? UpdateDate { get; set; }
    }
}


