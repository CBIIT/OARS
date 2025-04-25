using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using OARS.Data.Static;

namespace OARS.Data.Models
{
    public class AuditEntry
    {
        public AuditEntry(EntityEntry entry)
        {
            Entry = entry;
        }
        public EntityEntry Entry { get; }
        public int? UserId { get; set; }
        public string TableName { get; set; }
        public Dictionary<string, object> KeyValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> OldValues { get; } = new Dictionary<string, object>();
        public Dictionary<string, object> NewValues { get; } = new Dictionary<string, object>();
        public List<PropertyEntry> TemporaryProperties { get; } = new List<PropertyEntry>();
        public bool HasTemporaryProperties => TemporaryProperties.Any();
        public AuditType AuditType { get; set; }
        public List<string> ChangedColumns { get; } = new List<string>();
        public DateTime CreateDate { get; set; }
        public bool IsPrimaryTable { get; set; }
        public Audit ToAudit()
        {
            var audit = new Audit();
            audit.UserId = UserId;
            audit.AuditType = AuditType.ToString();
            audit.TableName = TableName;
            audit.CreateDate = CreateDate;
            audit.PrimaryKey = JsonConvert.SerializeObject(KeyValues);
            audit.OldValues = OldValues.Count == 0 ? null : JsonConvert.SerializeObject(OldValues);
            audit.NewValues = NewValues.Count == 0 ? null : JsonConvert.SerializeObject(NewValues);
            audit.AffectedColumns = ChangedColumns.Count == 0 ? null : JsonConvert.SerializeObject(ChangedColumns);
            audit.IsPrimaryTable = IsPrimaryTable;
            return audit;
        }
    }
}
