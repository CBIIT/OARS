using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheradexPortal.Data.Models
{
    [Table("PROTOCOL", Schema = "WRDB")]
    [Keyless]
    public class Protocol
    {
        //not unique - can have same study id for different statuses
        public string? StudyId { get; set; } 

        public string? Institution { get; set; }

        public string? Nickname { get; set; }

        public string? CTEP_Code { get; set; }

        public string? Lead_Organization { get; set; }

        public string? Multi_Institute { get; set; }

        public string? Local_Protocol_Number { get; set; }

        public string? Processing_Batch { get; set; }

        public string? Protocol_Title { get; set; }

        public string? Investigator { get; set; }

        public string? Study_Drug { get; set; }

        public string? Study_Drug_ID { get; set; }

        public string? Phase { get; set; }

        public string? Protocol_Treatment_Desc { get; set; }

        public string? Performance_Status_Scale { get; set; }

        public string? Toxicity_Scale { get; set; }

        public string? Toxicity_Dictionary_ID { get; set; }

        public string? Database_Generation { get; set; }

        public string? Data_Source { get; set; }

        public string? Monitoring_Method { get; set; }

        public string? CDASH { get; set; }

        public string? STS { get; set; }

        public string? Informed_Consent { get; set; }

        public DateTime? IRB_Approval { get; set; }

        public DateTime? NCI_Approval_Date { get; set; }

        public DateTime? Activation_Date { get; set; }
        
        public DateTime? Closed_To_Accrual { get; set; }

        public DateTime? All_Pts_Off_Study { get; set; }

        public DateTime? Completed { get; set; }

        public DateTime? Archived { get; set; }

        public string? Disease_1 { get; set; }

        public string? Disease_2 { get; set; }

        public string? Disease_3 { get; set; }

        public string? Lab_Units { get; set; }

        public string? CTCAE_Version_Lab_Toxes { get; set; }

        public string? Solid_Leukemia_Both { get; set; }

        public string? Follow_Up_Period { get; set; }

        public string? Disease_Code_Required { get; set; }

        public string? Database { get; set; }

        public string? Current_Status { get; set; }
        
        // determines which record is most recent for a given study id
        public DateTime Effective_Date { get; set; }

        //primary key, unique
        public string? PR_Record_Id { get; set; } 
    }
}
