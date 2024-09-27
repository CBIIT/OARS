using System.ComponentModel.DataAnnotations.Schema;

namespace TheradexPortal.Data.Models
{
    [Table("PROTOCOLEDCDICTIONARY", Schema = "DMU")]
    public class ProtocolEDCDictionary
    {
        [Column("PROTOCOL_EDC_DICTIONARY_ID")]
        public int ProtocolEDCDictionaryId { get; set; }
        [Column("PROTOCOL_MAPPING_ID")]
        public int? ProtocolMappingId { get; set; }
        [Column("EDC_ITEM_ID")]
        public string? EDCItemId { get; set; }
        [Column("EDC_ITEM_NAME")]
        public string? EDCItemName { get; set; }
        [Column("EDC_DICTIONARY_NAME")]
        public string? EDCDictionaryName { get; set; }
        [Column("CREATE_DATE")]
        public DateTime CreateDate { get; set; }
        [Column("UPDATED_DATE")]
        public DateTime UpdatedDate { get; set; }
        [ForeignKey(nameof(ProtocolMappingId))]
        public ProtocolMapping? ProtocolMapping { get; set; }
    }
}


