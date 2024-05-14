using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheradexPortal.Data.Models
{
    [Table("THORFieldType", Schema = "DMU")]
    public class ThorFieldType
    {
        [Column("THOR_Field_Type_Id"), Key]
        public int ThorFieldTypeId { get; set; }
        [Column("Field_Type_Name")]
        public string? FieldTypeName { get; set; }
        [Column("Is_Active")]
        public bool IsActive { get; set; }
        [Column("Create_Date")]
        public DateTime? CreateDate { get; set; }
        [Column("Update_Date")]
        public DateTime? UpdateDate { get; set; }
    }
}
