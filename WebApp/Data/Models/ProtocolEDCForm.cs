using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OARS.Data.Models
{
    [Table("PROTOCOLEDCFORM", Schema = "DMU")]
    public class ProtocolEDCForm
    {
        [Key, Column("Protocol_EDC_Form_Id")]
        public int ProtocolEDCFormId { get; set; }
        [Column("PROTOCOL_MAPPING_ID")]
        public int? ProtocolMappingId { get; set; }
        [Column("EDC_FORM_IDENTIFIER")]
        public string? EDCFormIdentifier { get; set; }
        [Column("EDC_FORM_NAME")]
        public string? EDCFormName { get; set; }
        [NotMapped]
        public string EDCFormDisplay
        {
            get
            {
                return (EDCFormIdentifier ?? "") + " - " + (EDCFormName ?? "");
            }
        }

        [Column("CREATE_DATE")]
        public DateTime CreateDate { get; set; }
        [Column("UPDATE_DATE")]
        public DateTime UpdatedDate { get; set; }
        [Column("EDC_FORM_TABLE")]
        public string? EDCFormTable { get; set; }
        [ForeignKey(nameof(ProtocolMappingId))]
        public ProtocolMapping ProtocolMapping { get; set; }
    }
}


