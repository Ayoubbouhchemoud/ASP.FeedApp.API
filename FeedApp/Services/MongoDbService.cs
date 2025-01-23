using MongoDB.Driver;
using ASP.FeedApp.API.Models;

namespace ASP.FeedApp.API.Services
{
    public class MongoDbService
    {
        private readonly IMongoDatabase _database;

        public MongoDbService(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetConnectionString("MongoDB"));
            _database = client.GetDatabase(configuration["DatabaseName"]);
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
