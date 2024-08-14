using Amazon.S3.Transfer;
using Amazon.S3;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using TheradexPortal.Data.Models;
using TheradexPortal.Data.Services.Abstract;
using Amazon;
using System.Text;
using Microsoft.Extensions.Options;
using TheradexPortal.Data.Models.Configuration;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using System.IO;
using Newtonsoft.Json;
using Org.BouncyCastle.Bcpg.OpenPgp;
using Microsoft.PowerBI.Api.Models;

namespace TheradexPortal.Data.Services
{
    public class UploadService : BaseService, IUploadService
    {
        private readonly IErrorLogService _errorLogService;
        private readonly NavigationManager _navManager;
        private readonly UploadSettings _uploadSettings;
        private readonly IDynamoDbService _dynamoDbService;
        private readonly IAWSS3Service _awsS3Service;
        private readonly IStudyService _studyService;

        public UploadService(IOptions<UploadSettings> uploadSettings,
            IDatabaseConnectionService databaseConnectionService,
            IErrorLogService errorLogService,
            NavigationManager navigationManager,
            IDynamoDbService dynamoDbService,
            IStudyService studyService,
            IAWSS3Service awsS3Service) : base(databaseConnectionService)
        {
            _errorLogService = errorLogService;
            _navManager = navigationManager;
            _uploadSettings = uploadSettings.Value;
            _dynamoDbService = dynamoDbService;
            _awsS3Service = awsS3Service;
            _studyService = studyService;
        }

        public async Task<UploadConfiguration> GetUploadConfigurationAsync(int userId, bool allStudies)
        {
            var uploadConfiguration = new UploadConfiguration();

            uploadConfiguration.ProtocolData = await GetProtocolData(userId, allStudies);

            uploadConfiguration.CRFRules = await GetCRFRules();

            return uploadConfiguration;
        }

