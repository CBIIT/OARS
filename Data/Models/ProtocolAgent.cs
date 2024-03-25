using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheradexPortal.Data.Models
{
    [Table("ProtocolAgent", Schema = "DMU")]
    public class ProtocolAgent
    {
        [Key, Column("Protocol_Agent_Id")]
        public int ProtocolAgentId { get; set; }
        [Column("Protocol_Mapping_Id")]
        public int ProtocolMappingId { get; set; }
        [Column("Nsc_Number")]
        public string NscNumber { get; set; }
        [Column("Agent_Name")]
        public string AgentName { get; set; }
        [Column("Create_Date")]
        public DateTime CreateDate { get; set; }
        [Column("Update_Date")]
        public DateTime UpdateDate { get; set; }
    }
}
