using System.ComponentModel.DataAnnotations.Schema;

namespace TheradexPortal.Data.Models
{
    [Table("ProtocolDictionaryMapping", Schema = "DMU")]
    public class ProtocolDictionaryMapping
    {
        [Column("Protocol_Dictionary_Mapping_Id")]
        public int ProtocolDictionaryMappingId { get; set; }
        [Column("Protocol_Field_Mapping_Id")]
        public int ProtocolFieldMappingId { get; set; }
        [Column("Protocol_EDC_Dictionary_Id")]
        public int ProtocolEDCDictionaryId { get; set; }
        [Column("THOR_Dictionary_Id")]
        public int THORDictionaryId { get; set; }
        [Column("Create_Date")]
        public DateTime? CreateDate { get; set; }

        // Display fields
        public string? ProtocolEDCDictionaryName { get; set; }
    }
}
