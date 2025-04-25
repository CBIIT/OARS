using Amazon.S3.Model;

namespace OARS.Data.Services.Abstract
{
    public interface IAWSS3Service
    {
        Task<bool> UploadDataAsync(string bucketName, string objectName, string content);

        Task<bool> UploadStreamAsync(string bucketName, string objectKey, Stream stream);

        Task<string> GetPreSignedUrl(string bucketName, string objectKey, string fileName);

        Task<string> GetDataAsync(string bucketName, string objectKey);
    }
}