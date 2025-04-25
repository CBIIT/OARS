
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OARS.Data.Models
{
    [Table("PROTOCOLMAPPING", Schema = "DMU")]
    public class ProtocolMapping
    {
        [Column("PROTOCOL_MAPPING_ID"), Key]
        public int ProtocolMappingId { get; set; }
        [Column("PROFILE_ID")]
        public int? ProfileId { get; set; }
        [Column("THOR_STUDY_ID")]
        public string? THORStudyId { get; set; }
        [ForeignKey(nameof(THORStudyId))]
        public virtual Protocol? Protocol { get; set; }
        [Column("MAPPING_VERSION")]
        public int? MappingVersion { get; set; }
        [Column("SOURCE_PROTOCOL_MAPPING_ID")]
        public int? SourceProtocolMappingId { get; set; }
        [Column("PROTOCOL_MAPPING_STATUS_ID")]
        public int? ProtocolMappingStatusId { get; set; }
        [Column("BILLING_CODE")]
        public string? BillingCode { get; set; }
        [Column("TITLE")]
        public string? ProtocolTitle { get; set; }
        [Column("SPONSOR")]
        public string? Sponsor { get; set; }
        [Column("PROTOCOL_DATA_SYSTEM_ID")]
        public int? ProtocolDataSystemId { get; set; }
        [Column("DATE_FORMAT")]
        public string? DateFormat { get; set; }
        [Column("DATA_FILE_FOLDER")]
        public string? DataFileFolder { get; set; }
        [Column("CREATE_DATE")]
        public DateTime? CreateDate { get; set; }
        [Column("IS_PUBLISHED")]
        public bool IsPublished { get; set; }
        [Column("PROTOCOL_CROSSOVER_OPTION_ID")]
        public int? ProtocolCrossoverOptionId { get; set; }

        [ForeignKey(nameof(ProtocolMappingStatusId))]
        public ProtocolMappingStatus? Status { get; set; }

        [ForeignKey(nameof(ProfileId))]
        public virtual Profile? Profile { get; set; }

    }
}


