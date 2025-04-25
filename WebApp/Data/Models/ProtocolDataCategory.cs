using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OARS.Data.Models
{
    [Table("PROTOCOLDATACATEGORY", Schema = "DMU")]
    public class ProtocolDataCategory
    {
        [Key, Column("Protocol_Category_Id")]
        public int ProtocolCategoryId { get; set; }
        [Column("PROTOCOL_MAPPING_ID")]
        public int ProtocolMappingId { get; set; }
        [Column("THOR_DATA_CATEGORY_ID")]
        public string THORDataCategoryId { get; set; }
        [Column("PROTOCOL_CATEGORY_STATUS_ID")]
        public int ProtocolCategoryStatusId { get; set; }
        [Column("CREATE_DATE")]
        public DateTime CreateDate { get; set; }
        [Column("UPDATED_DATE")]
        public DateTime UpdateDate { get; set; }
        [Column("IS_MULTI_FORM")]
        public bool IsMultiForm { get; set; }
        [ForeignKey(nameof(ProtocolMappingId))]
        public virtual ProtocolMapping ProtocolMapping { get; set; }
        [ForeignKey(nameof(THORDataCategoryId))]
        public virtual ThorCategory THORDataCategory { get; set; }
        [ForeignKey(nameof(ProtocolCategoryStatusId))]
        public virtual ProtocolCategoryStatus ProtocolCategoryStatus { get; set; }
    }
}


