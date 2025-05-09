﻿namespace OARS.Data.PowerBI
{
    using Microsoft.AspNetCore.Components.Authorization;
    using Microsoft.PowerBI.Api;
    using Microsoft.PowerBI.Api.Models;
    using Microsoft.Rest;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.InteropServices;
    using OARS.Data.PowerBI.Abstract;
    using OARS.Data.PowerBI.Models;

    public class PbiEmbedService : IPbiEmbedService
    {
        private readonly IAadService aadService;
        private readonly string urlPowerBiServiceApiRoot = "https://api.powerbi.com";
        private readonly ILogger<PbiEmbedService> logger;
        public PbiEmbedService(ILogger<PbiEmbedService> logger, IAadService aadService)
        {
            this.logger = logger;
            this.aadService = aadService;
        }

        /// <summary>
        /// Get Power BI client
        /// </summary>
        /// <returns>Power BI client object</returns>
        public PowerBIClient GetPowerBIClient()
        {
            logger.LogInformation("Entered"); // Log class and method name
            var tokenCredentials = new TokenCredentials(aadService.GetAccessToken(), "Bearer");
            return new PowerBIClient(new Uri(urlPowerBiServiceApiRoot), tokenCredentials);
        }

        /// <summary>
        /// Get embed params for a report
        /// </summary>
        /// <returns>Wrapper object containing Embed token, Embed URL, Report Id, and Report name for single report</returns>
        public EmbedParams GetEmbedParams(Guid workspaceId, Guid reportId, [Optional] Guid additionalDatasetId)
        {
            logger.LogInformation("Entered, Parameters: workspaceId={WorkspaceId}, reportId={ReportId}, additionalDatasetId={AdditionalDatasetId}", workspaceId, reportId, additionalDatasetId);
            PowerBIClient pbiClient = this.GetPowerBIClient();

            // Get report info
            var pbiReport = pbiClient.Reports.GetReportInGroup(workspaceId, reportId);

            //  Check if dataset is present for the corresponding report
            //  If isRDLReport is true then it is a RDL Report 
            var isRDLReport = String.IsNullOrEmpty(pbiReport.DatasetId);

            EmbedToken embedToken;

            // Generate embed token for RDL report if dataset is not present
            if (isRDLReport)
            {
                // Get Embed token for RDL Report
                embedToken = GetEmbedTokenForRDLReport(workspaceId, reportId);
            }
            else
            {
                // Create list of datasets
                var datasetIds = new List<Guid>();

                // Add dataset associated to the report
                datasetIds.Add(Guid.Parse(pbiReport.DatasetId));

                // Append additional dataset to the list to achieve dynamic binding later
                if (additionalDatasetId != Guid.Empty)
                {
                    datasetIds.Add(additionalDatasetId);
                }

                // Get Embed token multiple resources
                embedToken = GetEmbedToken(reportId, datasetIds, workspaceId);
            }

            // Add report data for embedding
            var embedReports = new List<EmbedReport>() {
                new EmbedReport
                {
                    ReportId = pbiReport.Id, ReportName = pbiReport.Name, EmbedUrl = pbiReport.EmbedUrl
                }
            };

            // Capture embed params
            var embedParams = new EmbedParams
            {
                EmbedReport = embedReports,
                Type = "Report",
                EmbedToken = embedToken
            };

            return embedParams;
        }        
        
        
        /// <summary>
        /// Get embed params for a report with row level security
        /// </summary>
        /// <returns>Wrapper object containing Embed token, Embed URL, Report Id, and Report name for single report</returns>
        public EmbedParams GetEmbedParams(Guid workspaceId, Guid reportId, string userEmail, string[] roles, [Optional] Guid additionalDatasetId)
        {
            logger.LogInformation("Entered Parameters: workspaceId={WorkspaceId}, reportId={ReportId}, userEmail={UserEmail}, roles={Roles}, additionalDatasetId={AdditionalDatasetId}",
                                 workspaceId, reportId, userEmail, string.Join(",", roles), additionalDatasetId);

            PowerBIClient pbiClient = this.GetPowerBIClient();

            // Get report info
            var pbiReport = pbiClient.Reports.GetReportInGroup(workspaceId, reportId);

            //  Check if dataset is present for the corresponding report
            //  If isRDLReport is true then it is a RDL Report 
            var isRDLReport = String.IsNullOrEmpty(pbiReport.DatasetId);

            EmbedToken embedToken;

            // Generate embed token for RDL report if dataset is not present
            if (isRDLReport)
            {
                // Get Embed token for RDL Report
                embedToken = GetEmbedTokenForRDLReport(workspaceId, reportId);
            }
            else
            {
                // Create list of datasets
                var datasetIds = new List<Guid>();

                // Add dataset associated to the report
                datasetIds.Add(Guid.Parse(pbiReport.DatasetId));

                // Append additional dataset to the list to achieve dynamic binding later
                if (additionalDatasetId != Guid.Empty)
                {
                    datasetIds.Add(additionalDatasetId);
                }

                // Get Embed token multiple resources
                embedToken = GetEmbedToken(reportId, datasetIds, userEmail, roles, workspaceId);
            }

            // Add report data for embedding
            var embedReports = new List<EmbedReport>() {
                new EmbedReport
                {
                    ReportId = pbiReport.Id, ReportName = pbiReport.Name, EmbedUrl = pbiReport.EmbedUrl
                }
            };

            // Capture embed params
            var embedParams = new EmbedParams
            {
                EmbedReport = embedReports,
                Type = "Report",
                EmbedToken = embedToken
            };

            return embedParams;
        }

        /// <summary>
        /// Get Embed token for single report, multiple datasets, and an optional target workspace
        /// </summary>
        /// <returns>Embed token</returns>
        /// <remarks>This function is not supported for RDL Report</remakrs>
        public EmbedToken GetEmbedToken(Guid reportId, IList<Guid> datasetIds, [Optional] Guid targetWorkspaceId)
        {
            logger.LogInformation("Entered Parameters: reportId={ReportId}, datasetIds={DatasetIds}, targetWorkspaceId={TargetWorkspaceId}",
                      reportId, string.Join(",", datasetIds), targetWorkspaceId);

            PowerBIClient pbiClient = this.GetPowerBIClient();

            // Create a request for getting Embed token 
            // This method works only with new Power BI V2 workspace experience
            var tokenRequest = new GenerateTokenRequestV2(

                reports: new List<GenerateTokenRequestV2Report>() { new GenerateTokenRequestV2Report(reportId) },

                datasets: datasetIds.Select(datasetId => new GenerateTokenRequestV2Dataset(datasetId.ToString())).ToList()
            );

            // Generate Embed token
            var embedToken = pbiClient.EmbedToken.GenerateToken(tokenRequest);

            return embedToken;
        }
        
        
        /// <summary>
        /// Get Embed token for single report with row level security, multiple datasets, and an optional target workspace
        /// </summary>
        /// <returns>Embed token</returns>
        /// <remarks>This function is not supported for RDL Report</remakrs>
        public EmbedToken GetEmbedToken(Guid reportId, IList<Guid> datasetIds, string userEmail, string[] roles, [Optional] Guid targetWorkspaceId)
        {
            logger.LogInformation("Entered Parameters: reportId={ReportId}, datasetIds={DatasetIds}, userEmail={UserEmail}, roles={Roles}, targetWorkspaceId={TargetWorkspaceId}",
                                 reportId, string.Join(",", datasetIds), userEmail, string.Join(",", roles), targetWorkspaceId);

            PowerBIClient pbiClient = this.GetPowerBIClient();

            // Create a request for getting Embed token 
            // This method works only with new Power BI V2 workspace experience
            var tokenRequest = new GenerateTokenRequestV2(

                reports: new List<GenerateTokenRequestV2Report>() { new GenerateTokenRequestV2Report(reportId) },

                datasets: datasetIds.Select(datasetId => new GenerateTokenRequestV2Dataset(datasetId.ToString())).ToList(),

                targetWorkspaces: targetWorkspaceId != Guid.Empty ? new List<GenerateTokenRequestV2TargetWorkspace>() { new GenerateTokenRequestV2TargetWorkspace(targetWorkspaceId) } : null,

                identities: new List<EffectiveIdentity>
                {
                    new EffectiveIdentity
                    (
                        username: userEmail,
                        datasets: datasetIds.Select(datasetId => datasetId.ToString()).ToList(),
                        roles: roles
                    )
                }
            );

            // Generate Embed token
            var embedToken = pbiClient.EmbedToken.GenerateToken(tokenRequest);

            return embedToken;
        }


        /// <summary>
        /// Get Embed token for RDL Report
        /// </summary>
        /// <returns>Embed token</returns>
        public EmbedToken GetEmbedTokenForRDLReport(Guid targetWorkspaceId, Guid reportId, string accessLevel = "view")
        {
            logger.LogInformation("Entered Parameters: targetWorkspaceId={TargetWorkspaceId}, reportId={ReportId}, accessLevel={AccessLevel}",
                                 targetWorkspaceId, reportId, accessLevel);

            PowerBIClient pbiClient = this.GetPowerBIClient();

            // Generate token request for RDL Report
            var generateTokenRequestParameters = new GenerateTokenRequest(
                accessLevel: accessLevel
            );

            // Generate Embed token
            var embedToken = pbiClient.Reports.GenerateTokenInGroup(targetWorkspaceId, reportId, generateTokenRequestParameters);

            return embedToken;
        }

        public async Task<Reports> GetPowerBIReportList(Guid targetWorkspaceId)
        {
            logger.LogInformation("Entered Parameters: targetWorkspaceId={TargetWorkspaceId}",
                                 targetWorkspaceId);

            PowerBIClient pbiClient = this.GetPowerBIClient();

            var pbiReports = await pbiClient.Reports.GetReportsInGroupAsync(targetWorkspaceId);

            return pbiReports;
        }

        public async Task<Pages> GetPowerBIReportPages(Guid targetWorkspaceId, Guid powerBIReportId)
        {
            logger.LogInformation("Entered Parameters: targetWorkspaceId={TargetWorkspaceId}, powerBIReportId={PowerBIReportId}",
                              targetWorkspaceId, powerBIReportId);
            PowerBIClient pbiClient = this.GetPowerBIClient();

            var pbiPages = await pbiClient.Reports.GetPagesInGroupAsync(targetWorkspaceId, powerBIReportId);

            return pbiPages;
        }
    }
}
