namespace OARS.Data.PowerBI.Models
{
    using Microsoft.PowerBI.Api.Models;
    using System.Collections.Generic;

    public class EmbedParams
    {
        // Type of the object to be embedded
        public string Type { get; set; }

        // Report to be embedded
        public List<EmbedReport> EmbedReport { get; set; }

        // Embed Token for the Power BI report
        public EmbedToken EmbedToken { get; set; }

        public string Settings { get; set; }
    }
}
