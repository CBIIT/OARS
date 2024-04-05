using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2;
using TheradexPortal.Data.Models;

namespace TheradexPortal.Data.Services
{
    public class DynamoDbService
    {
        private static readonly AmazonDynamoDBClient client = new AmazonDynamoDBClient();

        public DynamoDbService()
        {
            var client = new AmazonDynamoDBClient();
            context = new DynamoDBContext(client);
        }

        public DynamoDBContext context { get; private set; }

        public async Task<FileIngestRequest?> GetRequest(string requestId)
        {
            var record = await context.LoadAsync<FileIngestRequest>(requestId);

            return record;
        }

        public async Task<List<FileIngestRequest>?> GetAllRequestsOfUser(int userId)
        {
            var conditions = new List<ScanCondition>
                {
                    new ScanCondition("UserId", ScanOperator.Equal, userId)
                };

            var records = await (context.ScanAsync<FileIngestRequest>(conditions).GetNextSetAsync());

            return records;
        }

        public async Task<List<ReceivingStatusFileData>?> GetAllReceivingStatusData(string requestId)
        {
            var conditions = new List<ScanCondition>
                {
                    new ScanCondition("RequestId", ScanOperator.Equal, new string[1] { requestId })
                };

            var records = await (context.ScanAsync<ReceivingStatusFileData>(conditions).GetNextSetAsync());

            return records;
        }
    }
}
