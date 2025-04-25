using System.ComponentModel.DataAnnotations.Schema;

namespace OARS.Data.Models
{
    [Table("PROTOCOLDICTIONARYMAPPING", Schema = "DMU")]
    public class ProtocolDictionaryMapping
    {
        [Column("PROTOCOL_DICTIONARY_MAPPING_ID")]
        public int ProtocolDictionaryMappingId { get; set; }
        [Column("PROTOCOL_FIELD_MAPPING_ID")]
        public int ProtocolFieldMappingId { get; set; }
        [ForeignKey("ProtocolFieldMappingId")]
        public ProtocolFieldMapping? ProtocolFieldMapping { get; set; }
        
        [Column("PROTOCOL_EDC_DICTIONARY_ID")]
        public int ProtocolEDCDictionaryId { get; set; }
        [ForeignKey("ProtocolEDCDictionaryId")]
        public ProtocolEDCDictionary? ProtocolEDCDictionary { get; set; }

        [Column("THOR_DICTIONARY_ID")]
        public int THORDictionaryId { get; set; }
        [ForeignKey("THORDictionaryId")]
        public ThorDictionary? THORDictionary { get; set; }

        [Column("CREATE_DATE")]
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


