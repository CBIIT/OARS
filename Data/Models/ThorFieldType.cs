using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheradexPortal.Data.Models
{
    [Table("THORFIELDTYPE", Schema = "DMU")]
    public class ThorFieldType
    {
        [Column("THOR_FIELD_TYPE_ID"), Key]
        public int ThorFieldTypeId { get; set; }
        [Column("FIELD_TYPE_NAME")]
        public string? FieldTypeName { get; set; }
        [Column("IS_ACTIVE")]
        public bool IsActive { get; set; }
        [Column("CREATE_DATE")]
        public DateTime? CreateDate { get; set; }
        [Column("UPDATE_DATE")]
        public DateTime? UpdateDate { get; set; }
    }
}


