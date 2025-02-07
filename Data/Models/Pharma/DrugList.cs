using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheradexPortal.Data.Models.Pharma
{
    [Table("DRUG_LIST", Schema = "MRATHI")]
    public class DrugList
    {
        [Key]
        [Column("DRUGID")]
        [Required]
        [MaxLength(50)]
        public string DrugId { get; set; }

        [Key]
        [Column("NSC_NUMBER")]
        [Required]
        [MaxLength(50)]
        public string Nsc { get; set; }

        [Column("FULL_NAME")]
        [Required]
        [MaxLength(4000)]
        public string FullName { get; set; }

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