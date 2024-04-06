using Amazon.S3.Model;
using Amazon.S3;
using Amazon.SimpleEmail.Model;
using Amazon.SimpleEmail;
using MimeKit;
using System.IO.Compression;
using TheradexPortal.Data.Services.Abstract;

namespace TheradexPortal.Data.Services
{
    public class AWSS3Service : IAWSS3Service
    {
        private readonly ILogger<AWSS3Service> _logger;
        private readonly IAmazonS3 _s3Client;

        public AWSS3Service(ILogger<AWSS3Service> logger, IAmazonS3 s3Client)
        {
            _logger = logger;
            _s3Client = s3Client;
        }

        public async Task<bool> UploadDataAsync(string bucketName, string objectKey, string content)
        {
            try
            {
                var request = new PutObjectRequest
                {
                    BucketName = bucketName,
                    Key = objectKey,
                    ContentBody = content
                };

                var response = await _s3Client.PutObjectAsync(request);

                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    _logger.LogInformation($"Successfully uploaded Object ({objectKey}) to bucket ({bucketName}).");

                    return true;
                }
                else
                {
                    _logger.LogInformation($"Could not upload Object ({objectKey}) to bucket ({bucketName}).");

                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error uploading the object ({objectKey}) to bucket ({bucketName}): {ex}");

                return false;
            }
        }

        public async Task<bool> UploadStreamAsync(string bucketName, string objectKey, Stream stream)
        {
            try
            {
                var request = new PutObjectRequest
                {
                    BucketName = bucketName,
                    Key = objectKey,
                    InputStream = stream
                };

                var response = await _s3Client.PutObjectAsync(request);

                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    _logger.LogInformation($"Successfully uploaded Object ({objectKey}) to bucket ({bucketName}).");

                    return true;
                }
                else
                {
                    _logger.LogInformation($"Could not upload Object ({objectKey}) to bucket ({bucketName}).");

                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error uploading the object ({objectKey}) to bucket ({bucketName}): {ex}");

                return false;
            }
        }
    }
}