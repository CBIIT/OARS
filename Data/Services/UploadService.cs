using Amazon.S3.Transfer;
using Amazon.S3;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using TheradexPortal.Data.Models;
using TheradexPortal.Data.Services.Abstract;
using Amazon;
using System.Text;

namespace TheradexPortal.Data.Services
{
    public class UploadService : BaseService, IUploadService
    {
        private readonly IErrorLogService _errorLogService;
        private readonly NavigationManager _navManager;

        public UploadService(IDbContextFactory<ThorDBContext> dbFactory, IErrorLogService errorLogService, NavigationManager navigationManager) : base(dbFactory)
        {
            _errorLogService = errorLogService;
            _navManager = navigationManager;
        }

        public List<MedidataDictionaryModel> GetAssaysToUpload()
        {
            return new List<MedidataDictionaryModel>
            {
                new MedidataDictionaryModel{CodedData = "BCA", UserData = "BCA" },
                new MedidataDictionaryModel{CodedData = "Bioanalyzer", UserData = "Bioanalyzer" },
                new MedidataDictionaryModel{CodedData = "Bradford", UserData = "Bradford" },
                new MedidataDictionaryModel{CodedData = "E-gel", UserData = "E-gel" },
                new MedidataDictionaryModel{CodedData = "Picogreen", UserData = "Picogreen" },
                new MedidataDictionaryModel{CodedData = "Quant-iT", UserData = "Quant-iT" },
                new MedidataDictionaryModel{CodedData = "Qubit", UserData = "Qubit" },
                new MedidataDictionaryModel{CodedData = "Tapestation", UserData = "Tapestation" },
                new MedidataDictionaryModel{CodedData = "Nanodrop (Spec)", UserData = "Nanodrop (Spec)" }
            };
        }

        public List<CRFModel> GetCRFsToUpload()
        {
            return new List<CRFModel>
            {
                new CRFModel{FormName = "TSO500 Library QC", FormOID = "LIBRARY_QC" },
                new CRFModel{FormName = "TSO500 Sequencing QC", FormOID = "SEQUENCING_QC" },
                new CRFModel{FormName = "Biospecimen Roadmap", FormOID = "BIOSPECIMEN_ROADMAP" },
                new CRFModel{FormName = "Receiving Status", FormOID = "RECEIVING_STATUS" },
                new CRFModel{FormName = "Shipping Status", FormOID = "SHIPPING_STATUS" }
            };
        }

        public List<MedidataDictionaryModel> GetLabsToUpload()
        {
            return new List<MedidataDictionaryModel>
            {
                new MedidataDictionaryModel{CodedData = "MDACC Tissue Qualification Laboratory (TQL) for HER2", UserData = "MDACC Tissue Qualification Laboratory (TQL) for HER2"},
                new MedidataDictionaryModel{CodedData = "NCLN Genomics Laboratory at MD Anderson for WES & RNAseq", UserData = "NCLN Genomics Laboratory at MD Anderson for WES & RNAseq" },
                new MedidataDictionaryModel{CodedData = "Paulovich Laboratory, Fred Hutchinson Cancer Research Center(FHCRC)", UserData = "Paulovich Laboratory,  Fred Hutchinson Cancer Research Center (FHCRC)" },
                new MedidataDictionaryModel{CodedData = "PPD Laboratories", UserData = "PPD Laboratories" },
                new MedidataDictionaryModel{CodedData = "Cancer Pharmacokinetics and Pharmacodynamics Facility University of Pittsburgh Cancer Institute", UserData = "Cancer Pharmacokinetics and Pharmacodynamics FacilityUniversity of Pittsburgh Cancer Institute" },
                new MedidataDictionaryModel{CodedData = "Kaufmann Laboratory Department of Molecular Pharmacology and Experimental Therapeutics Mayo Clinic", UserData = "Kaufmann Laboratory Department of Molecular Pharmacology and Experimental Therapeutics Mayo Clinic" },
                new MedidataDictionaryModel{CodedData = "PADIS FNLCR", UserData = "PADIS FNLCR" },
                new MedidataDictionaryModel{CodedData = "NCLN PD Assay Laboratory at MD Anderson", UserData = "NCLN PD Assay Laboratory at MD Anderson" },
                new MedidataDictionaryModel{CodedData = "EET Biobank", UserData = "EET Biobank" },
                new MedidataDictionaryModel{CodedData = "Frederick MoCha Lab", UserData = "Frederick MoCha Lab" },
                new MedidataDictionaryModel{CodedData = "NCLN Genomics Lab", UserData = "NCLN Genomics Laboratory" },
                new MedidataDictionaryModel{CodedData = "NCLN PD Assay Lab at MDA", UserData = "NCLN PD Assay Laboratory at MD Anderson" },
                new MedidataDictionaryModel{CodedData = "Williams Laboratory at City of Hope", UserData = "Williams Laboratory at City of Hope" }
            };
        }

        public List<string> GetStudiesToUpload()
        {
            var studies = new List<string>();

            studies.Add("10323");

            return studies;
        }

        public async Task<bool> UploadCsvFileToS3(string bucket, string key, int userId, MemoryStream memoryStream)
        {
            try
            {
                using (var client = new AmazonS3Client(RegionEndpoint.USEast1))
                {
                    var uploadRequest = new TransferUtilityUploadRequest
                    {
                        InputStream = memoryStream,
                        Key = key,
                        BucketName = bucket
                    };

                    var fileTransferUtility = new TransferUtility(client);
                    await fileTransferUtility.UploadAsync(uploadRequest);
                    return true;
                }
            }
            catch (Exception ex)
            {
                _errorLogService.SaveErrorLogAsync(userId, _navManager.Uri, ex.InnerException, ex.Source, ex.Message, ex.StackTrace);
                return false;
            }
        }

        public async Task<bool> UploadMetatdataFileToS3(string bucket, string key, int userId, string content)
        {
            try
            {
                using (var client = new AmazonS3Client(RegionEndpoint.USEast1))
                {
                    byte[] data = Encoding.ASCII.GetBytes(content);
                    MemoryStream memoryStream = new MemoryStream(data, 0, data.Length);

                    var uploadRequest = new TransferUtilityUploadRequest
                    {
                        InputStream = memoryStream,
                        Key = key,
                        BucketName = bucket,
                        
                    };

                    var fileTransferUtility = new TransferUtility(client);
                    await fileTransferUtility.UploadAsync(uploadRequest);
                    return true;
                }
            }
            catch (Exception ex)
            {
                _errorLogService.SaveErrorLogAsync(userId, _navManager.Uri, ex.InnerException, ex.Source, ex.Message, ex.StackTrace);
                return false;
            }
        }
    }
}