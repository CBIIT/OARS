using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TheradexPortal.Data.Models
{
    [Table("WRAUDIT", Schema = "WRUSER")]
    public class Audit
    {
        [Key]
        public int WRAuditId { get; set; }
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
