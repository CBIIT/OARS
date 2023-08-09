using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TheradexPortal.Data.Models
{
    [Table("WRUSER", Schema = "WRUSER")]
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
        public string? Password { get; set; }
        public string? PasswordSalt { get; set; }
        public Boolean IsActive { get; set; }
        public Boolean IsLockedOut { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public DateTime? LastPasswordChangedDate { get; set; }
        public DateTime? FailedPasswordAttemptDate { get; set; }
        public int? FailedPasswordAttemptCount { get; set; }
        public int? LoginSourceId { get; set; }
        public string? SelectedStudies { get; set; }
        public string? CurrentStudy { get; set; }
    }
}
