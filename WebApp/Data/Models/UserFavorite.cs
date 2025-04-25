using Org.BouncyCastle.Bcpg.OpenPgp;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OARS.Data.Models
{
    [Table("USER_FAVORITE", Schema = "THOR_USER")]
    public class UserFavorite
    {
        [Key]
        public int UserFavoriteId { get; set; }
        public int? UserId { get; set; }
        public int? DashboardId { get; set; }
        public int? ReportId { get; set; }
        [Column("Create_Date")]
        public DateTime? CreateDate { get; set; }
        [Column("Update_Date")]
        public DateTime? UpdateDate { get; set; }
        public string? ReportName { get; set; }

        [Column("Display_Icon_Name")]
        public string? DisplayIconName { get; set; }
    }

    public class FavoriteReportItem
    {
        public int? DashboardId { get; set; }
        public int? ReportId { get; set; }
        public string? ReportName { get; set; }
        public string? DisplayName { get; set; }
        public bool isDashboard { get; set; }
        public List<Report> ReportList { get; set; }

        public List<int> UserFavoriteIdList { get; set; }

    }
}
