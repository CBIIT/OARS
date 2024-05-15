using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheradexPortal.Data.Models
{
    [Table("ProtocolDataCategory", Schema = "DMU")]
    public class ProtocolDataCategory
    {
        [Key, Column("Protocol_Category_Id")]
        public int ProtocolCategoryId { get; set; }
        [Column("Protocol_Mapping_Id")]
        public int ProtocolMappingId { get; set; }
        [Column("THOR_Data_Category_Id")]
        public string THORDataCategoryId { get; set; }
        [Column("Protocol_Category_Status_Id")]
        public int ProtocolCategoryStatusId { get; set; }
        [Column("Create_Date")]
        public DateTime CreateDate { get; set; }
        [Column("Updated_Date")]
        public DateTime UpdateDate { get; set; }
        [Column("Is_Multi_Form")]
        public bool IsMultiForm { get; set; }
        [ForeignKey(nameof(ProtocolMappingId))]
        public virtual ProtocolMapping ProtocolMapping { get; set; }
        [ForeignKey(nameof(THORDataCategoryId))]
        public virtual ThorCategory THORDataCategory { get; set; }
        [ForeignKey(nameof(ProtocolCategoryStatusId))]
        public virtual ProtocolCategoryStatus ProtocolCategoryStatus { get; set; }
    }
}
