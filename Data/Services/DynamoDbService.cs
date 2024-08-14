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

        public async Task<List<FileIngestRequest>?> GetAllRequestsOfUser(int userId, bool isAdminDisplay, string environment)
        {
            var conditions = new List<ScanCondition>();

            if (!isAdminDisplay)
            {
                conditions.Add(new ScanCondition
                  (
                    nameof(FileIngestRequest.UserId),
                    ScanOperator.Equal,
                    userId.ToString()
                  ));
            }

            conditions.Add(new ScanCondition
              (
                nameof(FileIngestRequest.Environment),
                ScanOperator.Equal,
                environment.ToString()
              ));

            var search = _dynamoDbContext.ScanAsync<FileIngestRequest>(conditions);

            var result = await search.GetRemainingAsync();

            return result?.OrderByDescending(p => p.CreatedDate).ToList();

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

            return result?.OrderBy(p => p.RowNo).ToList();
        }

        public async Task<List<BiospecimenRoadmapFileData>?> GetAllBiospecimenRoadmapData(string requestId)
        {
            var search = _dynamoDbContext.ScanAsync<BiospecimenRoadmapFileData>
            (
              new[] {
                            new ScanCondition
                              (
                                nameof(BiospecimenRoadmapFileData.RequestId),
                                ScanOperator.Equal,
                                requestId
                              )
                    }
            );

            var result = await search.GetRemainingAsync();

            return result?.OrderBy(p => p.RowNo).ToList();
        }

        public async Task<List<ShippingStatusFileData>?> GetAllShippingStatusData(string requestId)
        {
            //var conditions = new List<ScanCondition>
            //    {
            //        new ScanCondition("RequestId", ScanOperator.Equal, new string[1] { requestId })
            //    };

            //var records = await (_dynamoDbContext.ScanAsync<ShippingStatusFileData>(conditions).GetNextSetAsync());

            //return records;

            var search = _dynamoDbContext.ScanAsync<ShippingStatusFileData>
            (
              new[] {
                            new ScanCondition
                              (
                                nameof(ShippingStatusFileData.RequestId),
                                ScanOperator.Equal,
                                requestId
                              )
                    }
            );

            var result = await search.GetRemainingAsync();

            return result?.OrderBy(p => p.RowNo).ToList();
        }

        public async Task<List<TSO500SequencingQCFileData>?> GetAllTSO500SequencingQCData(string requestId)
        {
            //var conditions = new List<ScanCondition>
            //    {
            //        new ScanCondition("RequestId", ScanOperator.Equal, new string[1] { requestId })
            //    };

            //var records = await (_dynamoDbContext.ScanAsync<TSO500SequencingQCFileData>(conditions).GetNextSetAsync());

            //return records;

            var search = _dynamoDbContext.ScanAsync<TSO500SequencingQCFileData>
            (
              new[] {
                            new ScanCondition
                              (
                                nameof(TSO500SequencingQCFileData.RequestId),
                                ScanOperator.Equal,
                                requestId
                              )
                    }
            );

            var result = await search.GetRemainingAsync();

            return result?.OrderBy(p => p.RowNo).ToList();
        }

        public async Task<List<TSO500LibraryQCFileData>?> GetAllTSO500LibraryQCData(string requestId)
        {
            //var conditions = new List<ScanCondition>
            //    {
            //        new ScanCondition("RequestId", ScanOperator.Equal, new string[1] { requestId })
            //    };

            //var records = await (_dynamoDbContext.ScanAsync<TSO500LibraryQCFileData>(conditions).GetNextSetAsync());

            //return records;

            var search = _dynamoDbContext.ScanAsync<TSO500LibraryQCFileData>
            (
              new[] {
                            new ScanCondition
                              (
                                nameof(TSO500LibraryQCFileData.RequestId),
                                ScanOperator.Equal,
                                requestId
                              )
                    }
            );

            var result = await search.GetRemainingAsync();

            return result?.OrderBy(p => p.RowNo).ToList();
        }
    }
}