using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheradexPortal.Data.Models
{
    [Table("ERRORLOG", Schema = "THOR_USER")]
    public class ErrorLog
    {
        [Key]
        [Column("ErrorLogId")]
        public int ErrorLogId { get; set; }
        [Column("Create_Date")]
        public DateTime? CreateDate { get; set; }
        public int UserId { get; set; }
        public string? Url { get; set; }
        public string? Source { get; set; }
        public string? Message {  get; set; }
        public string? InnerException { get; set; }
        public string? StackTrace { get; set; }
    }
}
