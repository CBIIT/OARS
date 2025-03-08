using System.Threading.Tasks;
using TheradexPortal.Data.Models;
using TheradexPortal.Data.Models.ADDR;

namespace TheradexPortal.Data.Services.Abstract
{
    public interface IDynamoDbService
    {
        Task<List<FileIngestRequest>?> GetAllRequestsOfUser(int userId, bool isAdminDisplay, string environment);

        Task<List<ReceivingStatusFileData>?> GetAllReceivingStatusData(string requestId);

        Task<List<BiospecimenRoadmapFileData>?> GetAllBiospecimenRoadmapData(string requestId);

        Task<List<ShippingStatusFileData>?> GetAllShippingStatusData(string requestId);

        Task<List<TSO500SequencingQCFileData>?> GetAllTSO500SequencingQCData(string requestId);

        Task<List<TSO500LibraryQCFileData>?> GetAllTSO500LibraryQCData(string requestId);

        Task<List<IFAFileData>?> GetAllIFAData(string requestId);

        Task<List<IFAResultSummaryFileData>?> GetAllIFAResultSummaryData(string requestId);

        Task<List<PathologyEvaluationReportFileData>?> GetAllPathologyEvaluationReportData(string requestId);

        Task<bool> SaveAddrNotes<T>(AddrNotes<T> notes);

        Task<List<AddrNotes<T>>> GetAllAddrNotes<T>(string userId, string searchKey);
    }
}
