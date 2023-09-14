using Microsoft.PowerBI.Api;
using Microsoft.PowerBI.Api.Models;
using System.Runtime.InteropServices;
using TheradexPortal.Data.PowerBI.Models;

namespace TheradexPortal.Data.PowerBI.Abstract
{
    public interface IPbiEmbedService
    {
        public PowerBIClient GetPowerBIClient();
        public EmbedParams GetEmbedParams(Guid workspaceId, Guid reportId, [Optional] Guid additionalDatasetId);
        public EmbedParams GetEmbedParams(Guid workspaceId, Guid reportId, string userEmail, string[] roles, [Optional] Guid additionalDatasetId);
        public EmbedToken GetEmbedToken(Guid reportId, IList<Guid> datasetIds, [Optional] Guid targetWorkspaceId);
        public EmbedToken GetEmbedToken(Guid reportId, IList<Guid> datasetIds, string userEmail, string[] roles, [Optional] Guid targetWorkspaceId);
        public EmbedToken GetEmbedTokenForRDLReport(Guid targetWorkspaceId, Guid reportId, string accessLevel = "view");
        public Task<Reports> GetPowerBIReportList(Guid targetWorkspaceId);
        public Task<Microsoft.PowerBI.Api.Models.Pages> GetPowerBIReportPages(Guid targetWorkspaceId, Guid powerBIReportId);

    }
}