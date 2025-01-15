namespace TheradexPortal.Data.Models.DTO
{
    public class AuditTrailDTO
    {
        public string userName { get; set; }
        public int userId {  get; set; }
        public DateTime dateOfChange { get; set; }
        public string typeOfChange { get; set; }
        public string changeField { get; set; }
        public string previousValue { get; set; }
        public string newValue { get; set; }
    }
}
