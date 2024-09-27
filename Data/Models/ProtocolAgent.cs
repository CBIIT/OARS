using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheradexPortal.Data.Models
{
    [Table("PROTOCOLAGENT", Schema = "DMU")]
    public class ProtocolAgent
    {
        [Key, Column("Protocol_Agent_Id")]
        public int ProtocolAgentId { get; set; }
        [Column("PROTOCOL_MAPPING_ID")]
        public int ProtocolMappingId { get; set; }
        [Column("NSC_NUMBER")]
        public string NscNumber { get; set; }
        [Column("AGENT_NAME")]
        public string AgentName { get; set; }
        [Column("CREATE_DATE")]
        public DateTime CreateDate { get; set; }
        [Column("UPDATE_DATE")]
        public DateTime UpdateDate { get; set; }
    }
}


