using TheradexPortal.Data.Models;

namespace TheradexPortal.Data.Services.Abstract
{
    public interface IUploadService
    {
        public Task<UploadConfiguration> GetUploadConfigurationAsync(int userId, bool allStudies);

        public List<CRFModel> GetCRFs();

        public Task<bool> UploadCsvFileToS3(string key, int userId, MemoryStream memoryStream);

        public Task<bool> UploadMetatdataFileToS3(string key, int userId, string content);

        public FileMetadata GetMetadatafile(ETCTNUploadRequest ETCTNUploadRequestModel, UploadFileModel UploadFile, int userId, string env);

        public string GetCsvUploadKey(ETCTNUploadRequest ETCTNUploadRequestModel);

        public string GetMetadataFileUploadKey(ETCTNUploadRequest ETCTNUploadRequestModel);

        public Task<List<FileIngestRequest>?> GetAllRequestsOfUser(int userId, bool isAdminDisplay, string environment);

        public Task<List<ReceivingStatusFileData>?> GetReceivingStatusFileData(string requestId);

        public Task<List<BiospecimenRoadmapFileData>?> GetBiospecimenRoadmapFileData(string requestId);

        public Task<List<ShippingStatusFileData>?> GetShippingStatusFileData(string requestId);

        public Task<List<TSO500SequencingQCFileData>?> GetTSO500SequencingQCFileData(string requestId);

        public Task<List<TSO500LibraryQCFileData>?> GetTSO500LibraryQCFileData(string requestId);

        public Task<List<IFAFileData>?> GetIFAFileData(string requestId);
        public Task<List<IFAResultSummaryFileData>?> GetIFAResultSummaryFileData(string requestId);
        public Task<List<PathologyEvaluationReportFileData>?> GetPathologyEvaluationReportFileData(string requestId);


        public Task<string> GetCsvFileDownloadUrl(FileIngestRequest request);

        public Task<string> GetCRFTemplateDownloadUrl(string crf, string fileName);

        public string GetRequestId(ETCTNUploadRequest ETCTNUploadRequestModel);
    }
}
