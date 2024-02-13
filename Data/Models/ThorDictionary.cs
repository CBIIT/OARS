namespace TheradexPortal.Data.Models
{
    public class ThorDictionary
    {
        public int ThorDictionaryId { get; set; }
        public string? DictionaryName { get; set; }
        public string? DictionaryOption { get; set; }
        public string? DictionaryValue { get; set; }
        public int? SortOrder { get; set; }
        public bool IsActive { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
