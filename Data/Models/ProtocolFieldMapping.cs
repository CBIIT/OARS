using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheradexPortal.Data.Models
{
    [Table("ProtocolFieldMapping", Schema = "DMU")]
    public class ProtocolFieldMapping
    {
        [Key, Column("Protocol_Field_Mapping_Id")]
        public int ProtocolFieldMappingId { get; set; }
        [Column("THOR_Field_Id")]
        public string ThorFieldId { get; set; }
        [Column("Protocol_EDC_Field_Id")]
        public int ProtocolEDCFieldId { get; set; }
        [Column("Create_Date")]
        public DateTime CreateDate { get; set; }
        [Column("Update_Date")]
        public DateTime UpdateDate { get; set; }
        [ForeignKey("ProtocolEDCFieldId")]
        public ProtocolEDCField ProtocolEDCField { get; set; }
        [ForeignKey("ThorFieldId")]
        public ThorField ThorField { get; set; }
        [NotMapped]
        public int ProtocolEDCFormId { get; set; }
    }
}
