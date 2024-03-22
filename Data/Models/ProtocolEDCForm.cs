using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheradexPortal.Data.Models
{
    [Table("ProtocolEDCForm", Schema = "DMU")]
    public class ProtocolEDCForm
    {
        [Key, Column("Protocol_EDC_Form_Id")]
        public int ProtocolEDCFormId { get; set; }
        [Column("Protocol_Mapping_Id")]
        public int? ProtocolMappingId { get; set; }
        [Column("EDC_Form_Identifier")]
        public string? EDCFormIdentifier { get; set; }
        [Column("EDC_Form_Name")]
        public string? EDCFormName { get; set; }
        [Column("Create_Date")]
        public DateTime CreateDate { get; set; }
        [Column("Update_Date")]
        public DateTime UpdatedDate { get; set; }
        [ForeignKey(nameof(ProtocolMappingId))]
        public ProtocolMapping ProtocolMapping { get; set; }
    }
}
