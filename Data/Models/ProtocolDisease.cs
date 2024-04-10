using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheradexPortal.Data.Models
{
    [Table("ProtocolDisease", Schema = "DMU")]
    public class ProtocolDisease
    {
        [Key, Column("Protocol_Disease_Id")]
        public int ProtocolDiseaseId { get; set; }
        
        [Column("Protocol_Mapping_Id")]
        public int? ProtocolMappingId { get; set; }

        [Column("Meddra_Code")]
        public string? MeddraCode { get; set; }
        
        [Column("Disease_Name")]
        public string? DiseaseName { get; set; }
        
        [Column("Create_Date")]
        public DateTime CreateDate { get; set; }
        
        [Column("Update_Date")]
        public DateTime UpdatedDate { get; set; }
        
        [ForeignKey(nameof(ProtocolMappingId))]
        public ProtocolMapping ProtocolMapping { get; set; }
    }
}
