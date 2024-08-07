
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheradexPortal.Data.Models
{
    [Table("THORField", Schema = "DMU")]
    public class ThorField
    {
        [Column("THOR_Data_Category_Id")]
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
        [Column("THOR_Field_Id"), Key]
        public string ThorFieldId { get; set; }
        [Column("Field_Label")]
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

        [Column("THOR_Field_Type")]
        public int? ThorFieldTypeId { get; set; }
        public string FieldTypeName
        {
            get
            {
                return FieldType?.FieldTypeName??"";
            }
        }

        [Column("Is_Derivable")]
        public bool Derivable { get; set; }
        [ForeignKey(nameof(ThorDictionaryId))]
        public ThorDictionary? Dictionary { get; set; }
        [Column("THOR_Dictionary_Id")]
        public int? ThorDictionaryId { get; set; }
        public string DictionaryName
        {
            get
            {
                return Dictionary?.DictionaryName??"";
            }
        }

        [Column("Sort_Order")]
        public int? SortOrder { get; set; }
        [Column("Is_Multi_Form")]
        public bool IsMultiForm { get; set; }
        [Column("Is_Active")]
        public bool IsActive { get; set; }
        [Column("Create_Date")]
        public DateTime? CreateDate { get; set; }
        [Column("Update_Date")]
        public DateTime? UpdateDate { get; set; }

    }
}
