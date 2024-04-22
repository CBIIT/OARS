using System.ComponentModel.DataAnnotations.Schema;

namespace TheradexPortal.Data.Models
{
    [Table("ProtocolCategoryStatus", Schema = "DMU")]
    public class ProtocolCategoryStatus
    {
        [Column("Protocol_Category_Status_Id")]
        public int ProtocolCategoryStatusId { get; set; }
        [Column("Category_Status_Name")]
        public string CategoryStatusName { get; set; }
        [Column("Is_Active")]
        public bool IsActive { get; set; }
        [Column("Create_Date")]
        public DateTime CreateDate { get; set; }
        [Column("Update_Date")]
        public DateTime UpdateDate { get; set; }
    }
}
