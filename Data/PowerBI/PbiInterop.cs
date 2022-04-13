namespace TheradexPortal.Data.PowerBI
{
    using Microsoft.AspNetCore.Components;
    using Microsoft.Extensions.Options;
    using Microsoft.JSInterop;
    using TheradexPortal.Data.PowerBI.Models;

    public class PbiInterop
    {
        private readonly IJSRuntime js;
        private readonly PbiEmbedService pbiEmbedService;
        private readonly IOptions<PowerBI> powerBiConfig;

        public PbiInterop(
            IJSRuntime js,
            PbiEmbedService pbiEmbedService,
            IOptions<PowerBI> powerBiConfig)
        {
            this.js = js;
            this.pbiEmbedService = pbiEmbedService;
            this.powerBiConfig = powerBiConfig;
        }

        /// <summary>
        /// Embeds PowerBI report using Javascript SDK in a provided element container
        /// </summary>
        /// <param name="reportName">Configured report name from appsettings PowerBi.Reports</param>
        /// <param name="reportContainer">Reference to embedded report container</param>
        /// <returns></returns>
        public async ValueTask EmbedReportJS(string reportName, ElementReference reportContainer)
        {
            var reportConfig = powerBiConfig.Value.Reports.FirstOrDefault(r => r.Key == reportName).Value;
            if(reportConfig == null)
                throw new ArgumentException($"Could not find configured report ${reportName}");

            var embedParams = pbiEmbedService.GetEmbedParams(new Guid(reportConfig.WorkspaceId), new Guid(reportConfig.ReportId));

            await js.InvokeVoidAsync(
                "PowerBIEmbed.showReport",
                reportContainer,
                embedParams.EmbedToken.Token,
                embedParams.EmbedReport[0].EmbedUrl,
                embedParams.EmbedReport[0].ReportId.ToString()
            );
        }
    }
}
