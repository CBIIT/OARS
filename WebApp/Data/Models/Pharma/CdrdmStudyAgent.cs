using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OARS.Data.Models.Pharma
{
    [Table("CDRDM_STUDY_AGENTS", Schema = "THORDB")]
    public class CdrdmStudyAgent
    {
        [Column("CDR_STUDY_ID")]
        public int CdrStudyId { get; set; }

        [Column("DOCUMENT_NUMBER")]
        [StringLength(128)]
        public string DocumentNumber { get; set; }

        [Column("DOCUMENT_TYPE")]
        [StringLength(24)]
        public string DocumentType { get; set; }

        [Column("AGENT_NAME")]
        [StringLength(75)]
        public string AgentName { get; set; }

        [Column("NSC")]
        [StringLength(8)]
        public string Nsc { get; set; }

        [Column("LEAD_AGENT")]
        [StringLength(8)]
        public string LeadAgent { get; set; }

        [Column("STUDY_AGENT_STATUS")]
        [StringLength(64)]
        public string StudyAgentStatus { get; set; }

        [Column("STUDY_AGENT_STATUS_DATE")]
        public DateTime? StudyAgentStatusDate { get; set; }

        [Column("COMMERCIAL_INVEST_FLAG")]
        [StringLength(24)]
        public string CommercialInvestFlag { get; set; }

        [Column("PMB_DISTRIBUTED")]
        [StringLength(3)]
        public string PmbDistributed { get; set; }

        [Column("ASSOC_IND_NUMBER")]
        [StringLength(4000)]
        public string? AssocIndNumber { get; set; }

        [Column("DATE_RECORD_LAST_MODIFIED")]
        public DateTime? DateRecordLastModified { get; set; }

        [Column("IDB_DRUG_MONITOR")]
        [StringLength(100)]
        public string? IdbDrugMonitor { get; set; }

        [Column("ACCOUNT_NAME")]
        [StringLength(30)]
        public string? AccountName { get; set; }

        [Column("PRIMARY_CONTACT_EMAIL")]
        [StringLength(2000)]
        public string? PrimaryContactEmail { get; set; }

        [Column("PRIMARY_CONTACT_PHONE")]
        [StringLength(1000)]
        public string? PrimaryContactPhone { get; set; }
    }
}
