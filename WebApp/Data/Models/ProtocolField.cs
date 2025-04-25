using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OARS.Data.Models
{
    [Table("PROTOCOLFIELD", Schema = "DMU")]
    public class ProtocolField
    {
        [Key, Column("Protocol_Field_Id")]
        public int ProtocolFieldId { get; set; }
        [Column("PROTOCOL_MAPPING_ID")]
        public int ProtocolMappingId { get; set; }
        [Column("THOR_FIELD_ID")]
        public string ThorFieldId { get; set; }
        [Column("FORMAT")]
        public string Format { get; set; }
        [Column("IS_REQUIRED")]
        public bool IsRequired { get; set; }
        [Column("IS_ENABLED")]
        public bool IsEnabled { get; set; }
        [Column("CAN_BE_DICTIONARY")]
        public bool CanBeDictionary { get; set; }
        [Column("IS_MULTI_FORM")]
        public bool IsMultiForm { get; set; }
        [Column("CREATE_DATE")]
        public DateTime CreateDate { get; set; }
        [Column("UPDATED_DATE")]
        public DateTime UpdateDate { get; set; }
        [ForeignKey(nameof(ProtocolMappingId))]
        public ProtocolMapping ProtocolMapping { get; set; }
        [ForeignKey(nameof(ThorFieldId))]
        public ThorField ThorField { get; set; }

        [NotMapped]
        public string ThorDataCategoryId { get; set; }

        [NotMapped]
        public string ThorDataCategoryDisplay
        {
            get
            {
                return ThorField?.Category?.CategoryDisplay ?? "";
            }
        }

        [NotMapped]
        public string ThorFieldDisplay
        {
            get
            {
                return ThorField?.FieldDisplay ?? "";
            }
        }
    }
}


