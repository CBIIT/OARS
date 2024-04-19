using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2;
using TheradexPortal.Data.Models;
using TheradexPortal.Data.Services.Abstract;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;

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
            //var conditions = new List<ScanCondition>
            //    {
            //        new ScanCondition("UserId", ScanOperator.Equal, new string[1] { userId.ToString() })
            //    };

            //var records = await (_dynamoDbContext.ScanAsync<FileIngestRequest>(conditions).GetNextSetAsync());

            //return records;

            var search = _dynamoDbContext.ScanAsync<FileIngestRequest>
            (
              new[] {
                new ScanCondition
                  (
                    nameof(FileIngestRequest.UserId),
                    ScanOperator.Equal,
                    userId.ToString()
                  )
              }
            );

            var result = await search.GetRemainingAsync();

            return result;

            //QueryRequest queryRequest = new QueryRequest
            //{
            //    TableName = "FileIngestRequest",
            //    IndexName = "UserId-index",
            //    KeyConditionExpression = "UserId = :v_userid",
            //    //ExpressionAttributeNames = new Dictionary<String, String> {
            //    //    {"#dt", "Date"}
            //    //},
            //    ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
            //        {":v_userid", new AttributeValue { S =  userId.ToString() }}
            //    },
            //    ScanIndexForward = true
            //};

            //var response = await _dynamoDbContext.QueryAsync(queryRequest);

            //var request = new ScanRequest
            //{
            //    TableName = "FileIngestRequest",
            //    IndexName = "UserId-index",
            //    FilterExpression = "UserId = :value",
            //    ExpressionAttributeValues = new Dictionary<string, AttributeValue> { { userId.ToString(), new AttributeValue { S = userId.ToString() } } }
            //};

            //var response = await _dynamoDbContext.Cli.ScanAsync(request);

            //return response.items;
        }

        public async Task<List<ReceivingStatusFileData>?> GetAllReceivingStatusData(string requestId)
        {
            //var conditions = new List<ScanCondition>
            //    {
            //        new ScanCondition("RequestId", ScanOperator.Equal, new string[1] { requestId })
            //    };

            //var records = await (_dynamoDbContext.ScanAsync<ReceivingStatusFileData>(conditions).GetNextSetAsync());

            //return records;

            var search = _dynamoDbContext.ScanAsync<ReceivingStatusFileData>
            (
              new[] {
                            new ScanCondition
                              (
                                nameof(ReceivingStatusFileData.RequestId),
                                ScanOperator.Equal,
                                requestId
                              )
                    }
            );

            var result = await search.GetRemainingAsync();

            return result;
        }
    }
}
