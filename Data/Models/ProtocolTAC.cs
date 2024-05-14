using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheradexPortal.Data.Models
{
    [Table("ProtocolTac", Schema = "DMU")]
    public class ProtocolTac
    {
        [Key, Column("Protocol_Tac_Id")]
        public int ProtocolTacId { get; set; }
        
        [Column("Protocol_Mapping_Id")]
        public int? ProtocolMappingId { get; set; }

        [Column("Tac_Code")]
        public string? TacCode { get; set; }
        
        [Column("Tac_Description")]
        public string? TacDescription { get; set; }
        
        [Column("Create_Date")]
        public DateTime CreateDate { get; set; }
        
        [Column("Update_Date")]
        public DateTime UpdatedDate { get; set; }
        
        [ForeignKey(nameof(ProtocolMappingId))]
        public ProtocolMapping ProtocolMapping { get; set; }
    }
}
