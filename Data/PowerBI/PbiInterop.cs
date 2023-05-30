namespace TheradexPortal.Data.PowerBI
{
    using Microsoft.AspNetCore.Components;
    using Microsoft.AspNetCore.Components.Authorization;
    using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
    using Microsoft.Extensions.FileProviders;
    using Microsoft.Extensions.Options;
    using Microsoft.Identity.Client;
    using Microsoft.JSInterop;
    using Microsoft.PowerBI.Api.Models;
    using Newtonsoft.Json.Linq;
    using System.Diagnostics.Eventing.Reader;
    using System.Security.Claims;
    using TheradexPortal.Data.PowerBI.Models;
    using static TheradexPortal.Data.PowerBI.Models.PowerBI;

    public class PbiInterop
    {
        private readonly IJSRuntime js;
        private readonly PbiEmbedService pbiEmbedService;
        private readonly IOptions<PowerBI> powerBiConfig;
        private readonly IHttpContextAccessor httpContextAccessor;
        private IJSObjectReference? module;

        public PbiInterop(
            IJSRuntime js,
            PbiEmbedService pbiEmbedService,
            IOptions<PowerBI> powerBiConfig,
            IHttpContextAccessor httpContextAccessor)
        {
            this.js = js;
            this.pbiEmbedService = pbiEmbedService;
            this.powerBiConfig = powerBiConfig;
            this.httpContextAccessor = httpContextAccessor; 
        }

        /// <summary>
        /// Embeds PowerBI report using Javascript SDK in a provided element container
        /// </summary>
        /// <param name="reportName">Configured report name from appsettings PowerBi.Reports</param>
        /// <param name="reportContainer">Reference to embedded report container</param>
        /// <returns></returns>
        public async ValueTask<IJSObjectReference> EmbedReportJS(DotNetObjectReference<TheradexPortal.Shared.PowerBiCustomLayout>? obj, string reportName, ElementReference reportContainer)
        {
            module = await js.InvokeAsync<IJSObjectReference>("import", "./js/powerbi-embed.js");
            var reportConfig = powerBiConfig.Value.Reports.FirstOrDefault(r => r.Key == reportName).Value;
            if(reportConfig == null)
                throw new ArgumentException($"Could not find configured report ${reportName}");

            var userEmail = "jbidwell@innovativesol.com"; //httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
            if(string.IsNullOrEmpty(userEmail))
                throw new ArgumentNullException("Email address not found");

            EmbedParams embedParams;
            if(reportConfig.UseRowLevelSecurity)
            {
                embedParams = pbiEmbedService.GetEmbedParams(new Guid(reportConfig.WorkspaceId), new Guid(reportConfig.ReportId), userEmail, reportConfig.IdentityRoles);
            }
            else
            {
                embedParams = pbiEmbedService.GetEmbedParams(new Guid(reportConfig.WorkspaceId), new Guid(reportConfig.ReportId));
            }

            var report = await module.InvokeAsync<IJSObjectReference>(
                "initCustomLayoutReport",
                obj,
                reportContainer,
                embedParams.EmbedToken.Token,
                embedParams.EmbedReport[0].EmbedUrl,
                embedParams.EmbedReport[0].ReportId.ToString(),
                "ReportSection",
                new string[] { "aa9e6f7833a06a40c297", "3dfea830056db635a1c2" }
            );

            return report;
        }

        public async ValueTask<IJSObjectReference> EmbedFullReportJS(string reportName, ElementReference reportContainer)
        {
            module = await js.InvokeAsync<IJSObjectReference>("import", "./js/powerbi-embed.js");
            var reportConfig = powerBiConfig.Value.Reports.FirstOrDefault(r => r.Key == reportName).Value;
            if (reportConfig == null)
                throw new ArgumentException($"Could not find configured report ${reportName}");

            var userEmail = "jbidwell@innovativesol.com"; //httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(userEmail))
                throw new ArgumentNullException("Email address not found");

            EmbedParams embedParams;
            if (reportConfig.UseRowLevelSecurity)
            {
                embedParams = pbiEmbedService.GetEmbedParams(new Guid(reportConfig.WorkspaceId), new Guid(reportConfig.ReportId), userEmail, reportConfig.IdentityRoles);
            }
            else
            {
                embedParams = pbiEmbedService.GetEmbedParams(new Guid(reportConfig.WorkspaceId), new Guid(reportConfig.ReportId));
            }

            var report = await module.InvokeAsync<IJSObjectReference>(
                "embedFullReport",                
                reportContainer,
                embedParams.EmbedToken.Token,
                embedParams.EmbedReport[0].EmbedUrl,
                embedParams.EmbedReport[0].ReportId.ToString()
            );

            return report;
        }
    }
}
