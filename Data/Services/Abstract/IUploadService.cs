using TheradexPortal.Data.Models;

namespace TheradexPortal.Data.Services.Abstract
{
    public interface IUploadService
    {
        public List<string> GetStudies();

        public List<MedidataDictionaryModel> GetAssays();

        public List<MedidataDictionaryModel> GeTrackingDestinations();

        public List<CRFModel> GetCRFs();

        public Task<bool> UploadCsvFileToS3(string key, int userId, MemoryStream memoryStream);

        public Task<bool> UploadMetatdataFileToS3(string key, int userId, string content);

        public FileMetadata GetMetadatafile(ETCTNUploadRequest ETCTNUploadRequestModel, UploadFileModel UploadFile, int userId);

        public string GetCsvUploadKey(string id);

        public string GetMetadataFileUploadKey(string id);
    }
}
