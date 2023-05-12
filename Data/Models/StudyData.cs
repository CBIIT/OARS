namespace TheradexPortal.Data.Models
{
    public class StudyData
    {
        public List<Study> StudyList { get; set; }
        public class Study
        {
            // study id
            public string StudyId { get; set; }

            // primary agent
            public string PrimaryAgent { get; set; }

            // secondary agent
            public string SecondaryAgent { get; set; }

            // other
            public string Other { get; set; }

            // title (study title)
            public string Title { get; set; }

            // Overview fields:

            // Protocol
            public string Protocol { get; set; }

            // Protocol Title
            public string ProtocolTitle { get; set; }

            // Protocol Status
            public string ProtocolStatus { get; set; }

            // Primary Investigator
            public string PrimaryInvestigator { get; set; }

            // Monitoring Method
            public string MonitoringMethod { get; set; }

            // Accrual (Screening/Intervention/Other)
            public string Accrual { get; set; }

            // Date of Last EDC Update
            public DateTime DateOfLastEDCUpdate { get; set; }

            // Total Patients in Web Reporting
            public int totalPatients { get; set; }

            // Recent Enrollment in Web Reporting
            public int recentEnrollment { get; set; }

            // Subsequent Phase Activation Date
            public DateTime SubsequentPhaseActivationDate { get; set; }
        }
    }
}
