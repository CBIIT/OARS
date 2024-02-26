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
        [Column("Thor_Field_Id")]
        public string THORFieldId { get; set; }
        [Column("Format")]
        public string Format { get; set; }
        [Column("Is_Required")]
        public bool IsRequired { get; set; }
        [Column("Is_Enabled")]
        public bool IsEnabled { get; set; }
        [Column("Can_Be_Dictionary")]
        public bool CanBeDictionary { get; set; }
        [Column("Is_Multi_Form")]
        public bool IsMultiForm { get; set; }
        [Column("Create_Date")]
        public DateTime CreateDate { get; set; }
        [Column("Updated_Date")]
        public DateTime UpdateDate { get; set; }
        [ForeignKey(nameof(ProtocolMappingId))]
        public ProtocolMapping ProtocolMapping { get; set; }
        [ForeignKey(nameof(THORFieldId))]
        public ThorField ThorField { get; set; }
    }
}
