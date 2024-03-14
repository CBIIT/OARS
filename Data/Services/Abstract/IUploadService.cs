using TheradexPortal.Data.Models;

namespace TheradexPortal.Data.Services.Abstract
{
    public interface IUploadService
    {
        public List<string> GetStudiesToUpload();

        public List<MedidataDictionaryModel> GetAssaysToUpload();

        public List<MedidataDictionaryModel> GetLabsToUpload();

        public List<CRFModel> GetCRFsToUpload();

        public Task<bool> UploadCsvFileToS3(string bucket, string key, int userId, MemoryStream memoryStream);
    }
}
