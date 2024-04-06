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

        public async Task<string> DownloadObjectFromBucketAsync(string bucketName, string objectKey)
        {
            try
            {
                var request = new GetObjectRequest
                {
                    BucketName = bucketName,
                    Key = objectKey
                };

                _logger.LogInformation($"Downloading the Object ({objectKey}) from bucket ({bucketName}).");

                using GetObjectResponse response = await _s3Client.GetObjectAsync(request);

                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    _logger.LogInformation($"Successfully downloaded the Object ({objectKey}) from bucket ({bucketName}).");

                    StreamReader reader = new StreamReader(response.ResponseStream);

                    string content = reader.ReadToEnd();

                    return content;
                }
                else
                {
                    _logger.LogInformation($"Could not download the Object ({objectKey}) from bucket ({bucketName}); HttpStatusCode: {response.HttpStatusCode};");

                    return null;
                }
            }
            catch (AmazonS3Exception ex)
            {
                _logger.LogError($"Exception downloading the Object ({objectKey}) from bucket ({bucketName}); AmazonS3Exception: {ex};");

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception downloading the Object ({objectKey}) from bucket ({bucketName}); Exception: {ex};");

                return null;
            }
        }

        public async Task<GetObjectResponse> DownloadAsync(string bucketName, string objectKey)
        {
            try
            {
                var request = new GetObjectRequest
                {
                    BucketName = bucketName,
                    Key = objectKey
                };

                _logger.LogInformation($"Downloading the Object ({objectKey}) from bucket ({bucketName}).");

                var response =  await _s3Client.GetObjectAsync(request);

                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    _logger.LogInformation($"Successfully downloaded the Object ({objectKey}) from bucket ({bucketName}).");

                    return response;
                }
                else
                {
                    _logger.LogInformation($"Could not download the Object ({objectKey}) from bucket ({bucketName}); HttpStatusCode: {response.HttpStatusCode};");

                    return null;
                }
            }
            catch (AmazonS3Exception ex)
            {
                _logger.LogError($"Exception downloading the Object ({objectKey}) from bucket ({bucketName}); AmazonS3Exception: {ex};");

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception downloading the Object ({objectKey}) from bucket ({bucketName}); Exception: {ex};");

                return null;
            }
        }

    }
}