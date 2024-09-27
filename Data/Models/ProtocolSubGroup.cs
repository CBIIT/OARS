using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheradexPortal.Data.Models
{
    [Table("PROTOCOLSUBGROUP", Schema = "DMU")]
    public class ProtocolSubGroup
    {
        [Key, Column("Protocol_Sub_Group_Id")]
        public int ProtocolSubGroupId { get; set; }
        [Column("PROTOCOL_MAPPING_ID")]
        public int ProtocolMappingId { get; set; }
        [Column("SUB_GROUP_CODE")]
        public string? SubGroupCode { get; set; }
        [Column("DESCRIPTION")]
        public string? Description { get; set; }
        [Column("CREATE_DATE")]
        public DateTime CreateDate { get; set; }
        [Column("UPDATE_DATE")]
        public DateTime UpdateDate { get; set; }
    }
}