        private async Task<List<ProtocolData>> GetProtocolData(int userId, bool allStudies)
        {
            try
            {
                var dataFileContent = await _awsS3Service.GetDataAsync(_uploadSettings.AWSBucketName, _uploadSettings.ProtocolDataPath);

                if (dataFileContent == null)
                {
                    return null;
                }

                var protocols = JsonConvert.DeserializeObject<List<ProtocolData>>(dataFileContent);

                var protocolsWithAccess = _studyService.GetProtocolsForUserAsync(userId, allStudies);

                return protocols;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private async Task<List<CRFRule>> GetCRFRules()
        {
            try
            {
                var dataFileContent = await _awsS3Service.GetDataAsync(_uploadSettings.AWSBucketName, _uploadSettings.CRFRulesPath);

                if (dataFileContent == null)
                {
                    return null;
                }

                var crfRules = JsonConvert.DeserializeObject<List<CRFRule>>(dataFileContent);

                return crfRules;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<CRFModel> GetCRFs()
        {
            return new List<CRFModel>
            {
                new CRFModel{FormName = "Biospecimen Roadmap", FormOID = "BIOSPECIMEN_ROADMAP_ASSAY" },
                new CRFModel{FormName = "Receiving Status", FormOID = "RECEIVING_STATUS" },
                new CRFModel{FormName = "Shipping Status", FormOID = "SHIPPING_STATUS" },
                new CRFModel{FormName = "IFA", FormOID = "IFA_RESULT_SUMMARY" },
                new CRFModel{FormName = "TSO500 Library QC", FormOID = "LIBRARY_QC" },
                new CRFModel{FormName = "TSO500 Sequencing QC", FormOID = "SEQUENCING_QC" }
            };
        }

        public async Task<bool> UploadCsvFileToS3(string key, int userId, MemoryStream memoryStream)
        {
            try
            {
                var isSuccess = await _awsS3Service.UploadStreamAsync(_uploadSettings.AWSBucketName, key, memoryStream);

                return isSuccess;

            }
            catch (Exception ex)
            {
                _errorLogService.SaveErrorLogAsync(userId, _navManager.Uri, ex.InnerException, ex.Source, ex.Message, ex.StackTrace);

                return false;
            }
        }

        public async Task<bool> UploadMetatdataFileToS3(string key, int userId, string content)
        {
            try
            {
                var isSuccess = await _awsS3Service.UploadDataAsync(_uploadSettings.AWSBucketName, key, content);

                return isSuccess;

            }
            catch (Exception ex)
            {
                _errorLogService.SaveErrorLogAsync(userId, _navManager.Uri, ex.InnerException, ex.Source, ex.Message, ex.StackTrace);

                return false;
            }
        }

        public FileMetadata GetMetadatafile(ETCTNUploadRequest ETCTNUploadRequestModel, UploadFileModel UploadFile, int userId, string env)
        {
            var metadataFile = new FileMetadata();

            metadataFile.RequestId = ETCTNUploadRequestModel.RequestId;
            metadataFile.Assay = ETCTNUploadRequestModel.Assay;
            metadataFile.SourceSite = ETCTNUploadRequestModel.SourceSite;
            metadataFile.ReceivingSite = ETCTNUploadRequestModel.ReceivingSite;
            metadataFile.CRF = ETCTNUploadRequestModel.CRF;
            metadataFile.OriginalFileName = UploadFile.OriginalFileName;
            metadataFile.FilePath = UploadFile.S3Key;
            metadataFile.Bucket = _uploadSettings.AWSBucketName;
            metadataFile.Protocol = ETCTNUploadRequestModel.Protocol;
            metadataFile.UserId = userId.ToString();
            metadataFile.Environment = env;

            return metadataFile;
        }

        public string GetCsvUploadKey(ETCTNUploadRequest ETCTNUploadRequestModel)
        {
            var dateKey = $"{DateTime.Now.Year}-{DateTime.Now.Month}/{DateTime.Now.Day}";

            return $"{_uploadSettings.FilesUploadPath}/{ETCTNUploadRequestModel.CRF}/{dateKey}/{Guid.NewGuid()}.csv";
        }

        public string GetMetadataFileUploadKey(ETCTNUploadRequest ETCTNUploadRequestModel)
        {
            var dateKey = $"{DateTime.Now.Year}-{DateTime.Now.Month}/{DateTime.Now.Day}";

            return $"{_uploadSettings.MetadataUploadPath}/{ETCTNUploadRequestModel.CRF}/{dateKey}/{ETCTNUploadRequestModel.RequestId}.json";
        }

        public async Task<List<FileIngestRequest>?> GetAllRequestsOfUser(int userId, bool isAdminDisplay, string environment)
        {
            var requests = await _dynamoDbService.GetAllRequestsOfUser(userId, isAdminDisplay, environment);
            var crfs = GetCRFs();

            if (requests != null)
            {
                foreach (var request in requests)
                {
                    if (request.Status >= 101 && request.Status <= 110)
                    {
                        request.ClientStatus = RequestStatusV2.Received.ToString();
                    }
                    else if (request.Status >= 111 && request.Status <= 120)
                    {
                        request.ClientStatus = RequestStatusV2.InProgress.ToString();
                    }
                    else if (request.Status >= 121 && request.Status <= 130)
                    {
                        request.ClientStatus = ((RequestStatusV2)request.Status).ToString();
                    }
                    else
                    {
                        request.ClientStatus = "UnKnown";
                    }

                    request.InternalStatus = ((RequestStatusV2)request.Status).ToString();

                    request.Metadata.CRFDescription = crfs.First(t => t.FormOID.Equals(request.Metadata.CRF)).FormName;
                }
            }

            return requests;
        }

        public async Task<List<ReceivingStatusFileData>?> GetReceivingStatusFileData(string requestId)
        {
            var requestItems = await _dynamoDbService.GetAllReceivingStatusData(requestId);

            if (requestItems != null)
            {
                foreach (var requestItem in requestItems)
                {
                    if (requestItem.Status >= 201 && requestItem.Status <= 210)
                    {
                        requestItem.ClientStatus = RequestItemStatusV2.Received.ToString();
                    }
                    else if (requestItem.Status >= 211 && requestItem.Status <= 220)
                    {
                        requestItem.ClientStatus = RequestItemStatusV2.InProgress.ToString();
                    }
                    else if (requestItem.Status >= 221 && requestItem.Status <= 230)
                    {
                        requestItem.ClientStatus = ((RequestItemStatusV2)requestItem.Status).ToString();
                    }
                    else
                    {
                        requestItem.ClientStatus = "UnKnown";
                    }

                    requestItem.InternalStatus = ((RequestItemStatusV2)requestItem.Status).ToString();
                }
            }

            return requestItems;
        }

        public async Task<List<BiospecimenRoadmapFileData>?> GetBiospecimenRoadmapFileData(string requestId)
        {
            var requestItems = await _dynamoDbService.GetAllBiospecimenRoadmapData(requestId);

            if (requestItems != null)
            {
                foreach (var requestItem in requestItems)
                {
                    if (requestItem.Status >= 201 && requestItem.Status <= 210)
                    {
                        requestItem.ClientStatus = RequestItemStatusV2.Received.ToString();
                    }
                    else if (requestItem.Status >= 211 && requestItem.Status <= 220)
                    {
                        requestItem.ClientStatus = RequestItemStatusV2.InProgress.ToString();
                    }
                    else if (requestItem.Status >= 221 && requestItem.Status <= 230)
                    {
                        requestItem.ClientStatus = ((RequestItemStatusV2)requestItem.Status).ToString();
                    }
                    else
                    {
                        requestItem.ClientStatus = "UnKnown";
                    }

                    requestItem.InternalStatus = ((RequestItemStatusV2)requestItem.Status).ToString();
                }
            }

            return requestItems;
        }

        public async Task<List<ShippingStatusFileData>?> GetShippingStatusFileData(string requestId)
        {
            var requestItems = await _dynamoDbService.GetAllShippingStatusData(requestId);

            if (requestItems != null)
            {
                foreach (var requestItem in requestItems)
                {
                    if (requestItem.Status >= 201 && requestItem.Status <= 210)
                    {
                        requestItem.ClientStatus = RequestItemStatusV2.Received.ToString();
                    }
                    else if (requestItem.Status >= 211 && requestItem.Status <= 220)
                    {
                        requestItem.ClientStatus = RequestItemStatusV2.InProgress.ToString();
                    }
                    else if (requestItem.Status >= 221 && requestItem.Status <= 230)
                    {
                        requestItem.ClientStatus = ((RequestItemStatusV2)requestItem.Status).ToString();
                    }
                    else
                    {
                        requestItem.ClientStatus = "UnKnown";
                    }

                    requestItem.InternalStatus = ((RequestItemStatusV2)requestItem.Status).ToString();
                }
            }

            return requestItems;
        }

        public async Task<List<TSO500SequencingQCFileData>?> GetTSO500SequencingQCFileData(string requestId)
        {
            var requestItems = await _dynamoDbService.GetAllTSO500SequencingQCData(requestId);

            if (requestItems != null)
            {
                foreach (var requestItem in requestItems)
                {
                    if (requestItem.Status >= 201 && requestItem.Status <= 210)
                    {
                        requestItem.ClientStatus = RequestItemStatusV2.Received.ToString();
                    }
                    else if (requestItem.Status >= 211 && requestItem.Status <= 220)
                    {
                        requestItem.ClientStatus = RequestItemStatusV2.InProgress.ToString();
                    }
                    else if (requestItem.Status >= 221 && requestItem.Status <= 230)
                    {
                        requestItem.ClientStatus = ((RequestItemStatusV2)requestItem.Status).ToString();
                    }
                    else
                    {
                        requestItem.ClientStatus = "UnKnown";
                    }

                    requestItem.InternalStatus = ((RequestItemStatusV2)requestItem.Status).ToString();
                }
            }

            return requestItems;
        }

        public async Task<List<TSO500LibraryQCFileData>?> GetTSO500LibraryQCFileData(string requestId)
        {
            var requestItems = await _dynamoDbService.GetAllTSO500LibraryQCData(requestId);

            if (requestItems != null)
            {
                foreach (var requestItem in requestItems)
                {
                    if (requestItem.Status >= 201 && requestItem.Status <= 210)
                    {
                        requestItem.ClientStatus = RequestItemStatusV2.Received.ToString();
                    }
                    else if (requestItem.Status >= 211 && requestItem.Status <= 220)
                    {
                        requestItem.ClientStatus = RequestItemStatusV2.InProgress.ToString();
                    }
                    else if (requestItem.Status >= 221 && requestItem.Status <= 230)
                    {
                        requestItem.ClientStatus = ((RequestItemStatusV2)requestItem.Status).ToString();
                    }
                    else
                    {
                        requestItem.ClientStatus = "UnKnown";
                    }

                    requestItem.InternalStatus = ((RequestItemStatusV2)requestItem.Status).ToString();
                }
            }

            return requestItems;
        }


        public string GetRequestId(ETCTNUploadRequest ETCTNUploadRequestModel)
        {
            // Generate a new GUID
            Guid guid = Guid.NewGuid();

            // Convert the GUID to bytes
            byte[] bytes = guid.ToByteArray();

            // Create a base64 string from the bytes
            string base64String = Convert.ToBase64String(bytes);

            // Remove padding characters from base64 string
            base64String = base64String.TrimEnd('=');

            // Replace invalid characters
            base64String = base64String.Replace('+', 'P').Replace('/', 'S').Replace('=', 'E');

            string prefix = "";

            if (ETCTNUploadRequestModel.CRF == "RECEIVING_STATUS")
                prefix = "RS";
            else if (ETCTNUploadRequestModel.CRF == "BIOSPECIMEN_ROADMAP_ASSAY")
                prefix = "BA";
            else if (ETCTNUploadRequestModel.CRF == "LIBRARY_QC")
                prefix = "LQ";
            else if (ETCTNUploadRequestModel.CRF == "SEQUENCING_QC")
                prefix = "SQ";
            else if (ETCTNUploadRequestModel.CRF == "SHIPPING_STATUS")
                prefix = "SS";
            else if (ETCTNUploadRequestModel.CRF == "IFA_RESULT_SUMMARY")
                prefix = "IF";

            // Prefix the unique code
            string uniqueCode = prefix + base64String;

            return uniqueCode;
        }

        public Task<string> GetCsvFileDownloadUrl(FileIngestRequest request)
        {
            return _awsS3Service.GetPreSignedUrl(request.Metadata.Bucket, request.Metadata.FilePath, request.Metadata.OriginalFileName);
        }

        public Task<string> GetCRFTemplateDownloadUrl(string crf, string fileName)
        {
            var objectKey = $"{_uploadSettings.TemplatesPath}/{crf}.csv";

            return _awsS3Service.GetPreSignedUrl(_uploadSettings.AWSBucketName, objectKey, fileName);
        }
    }
}