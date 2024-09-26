using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheradexPortal.Data.Models
{
    [Table("THORDICTIONARY", Schema = "DMU")]
    public class ThorDictionary
    {
        [Column("THOR_DICTIONARY_ID"), Key]
        public int ThorDictionaryId { get; set; }

        [Column("DICTIONARY_NAME")]
        public string? DictionaryName { get; set; }

        [Column("DICTIONARY_OPTION")]
        public string? DictionaryOption { get; set; }

        [Column("DICTONARY_VALUE")]
        public string? DictionaryValue { get; set; }

        [Column("SORT_ORDER")]
        public int? SortOrder { get; set; }

        [Column("IS_ACTIVE")]
        public bool IsActive { get; set; }

        [Column("CREATE_DATE")]
        public DateTime? CreateDate { get; set; }

        [Column("UPDATE_DATE")]
        public DateTime? UpdateDate { get; set; }

        // Display fields
        [NotMapped]
        public string? ThorDictionaryDisplay
        {
            get
            {
                return DictionaryOption + " - " + DictionaryValue;
            }
        }
    }
}

