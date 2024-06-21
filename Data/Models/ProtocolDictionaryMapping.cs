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
        [ForeignKey("ProtocolFieldMappingId")]
        public ProtocolFieldMapping? ProtocolFieldMapping { get; set; }
        
        [Column("Protocol_EDC_Dictionary_Id")]
        public int ProtocolEDCDictionaryId { get; set; }
        [ForeignKey("ProtocolEDCDictionaryId")]
        public ProtocolEDCDictionary? ProtocolEDCDictionary { get; set; }

        [Column("THOR_Dictionary_Id")]
        public int THORDictionaryId { get; set; }
        [ForeignKey("THORDictionaryId")]
        public ThorDictionary? THORDictionary { get; set; }

        [Column("Create_Date")]
        public DateTime? CreateDate { get; set; }

        // Display fields
        [NotMapped]
        public string? ProtocolEDCDictionaryDisplay
        {
            get
            {
                if (ProtocolEDCDictionary == null)
                {
                    return "";
                }
                return ProtocolEDCDictionary.EDCItemName + " - " + ProtocolEDCDictionary.EDCItemId;
            }
        }
    }
}
