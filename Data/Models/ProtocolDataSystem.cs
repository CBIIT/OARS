using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheradexPortal.Data.Models
{
    public class ProtocolDataSystem
    {
        [Column("Protocol_Data_System_ID"), Key]
        public int ProtocolDataSystemID { get; set; }
        [Column("Data_System_Name")]
        public string ProtocolDataSystemName { get; set; }
        [Column("Sort_Order")]
        public int SortOrder { get; set; }
        [Column("Is_Active")]
        public bool IsActive { get; set; }
        [Column("Create_Date")]
        public DateTime? CreateDate { get; set; }
        [Column("Update_Date")]
        public DateTime? UpdateDate { get; set; }

    }
}
