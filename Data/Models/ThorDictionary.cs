using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheradexPortal.Data.Models
{
    [Table("THORDictionary", Schema = "DMU")]
    public class ThorDictionary
    {
        [Column("THOR_Dictionary_Id"), Key]
        public int ThorDictionaryId { get; set; }

        [Column("Dictionary_Name")]
        public string? DictionaryName { get; set; }

        [Column("Dictionary_Option")]
        public string? DictionaryOption { get; set; }

        [Column("Dictonary_Value")]
        public string? DictionaryValue { get; set; }

        [Column("Sort_Order")]
        public int? SortOrder { get; set; }

        [Column("Is_Active")]
        public bool IsActive { get; set; }

        [Column("Create_Date")]
        public DateTime? CreateDate { get; set; }

        [Column("Update_Date")]
        public DateTime? UpdateDate { get; set; }
    }
}