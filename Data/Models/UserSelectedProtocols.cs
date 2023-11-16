using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheradexPortal.Data.Models
{
    [Table("WRSELECTED_PROTOCOLS", Schema = "WRUSER")]
    public class UserSelectedProtocols
    {
        [Key]
        public int WRSelected_Protocol_Id { get; set; }
        public int? UserId { get; set; }
        public string? Selected_Protocols { get; set; }
        public string? Current_Protocols { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
