using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TheradexPortal.Data.Models
{
    [Table("USER", Schema = "THOR_USER")]
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? EmailAddress { get; set; }
        public string? CtepUserId { get; set; }
        public string? Title { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public Boolean IsActive { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public bool IsCtepUser { get; set; }
        public bool AllStudies { get;set;}
        public string? TimeZoneAbbreviation { get; set; }
        public int? TimeOffset { get; set; }
        public ICollection<UserRole> UserRoles { get; } = new List<UserRole>();
        public ICollection<UserProtocol> UserProtocols { get; } = new List<UserProtocol>();
        public ICollection<UserGroup> UserGroups { get; } = new List<UserGroup>();
    }
}
