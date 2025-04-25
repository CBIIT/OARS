using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OARS.Data.Models
{
    [Table("PROTOCOLFIELDMAPPING", Schema = "DMU")]
    public class ProtocolFieldMapping
    {
        [Key, Column("Protocol_Field_Mapping_Id")]
        public int ProtocolFieldMappingId { get; set; }
        [Column("THOR_FIELD_ID")]
        public string ThorFieldId { get; set; }
        [Column("PROTOCOL_EDC_FIELD_ID")]
        public int ProtocolEDCFieldId { get; set; }
        [Column("CREATE_DATE")]
        public DateTime CreateDate { get; set; }
        [Column("UPDATE_DATE")]
        public DateTime UpdateDate { get; set; }
        [ForeignKey("ProtocolEDCFieldId")]
        public ProtocolEDCField ProtocolEDCField { get; set; }
        [ForeignKey("ThorFieldId")]
        public ThorField? ThorField { get; set; }
        [NotMapped]
        public int ProtocolEDCFormId { get; set; }
        [NotMapped]
        public int ThorDictionaryId { get; set; }
    }
}


