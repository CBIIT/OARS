namespace TheradexPortal.Data.Services
{
    using TheradexPortal.Data.ViewModels;

    public class StudyService
    {
        public StudyData GetStudyData()
        {
            var studyData = new StudyData();
            studyData.StudyList = new List<StudyData.Study>();
            var study = new StudyData.Study();

            //example study data
            study.StudyId = "1";
            study.PrimaryAgent = "Primary Agent";
            study.SecondaryAgent = "Secondary Agent";
            study.Other = "Other";
            study.Title = "Title";
            study.Protocol = "Protocol";
            study.ProtocolTitle = "Protocol Title";
            study.ProtocolStatus = "Protocol Status";
            study.PrimaryInvestigator = "Primary Investigator";
            study.MonitoringMethod = "Monitoring Method";
            study.Accrual = "Accrual";
            study.DateOfLastEDCUpdate = new DateTime(2020, 1, 1);
            study.totalPatients = 100;
            study.recentEnrollment = 10;
            study.SubsequentPhaseActivationDate = new DateTime(2020, 1, 1);
            studyData.StudyList.Add(study);

            return studyData;
        }

        public StudyData.Study getStudy()
        {
            return GetStudyData().StudyList[0];
        }
    }
}