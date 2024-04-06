using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2;
using TheradexPortal.Data.Models;
using TheradexPortal.Data.Services.Abstract;

namespace TheradexPortal.Data.Services
{
    public class DynamoDbService : IDynamoDbService
    {
        private const int BatchSize = 25;
        protected readonly IDynamoDBContext _dynamoDbContext;
        protected readonly ILogger<DynamoDbService> _logger;

        public DynamoDbService(ILogger<DynamoDbService> logger, IDynamoDBContext dynamoDbContext)
        {
            _logger = logger;

            _dynamoDbContext = dynamoDbContext;
        }

        public async Task<List<FileIngestRequest>?> GetAllRequestsOfUser(int userId)
        {
            var conditions = new List<ScanCondition>
                {
                    new ScanCondition("UserId", ScanOperator.Equal, new string[1] { userId.ToString() })
                };

            var records = await (_dynamoDbContext.ScanAsync<FileIngestRequest>(conditions).GetNextSetAsync());

            return records;
        }

        public async Task<List<ReceivingStatusFileData>?> GetAllReceivingStatusData(string requestId)
        {
            var conditions = new List<ScanCondition>
                {
                    new ScanCondition("RequestId", ScanOperator.Equal, new string[1] { requestId })
                };

            var records = await (_dynamoDbContext.ScanAsync<ReceivingStatusFileData>(conditions).GetNextSetAsync());

            return records;
        }
    }
}
