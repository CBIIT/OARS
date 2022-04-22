namespace TheradexPortal.Data.PowerBI.Models
{
    public class PowerBI
    {
        public IDictionary<string, ReportConfig> Reports { get; set; }
        
        public class ReportConfig
        {
            // Workspace Id for which Embed token needs to be generated
            public string WorkspaceId { get; set; }

            // Report Id for which Embed token needs to be generated
            public string ReportId { get; set; }

            public bool UseRowLevelSecurity { get; set; } = true;

            public string[]? IdentityRoles { get; set; }
        }
    }
}
