using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheradexPortal.Data.Models.Pharma
{
    [Table("PROTOCOL_TAC", Schema = "THORDB")]
    public class ProtocolTac
    {
        [Key]
        [Column("STUDY_ID")]
        [Required]
        [MaxLength(50)]
        public string StudyId { get; set; }

        [Key]
        [Column("TRT_ASGNMT_CODE")]
        [Required]
        [MaxLength(500)]
        public string TrtAsgnmtCode { get; set; }

        [Column("TRT_ASGNMT_DESCRIPTION")]
        [MaxLength(4000)]
        public string TrtAsgnmtDescription { get; set; }

        [Column("CREATED")]
        public DateTime Created { get; set; } = DateTime.UtcNow;

        [Column("UPDATED")]
        public DateTime? Updated { get; set; }

        [Column("DELETED")]
        public DateTime? Deleted { get; set; }

        [Column("ISACTIVE")]
        [MaxLength(1)]
        public string IsActive { get; set; } = "N";

        [Column("ISDELETED")]
        [MaxLength(1)]
        public string IsDeleted { get; set; } = "N";
    }
}