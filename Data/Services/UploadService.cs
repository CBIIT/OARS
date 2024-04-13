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

namespace TheradexPortal.Data.Services
{
    public class UploadService : BaseService, IUploadService
    {
        private readonly IErrorLogService _errorLogService;
        private readonly NavigationManager _navManager;
        private readonly UploadSettings _uploadSettings;
        private readonly IDynamoDbService _dynamoDbService;
        private readonly IAWSS3Service _awsS3Service;

        public UploadService(IOptions<UploadSettings> uploadSettings,
            IDbContextFactory<ThorDBContext> dbFactory,
            IErrorLogService errorLogService,
            NavigationManager navigationManager,
            IDynamoDbService dynamoDbService,
            IAWSS3Service awsS3Service) : base(dbFactory)
        {
            _errorLogService = errorLogService;
            _navManager = navigationManager;
            _uploadSettings = uploadSettings.Value;
            _dynamoDbService = dynamoDbService;
            _awsS3Service = awsS3Service;
        }

        //public List<MedidataDictionaryModel> GetAssays()
        //{
        //    return new List<MedidataDictionaryModel>
        //    {
        //        new MedidataDictionaryModel{CodedData = "BCA", UserData = "BCA" },
        //        new MedidataDictionaryModel{CodedData = "Bioanalyzer", UserData = "Bioanalyzer" },
        //        new MedidataDictionaryModel{CodedData = "Bradford", UserData = "Bradford" },
        //        new MedidataDictionaryModel{CodedData = "E-gel", UserData = "E-gel" },
        //        new MedidataDictionaryModel{CodedData = "Picogreen", UserData = "Picogreen" },
        //        new MedidataDictionaryModel{CodedData = "Quant-iT", UserData = "Quant-iT" },
        //        new MedidataDictionaryModel{CodedData = "Qubit", UserData = "Qubit" },
        //        new MedidataDictionaryModel{CodedData = "Tapestation", UserData = "Tapestation" },
        //        new MedidataDictionaryModel{CodedData = "Nanodrop (Spec)", UserData = "Nanodrop (Spec)" }
        //    };
        //}        

        //public List<MedidataDictionaryModel> GeTrackingDestinations()
        //{
        //    return new List<MedidataDictionaryModel>
        //    {
        //       new MedidataDictionaryModel{CodedData="EET Biobank", UserData ="EET Biobank"},
        //       new MedidataDictionaryModel{CodedData="NCLN Genomics Laboratory for HER2", UserData ="NCLN Genomics Laboratory  for HER2"},
        //       new MedidataDictionaryModel{CodedData="NCLN Genomics Laboratory at MD Anderson for WES &amp; RNAseq", UserData ="NCLN Genomics Laboratory at MD Anderson for WES &amp; RNAseq"},
        //       new MedidataDictionaryModel{CodedData="Paulovich Laboratory,  Fred Hutchinson Cancer Research Center (FHCRC)", UserData ="Paulovich Laboratory,  Fred Hutchinson Cancer Research Center (FHCRC)"},
        //       new MedidataDictionaryModel{CodedData="PPD Laboratories", UserData ="PPD Laboratories"},
        //       new MedidataDictionaryModel{CodedData="NCLN PD Assay Laboratory at MD Anderson", UserData ="NCLN PD Assay Laboratory at MD Anderson"},
        //       new MedidataDictionaryModel{CodedData="Kopetz Laboratory, Department of Gastrointestinal Medical Oncology", UserData ="Kopetz Laboratory, Department of Gastrointestinal Medical Oncology, MD Anderson Cancer Center"},
        //       new MedidataDictionaryModel{CodedData="Covance Central Laboratory Services", UserData ="Covance Central Laboratory Services"},
        //       new MedidataDictionaryModel{CodedData="Frederick MoCha Lab", UserData ="Frederick MoCha Lab"},
        //       new MedidataDictionaryModel{CodedData="NCLN PD Assay Lab at Molecular Path Network", UserData ="NCLN PD Assay Lab at Molecular Path Network"},
        //       new MedidataDictionaryModel{CodedData="PADIS Lab at Frederick", UserData ="PADIS Lab at Frederick"},
        //       new MedidataDictionaryModel{CodedData="NCLN Genomics Laboratory at MDA", UserData ="NCLN Genomics Laboratory at MDA"},
        //       new MedidataDictionaryModel{CodedData="MDACC Tissue Qualification Laboratory (TQL)", UserData ="MDACC Tissue Qualification Laboratory (TQL)" }
        //    };
        //}

        //public List<string> GetStudies()
        //{
        //    var studies = new List<string>();

        //    studies.Add("10358(FUNCTEST)");

        //    return studies;
        //}

        public async Task<List<ProtocolData>> GetProtocolData()
        {
            try
            {
                var dataFileContent = await _awsS3Service.GetDataAsync(_uploadSettings.AWSBucketName, _uploadSettings.ProtocolDataPath);

                if (dataFileContent == null)
                {
                    return null;
                }

                var protocols = JsonConvert.DeserializeObject<List<ProtocolData>>(dataFileContent);

                return protocols;
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
                //new CRFModel{FormName = "TSO500 Library QC", FormOID = "LIBRARY_QC" },
                //new CRFModel{FormName = "TSO500 Sequencing QC", FormOID = "SEQUENCING_QC" },
                new CRFModel{FormName = "Biospecimen Roadmap", FormOID = "BIOSPECIMEN_ROADMAP" },
                new CRFModel{FormName = "Receiving Status", FormOID = "RECEIVING_STATUS" },
                //new CRFModel{FormName = "Shipping Status", FormOID = "SHIPPING_STATUS" }
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

        public FileMetadata GetMetadatafile(ETCTNUploadRequest ETCTNUploadRequestModel, UploadFileModel UploadFile, int userId)
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

            return metadataFile;
        }

        public string GetCsvUploadKey(string id)
        {
            var dateKey = $"{DateTime.Now.Year}-{DateTime.Now.Month}/{DateTime.Now.Day}";

            return $"{_uploadSettings.FilesUploadPath}/{dateKey}/{id}.csv";
        }

        public string GetMetadataFileUploadKey(string id)
        {
            var dateKey = $"{DateTime.Now.Year}-{DateTime.Now.Month}/{DateTime.Now.Day}";

            return $"{_uploadSettings.MetadataUploadPath}/{dateKey}/{id}.json";
        }

        public async Task<List<FileIngestRequest>?> GetAllRequestsOfUser(int userId)
        {
            var requests = await _dynamoDbService.GetAllRequestsOfUser(userId);

            return requests;
        }

        public async Task<List<ReceivingStatusFileData>?> GetReceivingStatusFileData(string requestId)
        {
            var requests = await _dynamoDbService.GetAllReceivingStatusData(requestId);

            return requests;
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