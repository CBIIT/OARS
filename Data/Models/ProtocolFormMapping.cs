using System.ComponentModel.DataAnnotations.Schema;

namespace TheradexPortal.Data.Models
{
    [Table("ProtocolFormMapping", Schema = "DMU")]
    public class ProtocolFormMapping
    {
        [Column("Protocol_Form_Mapping_Id")]
        public int ProtocolFormMappingId { get; set; }
        [Column("Protocol_EDC_Form_Id")]
        public int? ProtocolEDCFormId { get; set; }
        [Column("Is_Primary_Form")]
        public bool IsPrimaryForm { get; set; } = false;
        [Column("Create_Date")]
        public DateTime CreateDate { get; set; }
        [Column("Update_Date")]
        public DateTime UpdateDate { get; set; }
        [Column("Protocol_Category_Id")]
        public int ProtocolCategoryId { get; set; }
    }
}
