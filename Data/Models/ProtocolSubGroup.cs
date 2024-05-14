using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheradexPortal.Data.Models
{
    [Table("ProtocolSubGroup", Schema = "DMU")]
    public class ProtocolSubGroup
    {
        [Key, Column("Protocol_Sub_Group_Id")]
        public int ProtocolSubGroupId { get; set; }
        [Column("Protocol_Mapping_Id")]
        public int ProtocolMappingId { get; set; }
        [Column("Sub_Group_Code")]
        public string? SubGroupCode { get; set; }
        [Column("Description")]
        public string? Description { get; set; }
        [Column("Create_Date")]
        public DateTime CreateDate { get; set; }
        [Column("Update_Date")]
        public DateTime UpdateDate { get; set; }
    }
}
