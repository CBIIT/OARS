using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheradexPortal.Data.Models
{
    [Table("PROTOCOLMAPPINGSTATUS", Schema = "DMU")]
    public class ProtocolMappingStatus
    {
        [Column("PROTOCOL_MAPPING_STATUS_ID"), Key]
        public int ProtocolMappingStatusId { get; set; }
        [Column("STATUS_NAME")]
        public string StatusName { get; set; }
        [Column("IS_ACTIVE")]
        public bool IsActive { get; set; }
        [Column("CREATE_DATE")]
        public DateTime? CreateDate { get; set; }
        [Column("UPDATE_DATE")]
        public DateTime? UpdateDate { get; set; }
    }
}


