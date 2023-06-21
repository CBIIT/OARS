namespace TheradexPortal.Data.PowerBI.Models
{
    public class PowerBI
    {
        public string WorkspaceId { get; set; }
        public bool UseRowLevelSecurity { get; set; } = true;

        public string[]? IdentityRoles { get; set; }
    }
}
