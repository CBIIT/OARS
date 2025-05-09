﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace OARS.Data.Models
{
    [Table("REPORT", Schema = "THOR_USER")]
    public class Report
    {
        [Key]
        public int ReportId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int DashboardId { get; set; }
        [Column("Display_Order")]
        public int DisplayOrder { get; set; }
        [Column("Display_Icon_Name")]
        public string? DisplayIconName { get; set; }
        [Column("Create_Date")]
        public DateTime? CreateDate { get; set; }
        [Column("Update_Date")]
        public DateTime? UpdateDate { get; set; }
        [Column("Is_Full_Page")]
        public bool IsFullPage { get; set; }
        [Column("Custom_Page_Path")]
        public string? CustomPagePath { get; set; }
        [Column("PowerBI_Page_Name")]
        public string? PowerBIPageName { get; set; }
        [Column("SubMenu_Name")]
        public string? SubMenuName { get; set; }
        [Column("PowerBI_Report_Id")]
        public string? PowerBIReportId { get; set; }
        public string? PageName { get; set; }
        public string? ReportName { get; set; }
        [Column("Report_Filter_Id")]
        public int? ReportFilterId { get; set; }
        public Dashboard? Dashboard { get; init; }

    }
}
