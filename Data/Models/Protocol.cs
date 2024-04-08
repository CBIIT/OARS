using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheradexPortal.Data.Models
{
    [Table("PROTOCOL", Schema = "THORDB")]
    [Keyless]
    public class Protocol
    {
        //unique index
        [Column("Study_Id")]
        public string? StudyId { get; set; } 

        public string? Institution { get; set; }

        public string? Nickname { get; set; }
        [Column("CTEP_Code")]
        public string? CTEPCode { get; set; }

        [Column("Lead_Organization")]
        public string? LeadOrganization { get; set; }

        [Column("Multi_Institute")]
        public string? MultiInstitute { get; set; }

        [Column("Local_Protocol_Number")]
        public string? LocalProtocolNumber { get; set; }

        [Column("Processing_Batch")]
        public string? ProcessingBatch { get; set; }

        [Column("Protocol_Title")]
        public string? ProtocolTitle { get; set; }

        public string? Investigator { get; set; }

        [Column("Study_Drug")]
        public string? StudyDrug { get; set; }

        [Column("Study_Drug_ID")]
        public string? StudyDrugID { get; set; }

        public string? Phase { get; set; }

        [Column("Protocol_Treatment_Desc")]
        public string? ProtocolTreatmentDesc { get; set; }

        [Column("Performance_Status_Scale")]
        public string? PerformanceStatusScale { get; set; }

        [Column("Toxicity_Scale")]
        public string? ToxicityScale { get; set; }

        [Column("Toxicity_Dictionary_ID")]
        public string? ToxicityDictionaryID { get; set; }

        [Column("Database_Generation")]
        public string? DatabaseGeneration { get; set; }

        [Column("Data_Source")]
        public string? DataSource { get; set; }

        [Column("Monitoring_Method")]
        public string? MonitoringMethod { get; set; }

        public string? CDASH { get; set; }

        public string? STS { get; set; }

        [Column("Informed_Consent")]
        public string? InformedConsent { get; set; }

        [Column("IRB_Approval_DT")]
        public DateTime? IRBApprovalDT { get; set; }

        [Column("NCI_Approval_DT")]
        public DateTime? NCIApprovalDT { get; set; }

        [Column("Activation_DT")]
        public DateTime? ActivationDT { get; set; }

        [Column("Closed_To_Accrual_DT")]
        public DateTime? ClosedToAccrualDT { get; set; }

        [Column("All_Pts_Off_Study_DT")]
        public DateTime? AllPtsOffStudyDT { get; set; }

        [Column("Completed_DT")]
        public DateTime? CompletedDT { get; set; }

        [Column("Archived_DT")]
        public DateTime? ArchivedDT { get; set; }

        [Column("Disease_1")]
        public string? Disease1 { get; set; }

        [Column("Disease_2")]
        public string? Disease2 { get; set; }

        [Column("Disease_3")]
        public string? Disease3 { get; set; }

        [Column("Lab_Units")]
        public string? LabUnits { get; set; }

        [Column("CTCAE_Version_Lab_Toxes")]
        public string? CTCAEVersionLabToxes { get; set; }

        [Column("Solid_Leukemia_Both")]
        public string? SolidLeukemiaBoth { get; set; }

        [Column("Follow_Up_Period")]
        public string? FollowUpPeriod { get; set; }

        [Column("Disease_Code_Required")]
        public string? DiseaseCodeRequired { get; set; }

        public string? Database { get; set; }

        [Column("Current_Status")]
        public string? CurrentStatus { get; set; }

        // what does this mean now that entries are unique per study?
        [Column("Effective_DT")]
        public DateTime EffectiveDT { get; set; }

        //primary key, unique
        [Column("PR_Record_Id")]
        public string? PRRecordId { get; set; }

        [Column("Hide_Study")]
        public string? HideStudy { get; set; }

        [Column("Total_Screening_Count")]
        public string? TotalScreeningCount { get; set; }

        [Column("Total_Intervention_Count")]
        public string? TotalInterventionCount { get; set; }

        [Column("Total_Other_Count")]
        public string? TotalOtherCount { get; set; }

        [Column("Date_Of_EDC_Update")]
        public DateTime? DateOfEDCUpdate { get; set; }

        [Column("Patients_Enrolled")]
        public string? PatientsEnrolled { get; set; }

        [Column("Patients_Treated")]
        public string? PatientTreated { get; set; }

        [Column("Date_Of_Last_Registration")]
        public DateTime? DateOfLastRegistration { get; set; }

        [Column("Phase_Activation_Date")]
        public DateTime? PhaseActivationDate { get; set; }

        [Column("RecordActive")]
        public string? RecordActive { get; set; }

        [Column("Created_On")]
        public DateTime? CreatedOn { get; set; }

        [Column("Created_By")]
        public string? CreatedBy { get; set; }

        [Column("Primary_Agent")]
        public string? PrimaryAgent { get; set; }

        [Column("Other_Agents")]
        public string? OtherAgents { get; set; }

        [Column("SRC_Record_Id")]
        public string? SrcRecordId { get; set; }

        [NotMapped]
        public int RecentOrder { get; set; }
    }
}
