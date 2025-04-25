using CsvHelper.Configuration.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace OARS.Data.Models
{
    [Table("PROTOCOLEDCFIELD", Schema = "DMU")]
    public class ProtocolEDCField
    {
        [Column("PROTOCOL_EDC_FIELD_ID")]
        public int ProtocolEDCFieldId { get; set; }
        [Column("PROTOCOL_EDC_FORM_ID")]
        public int ProtocolEDCFormId { get; set; } = 0;

        [NotMapped]
        public string ProtocolEDCFormName
        {
            get
            {
                if (ProtocolEDCForm == null)
                    return "";
                return ProtocolEDCForm.EDCFormName ?? "";
            }
        }

        [NotMapped]
        public string ProtocolEDCFormDisplay
        {
            get
            {
                if (ProtocolEDCForm == null)
                    return "";
                return ProtocolEDCForm.EDCFormDisplay;
            }
        }

        [Column("EDC_FIELD_IDENTIFIER")]
        public string? EDCFieldIdentifier { get; set; }
        [Column("EDC_FIELD_NAME")]
        public string? EDCFieldName { get; set; }
        [NotMapped]
        public string EDCFieldDisplay
        {
            get
            {
                return EDCFieldIdentifier + " - " + EDCFieldName;
            }
        }

        [Column("EDC_DICTIONARY_NAME")]
        public string? EDCDictionaryName { get; set; }
        [Column("CREATE_DATE")]
        public DateTime CreateDate { get; set; }
        [Column("UPDATE_DATE")]
        public DateTime UpdateDate { get; set; }

        [ForeignKey(nameof(ProtocolEDCFormId))]
        public ProtocolEDCForm ProtocolEDCForm { get; set; }
    }
}


