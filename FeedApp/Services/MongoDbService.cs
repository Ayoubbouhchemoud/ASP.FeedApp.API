using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using ASP.FeedApp.API.Models;

namespace ASP.FeedApp.API.Services
{
    public class MongoDbService
    {
        private readonly IMongoDatabase _database;

        public MongoDbService(IConfiguration configuration)
        {
            var connectionString = configuration.GetSection("MongoDB:ConnectionString").Value;
            var databaseName = configuration.GetSection("MongoDB:DatabaseName").Value;

            if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(databaseName))
            {
                throw new ArgumentNullException("MongoDB configuration is missing or incorrect.");
            }

            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        public IMongoCollection<Users> GetUsersCollection()
        {
            return _database.GetCollection<Users>("Users");
        }

        public IMongoCollection<Counter> GetCountersCollection()
        {
            return _database.GetCollection<Counter>("Counters");
        }

        public async Task<int> GetNextSequenceValue(string collectionName)
        {
            var countersCollection = GetCountersCollection();

            var filter = Builders<Counter>.Filter.Eq(c => c.Id, collectionName);
            var update = Builders<Counter>.Update.Inc(c => c.SequenceValue, 1);
            var options = new FindOneAndUpdateOptions<Counter>
            {
                ReturnDocument = ReturnDocument.After,
                IsUpsert = true
            };

            var counter = await countersCollection.FindOneAndUpdateAsync(filter, update, options);
            return counter.SequenceValue;
        }

    }
}
