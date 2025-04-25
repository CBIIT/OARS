using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OARS.Data.Models
{
    [Table("PROTOCOLDISEASE", Schema = "DMU")]
    public class ProtocolDisease
    {
        [Key, Column("Protocol_Disease_Id")]
        public int ProtocolDiseaseId { get; set; }
        
        [Column("PROTOCOL_MAPPING_ID")]
        public int? ProtocolMappingId { get; set; }

        [Column("MEDDRA_CODE")]
        public string? MeddraCode { get; set; }
        
        [Column("DISEASE_NAME")]
        public string? DiseaseName { get; set; }
        
        [Column("CREATE_DATE")]
        public DateTime CreateDate { get; set; }
        
        [Column("UPDATE_DATE")]
        public DateTime UpdatedDate { get; set; }
        
        [ForeignKey(nameof(ProtocolMappingId))]
        public ProtocolMapping ProtocolMapping { get; set; }
    }
}


