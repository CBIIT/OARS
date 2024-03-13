using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheradexPortal.Data.Models
{
    [Table("ProtocolPhase", Schema = "DMU")]
    public class ProtocolPhase
    {
        [Column("Protocol_Phase_Id"), Key]
        public int ProtocolPhaseId { get; set; }

        [Column("Protocol_Mapping_Id")]
        public int ProtocolMappingId { get; set; }

        [Column("Sort_Order")]
        public int? SortOrder { get; set; }

        [Column("Is_Randomized")]
        public char? IsRandomized { get; set; }

        [Column("Is_Enabled")]
        public char? IsEnabled { get; set; }

        [Column("Create_Date")]
        public DateTime? CreateDate { get; set; }

        [Column("Update_Date")]
        public DateTime? UpdateDate { get; set; }
    }
}