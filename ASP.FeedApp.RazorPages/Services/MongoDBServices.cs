using MongoDB.Driver;
using ASP.FeedApp.RazorPages.Models;

namespace ASP.FeedApp.RazorPages.Services
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
    }
}
