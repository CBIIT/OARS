
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheradexPortal.Data.Models
{
    [Table("ProtocolMapping", Schema = "DMU")]
    public class ProtocolMapping
    {
        [Column("Protocol_Mapping_Id"), Key]
        public int ProtocolMappingId { get; set; }
        [Column("Profile_Id")]
        public int? ProfileId { get; set; }
        [Column("THOR_Study_Id")]
        public string? THORStudyId { get; set; }
        [ForeignKey(nameof(THORStudyId))]
        public virtual Protocol? Protocol { get; set; }
        [Column("Mapping_Version")]
        public int? MappingVersion { get; set; }
        [Column("Source_Protocol_Mapping_Id")]
        public int? SourceProtocolMappingId { get; set; }
        [Column("Is_Published")]
        public bool IsPublished { get; set; } = false;
        [Column("Protocol_Mapping_Status_Id")]
        public int? ProtocolMappingStatusId { get; set; }
        [Column("Billing_Code")]
        public string? BillingCode { get; set; }
        [Column("Title")]
        public string? ProtocolTitle { get; set; }
        [Column("Sponsor")]
        public string? Sponsor { get; set; }
        [Column("Protocol_Data_System_Id")]
        public int? ProtocolDataSystemId { get; set; }
        [Column("Date_Format")]
        public string? DateFormat { get; set; }
        [Column("Data_File_Folder")]
        public string? DataFileFolder { get; set; }
        [Column("Create_Date")]
        public DateTime? CreateDate { get; set; }
        [ForeignKey(nameof(ProtocolMappingStatusId))]
        public ProtocolMappingStatus? Status { get; set; }

        [ForeignKey(nameof(ProfileId))]
        public Profile? Profile { get; set; }

    }
}
