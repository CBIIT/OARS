using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TheradexPortal.Data.Models
{
    [Table("AUDIT", Schema = "THOR_USER")]
    public class Audit
    {
        [Key]
        public int AuditId { get; set; }
        public int? UserId { get; set; }
        public DateTime CreateDate { get; set; }
        public string AuditType { get; set; }
        public string TableName { get; set; }
        public string AffectedColumns {  get; set; }
        public string OldValues { get; set; }
        public string NewValues { get; set; }
        public string PrimaryKey { get; set; }
        public bool IsPrimaryTable {  get; set; }
    }
}
