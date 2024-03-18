using System.ComponentModel.DataAnnotations.Schema;

namespace TheradexPortal.Data.Models
{
    [Table("ProtocolEDCDictionary", Schema = "DMU")]
    public class ProtocolEDCDictionary
    {
        [Column("Protocol_EDC_Dictionary_Id")]
        public int ProtocolEDCDictionaryId { get; set; }
        [Column("Protocol_Mapping_Id")]
        public int? ProtocolMappingId { get; set; }
        [Column("EDC_Item_Id")]
        public string? EDCItemId { get; set; }
        [Column("EDC_Item_Name")]
        public string? EDCItemName { get; set; }
        [Column("EDC_Dictionary_Name")]
        public string? EDCDictionaryName { get; set; }
        [Column("Create_Date")]
        public DateTime CreateDate { get; set; }
        [Column("Updated_Date")]
        public DateTime UpdatedDate { get; set; }
        [ForeignKey(nameof(ProtocolMappingId))]
        public ProtocolMapping? ProtocolMapping { get; set; }
    }
}
