using System.Threading.Tasks;
using TheradexPortal.Data.Models;

namespace TheradexPortal.Data.Services.Abstract
{
    public interface IDynamoDbService
    {
        Task<FileIngestRequest?> GetRequest(string requestId);

        Task<List<FileIngestRequest>?> GetAllRequestsOfUser(int userId);

        Task<List<ReceivingStatusFileData>?> GetAllReceivingStatusData(string requestId);
    }
}
