namespace TheradexPortal.Data.PowerBI.Models
{
    using System;

    public class EmbedVisual
    {
        // Name of the visual
        public string VisualName { get; set; }

        // Name of the page on which the visual resides
        public string PageName { get; set; }

        // Id of Power BI report to be embedded
        public Guid ReportId { get; set; }

        // Embed URL for the Power BI report
        public string EmbedUrl { get; set; }
    }
}
