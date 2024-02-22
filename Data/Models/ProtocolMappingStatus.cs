using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheradexPortal.Data.Models
{
    public class ProtocolMappingStatus
    {
        [Column("Protocol_Mapping_Status_Id"), Key]
        public int ProtocolMappingStatusId { get; set; }
        [Column("Status_Name")]
        public string StatusName { get; set; }
        [Column("Is_Active")]
        public bool IsActive { get; set; }
        [Column("Create_Date")]
        public DateTime? CreateDate { get; set; }
        [Column("Update_Date")]
        public DateTime? UpdateDate { get; set; }
    }
}
