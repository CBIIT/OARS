using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheradexPortal.Data.Models
{
    [Table("ProtocolField", Schema = "DMU")]
    public class ProtocolField
    {
        [Key, Column("Protocol_Field_Id")]
        public int ProtocolFieldId { get; set; }
        [Column("Protocol_Mapping_Id")]
        public int ProtocolMappingId { get; set; }
        [Column("THOR_Field_Id")]
        public string ThorFieldId { get; set; }
        [Column("Format")]
        public string Format { get; set; }
        [Column("Is_Required")]
        public char IsRequired { get; set; }
        [Column("Is_Enabled")]
        public char IsEnabled { get; set; }
        [Column("Can_Be_Dictionary")]
        public char CanBeDictionary { get; set; }
        [Column("Is_Multi_Form")]
        public char IsMultiForm { get; set; }
        [Column("Create_Date")]
        public DateTime CreateDate { get; set; }
        [Column("Updated_Date")]
        public DateTime UpdateDate { get; set; }
        [ForeignKey(nameof(ProtocolMappingId))]
        public ProtocolMapping ProtocolMapping { get; set; }
        [ForeignKey(nameof(ThorFieldId))]
        public ThorField ThorField { get; set; }
    }
}
