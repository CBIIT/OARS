using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheradexPortal.Data.Models.Pharma
{
    [Table("PHARMA_NSC_TAC", Schema = "THORDB")]
    public class PharmaNscTac
    {
        [Key]
        public int Id { get; set; }

        [Column("PROTOCOL_NUMBER")]
        public string? ProtocolNumber { get; set; }

        [Column("TRT_ASGNMT_CODE")]
        public string? TrtAsgnmtCode { get; set; }

        [Column("TRT_ASGNMT_DESCRIPTION")]
        public string? TrtAsgnmtDescription { get; set; }

        [Column("NSC")]
        public string? Nsc { get; set; }

        [Column("AGREEMENT_NUMBER")]
        public string? AgreementNumber { get; set; }

        [Column("CREATED")]
        public DateTime Created { get; set; }
        [Column("UPDATED")]
        public DateTime? Updated { get; set; }
        [Column("DELETED")]
        public DateTime? Deleted { get; set; }
        [Column("ISACTIVE")]
        public bool IsActive { get; set; }
        [Column("ISDELETED")]
        public bool IsDeleted{ get; set; }
    }
}
