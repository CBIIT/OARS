using System.ComponentModel.DataAnnotations.Schema;

namespace TheradexPortal.Data.Models
{
    [Table("PROTOCOLCATEGORYSTATUS", Schema = "DMU")]
    public class ProtocolCategoryStatus
    {
        [Column("PROTOCOL_CATEGORY_STATUS_ID")]
        public int ProtocolCategoryStatusId { get; set; }
        [Column("CATEGORY_STATUS_NAME")]
        public string CategoryStatusName { get; set; }
        [Column("IS_ACTIVE")]
        public bool IsActive { get; set; }
        [Column("CREATE_DATE")]
        public DateTime CreateDate { get; set; }
        [Column("UPDATE_DATE")]
        public DateTime UpdateDate { get; set; }
    }
}


