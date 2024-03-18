using System.ComponentModel.DataAnnotations.Schema;

namespace TheradexPortal.Data.Models
{
    [Table("ProtocolEDCField", Schema = "DMU")]
    public class ProtocolEDCField
    {
        [Column("Protocol_EDC_Field_Id")]
        public int ProtocolEDCFieldId { get; set; }
        [Column("Protocol_EDC_Form_Id")]
        public int ProtocolEDCFormId { get; set; }
        [Column("EDC_Field_Identifier")]
        public string? EDCFieldIdentifier { get; set; }
        [Column("EDC_Field_Name")]
        public string? EDCFieldName { get; set; }
        [Column("EDC_Dictionary_Name")]
        public string? EDCDictionaryName { get; set; }
        [Column("Create_Date")]
        public DateTime CreateDate { get; set; }
        [Column("Update_Date")]
        public DateTime UpdateDate { get; set; }
    }
}
