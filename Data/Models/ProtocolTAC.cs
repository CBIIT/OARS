using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheradexPortal.Data.Models
{
    [Table("PROTOCOLTAC", Schema = "DMU")]
    public class ProtocolTac
    {
        [Key, Column("Protocol_Tac_Id")]
        public int ProtocolTacId { get; set; }
        
        [Column("PROTOCOL_MAPPING_ID")]
        public int? ProtocolMappingId { get; set; }

        [Column("TAC_CODE")]
        public string? TacCode { get; set; }
        
        [Column("TAC_DESCRIPTION")]
        public string? TacDescription { get; set; }
        
        [Column("CREATE_DATE")]
        public DateTime CreateDate { get; set; }
        
        [Column("UPDATE_DATE")]
        public DateTime UpdatedDate { get; set; }
        
        [ForeignKey(nameof(ProtocolMappingId))]
        public ProtocolMapping ProtocolMapping { get; set; }
    }
}


