using System.ComponentModel.DataAnnotations.Schema;

namespace OARS.Data.Models
{
    [Table("PROTOCOLFORMMAPPING", Schema = "DMU")]
    public class ProtocolFormMapping
    {
        [Column("PROTOCOL_FORM_MAPPING_ID")]
        public int ProtocolFormMappingId { get; set; }
        [Column("PROTOCOL_EDC_FORM_ID")]
        public int? ProtocolEDCFormId { get; set; }
        [ForeignKey("ProtocolEDCFormId")]
        public ProtocolEDCForm ProtocolEDCForm { get; set; }
        [Column("IS_PRIMARY_FORM")]
        public bool IsPrimaryForm { get; set; } = false;
        [Column("CREATE_DATE")]
        public DateTime CreateDate { get; set; }
        [Column("UPDATE_DATE")]
        public DateTime UpdateDate { get; set; }
        [Column("PROTOCOL_CATEGORY_ID")]
        public int ProtocolCategoryId { get; set; }
        [ForeignKey("ProtocolCategoryId")]
        public ProtocolDataCategory ProtocolCategory { get; set; }
    }
}


