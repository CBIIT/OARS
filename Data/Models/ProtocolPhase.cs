using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheradexPortal.Data.Models
{
    [Table("PROTOCOLPHASE", Schema = "DMU")]
    public class ProtocolPhase
    {
        [Column("PROTOCOL_PHASE_ID"), Key]
        public int ProtocolPhaseId { get; set; }

        [Column("PROTOCOL_MAPPING_ID")]
        public int ProtocolMappingId { get; set; }

        [Column("SORT_ORDER")]
        public int? SortOrder { get; set; }

        [Column("IS_RANDOMIZED")]
        public char? IsRandomized { get; set; }

        [Column("IS_ENABLED")]
        public char? IsEnabled { get; set; }

        [Column("CREATE_DATE")]
        public DateTime? CreateDate { get; set; }

        [Column("UPDATE_DATE")]
        public DateTime? UpdateDate { get; set; }
    }
}

