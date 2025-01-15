namespace TheradexPortal.Data.Models.DTO
{
    public class ReviewPiDTO
    {
        public string PiName { get; set; }
        public string caseNumber {  get; set; }
        public string dueDate { get; set; }
        public DateTime? updateDate { get; set; }
        public string currentStatus { get; set; }
        public string periodName { get; set; }
    }
}
