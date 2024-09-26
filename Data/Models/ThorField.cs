
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheradexPortal.Data.Models
{
    [Table("THORFIELD", Schema = "DMU")]
    public class ThorField
    {
        [Column("THOR_DATA_CATEGORY_ID")]
        public string? ThorDataCategoryId { get; set; }
        [ForeignKey(nameof(ThorDataCategoryId))]
        public ThorCategory? Category { get; set; }
        [NotMapped]
        public string CategoryDisplay
        {
            get
            {
                if (Category == null)
                    return "";
                return Category.CategoryDisplay;
            }
        }
        [Column("THOR_FIELD_ID"), Key]
        public string ThorFieldId { get; set; }
        [Column("FIELD_LABEL")]
        public string? FieldLabel { get; set; }
        [NotMapped]
        public string FieldDisplay
        {
            get
            {
                return ThorFieldId + " - " + FieldLabel;
            }
        }
        [ForeignKey(nameof(ThorFieldTypeId))]
        public ThorFieldType? FieldType { get; set; }

        [Column("THOR_FIELD_TYPE")]
        public int? ThorFieldTypeId { get; set; }
        public string FieldTypeName
        {
            get
            {
                return FieldType?.FieldTypeName??"";
            }
        }

        [Column("IS_DERIVABLE")]
        public bool Derivable { get; set; }
        [ForeignKey(nameof(ThorDictionaryId))]
        public ThorDictionary? Dictionary { get; set; }
        [Column("THOR_DICTIONARY_ID")]
        public int? ThorDictionaryId { get; set; }
        public string DictionaryName
        {
            get
            {
                return Dictionary?.DictionaryName??"";
            }
        }

        [Column("SORT_ORDER")]
        public int? SortOrder { get; set; }
        [Column("IS_MULTI_FORM")]
        public bool IsMultiForm { get; set; }
        [Column("IS_ACTIVE")]
        public bool IsActive { get; set; }
        [Column("CREATE_DATE")]
        public DateTime? CreateDate { get; set; }
        [Column("UPDATE_DATE")]
        public DateTime? UpdateDate { get; set; }

    }
}


