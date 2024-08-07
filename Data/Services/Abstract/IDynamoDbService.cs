using System.Threading.Tasks;
using TheradexPortal.Data.Models;

namespace TheradexPortal.Data.Services.Abstract
{
    public interface IDynamoDbService
    {
        Task<List<FileIngestRequest>?> GetAllRequestsOfUser(int userId, bool isAdminDisplay, string environment);

        Task<List<ReceivingStatusFileData>?> GetAllReceivingStatusData(string requestId);

        Task<List<BiospecimenRoadmapFileData>?> GetAllBiospecimenRoadmapData(string requestId);
    }
}
