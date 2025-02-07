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
using TheradexPortal.Data.Models.ADDR;
using ClosedXML.Excel;
using System.Text.Json;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.ExtendedProperties;
using TheradexPortal.Data.Services.Abstract.ADDR;
using TheradexPortal.Pages.ADDR;
using Microsoft.Extensions.Hosting.Internal;
using System.Text.Json.Nodes;

namespace TheradexPortal.Data.Services
{
    public class ShippingStatusService : BaseService, IShippingStatusService, INotesService<ShippingStatus>
    {
        private readonly IErrorLogService _errorLogService;
        private readonly NavigationManager _navManager;
        private readonly IDynamoDbService _dynamoDbService;
        private readonly IAWSS3Service _awsS3Service;
        private readonly IStudyService _studyService;
        private readonly ILogger<ShippingStatusService> logger; // Add logger field
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ShippingStatusService(ILogger<ShippingStatusService> logger,
            IDatabaseConnectionService databaseConnectionService,
            IErrorLogService errorLogService,
            NavigationManager navigationManager,
            IDynamoDbService dynamoDbService,
            IStudyService studyService,
            IAWSS3Service awsS3Service,
            IWebHostEnvironment webHostEnvironment,
            IHttpContextAccessor httpContextAccessor) : base(databaseConnectionService)
        {
            _errorLogService = errorLogService;
            _navManager = navigationManager;
            _dynamoDbService = dynamoDbService;
            _awsS3Service = awsS3Service;
            _studyService = studyService;
            _webHostEnvironment = webHostEnvironment;
            this.logger = logger; // Initialize logger
        }
        public async Task<List<ShippingStatus>?> GetShippingStatus(string protocalNumber)
        {
            var data = await GeShippingStatusExcel();
            return data.Where(i => i.ProtocolNumber == protocalNumber).ToList();
        }

        public async Task<List<ShippingStatus>?> GeShippingStatusExcel(string filePath = "~/addr/difference_report.xlsx")
        {
            var shippingStatusList = new List<ShippingStatus>();
            //using (var workbook = new XLWorkbook(filePath))
            //{
            //    try
            //    {
            //        var worksheet = workbook.Worksheet("I-RECEIVING_STATUS");

            //        // Get the headers and create a dictionary to map column names to their indices
            //        var headerRow = worksheet.Row(1); // Assuming the first row contains headers
            //        var headerDictionary = headerRow.Cells()
            //            .ToDictionary(cell => cell.GetValue<string>(), cell => cell.Address.ColumnNumber - 1);

            //        // Skip the first 2 rows (headers), adjust if necessary
            //        foreach (var row in worksheet.RowsUsed().Skip(2))
            //        {
            //            var receivingStatus = new ReceivingStatus();

            //            // Assign values using 1-based indexes (start from 1)
            //            receivingStatus.DataSource = row.Cell(1).GetValue<string>(); // DATASOURCE
            //            receivingStatus.SystemComments = row.Cell(2).GetValue<string>(); // VARI Comments
            //            receivingStatus.VariCount = row.Cell(3).GetValue<string>(); // VariCount
            //            receivingStatus.RaveCount = row.Cell(4).GetValue<string>(); // RaveCount
            //            receivingStatus.SubjectKey = row.Cell(5).GetValue<string>(); // SUBJECTKEY
            //            receivingStatus.SiteId = row.Cell(6).GetValue<string>(); // SITEID
            //            receivingStatus.SpecimenId = row.Cell(7).GetValue<string>(); // Specimen_Sponsor_Group_Specimen_ID
            //            receivingStatus.RegisteredPatientId = row.Cell(8).GetValue<string>(); // Reg_Patient_ID
            //            receivingStatus.CarrierTrackingNo = row.Cell(9).GetValue<string>(); // Submission_Carrier_Tracking_No
            //            receivingStatus.CarrierName = row.Cell(10).GetValue<string>(); // Submission_Carrier_Name
            //            receivingStatus.PackagingCondition = row.Cell(11).GetValue<string>(); // Submission_Condition_Packaging
            //            receivingStatus.SubSpecimenId = row.Cell(12).GetValue<string>(); // Sub_Specimen_Id
            //            receivingStatus.ReceivingSite = row.Cell(13).GetValue<string>(); // Receiving_Site
            //            receivingStatus.ShippedDate = row.Cell(14).GetValue<string>(); // Shipped_Date
            //            receivingStatus.ReceivedDateTime = row.Cell(15).GetValue<string>(); // Submission_Received_DateTime
            //            receivingStatus.PackagingDetailCondition = row.Cell(16).GetValue<string>(); // Submission_Condition_Packaging_Detail
            //            receivingStatus.Inadreas = row.Cell(17).GetValue<string>(); // INADREAS
            //            receivingStatus.Comments = row.Cell(18).GetValue<string>(); // Comments
            //            receivingStatus.Notes = row.Cell(19).GetValue<string>(); // Notes
            //            receivingStatus.Action = row.Cell(20).GetValue<string>(); // Action
            //            receivingStatus.StudyEventRepeatKey = row.Cell(21).GetValue<string>(); // StudyEventRepeatKey
            //            receivingStatus.ItemGroupRepeatKey = row.Cell(22).GetValue<string>(); // ItemGroupRepeatKey
            //            receivingStatus.TransactionType = row.Cell(23).GetValue<string>(); // TransactionType
            //            receivingStatus.Active = row.Cell(24).GetValue<string>(); // Active

            //            receivingStatus.Id = $"{receivingStatus.SubjectKey}/{receivingStatus.SiteId}/{receivingStatus.SpecimenId}";

            //            receivingStatusList.Add(receivingStatus);
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        logger.LogError(ex, $"An unhandled exception occurred while reading the Excel file.");
            //    }
            //}
            //string jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(receivingStatusList.Take(100), Formatting.Indented);
            //File.WriteAllText("C:\\ManishRathi\\repos\\TheradexGit\\nci-web-reporting\\Pages\\ADDR\\DummyData\\Receiving_Status.json", jsonString);
            //return receivingStatusList;

            // Read the JSON from the file
            string excelFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "addr", "IV-SHIPPING_STATUS.json");
            string jsonString = File.ReadAllText(excelFilePath);

            var settings = new JsonSerializerSettings
                {
                    ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver
                    {
                        NamingStrategy = new Newtonsoft.Json.Serialization.DefaultNamingStrategy()
                    }
                };
            
            shippingStatusList = JsonConvert.DeserializeObject<List<ShippingStatus>>(jsonString,settings);
            // Add "ProtocolNumber": "10323" to each object
            foreach (var item in shippingStatusList)
            {
                item.ProtocolNumber = "10323";
            }

            return shippingStatusList;

        }

        public Task SaveNotesAsync(int statusId, Note<ShippingStatus> note)
        {
            throw new NotImplementedException();
        }

        public Task<ShippingStatus> GetNoteByIdAsync(int id)
        {
            return Task.FromResult(new ShippingStatus());
        }

        public Task<List<Note<ShippingStatus>>> GetNotesByStatusIdAsync(int statusId)
        {
            return Task.FromResult(new List<Note<ShippingStatus>>());
        }


    }
}