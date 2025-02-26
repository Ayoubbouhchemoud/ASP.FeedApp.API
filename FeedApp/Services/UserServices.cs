using MongoDB.Driver;
using ASP.FeedApp.API.Models;
using System.Threading.Tasks;

namespace ASP.FeedApp.API.Services
{
    public class UserService
    {
        private readonly IMongoCollection<Users> _usersCollection;
        private readonly MongoDbService _mongoDbService;

        public UserService(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
            _usersCollection = mongoDbService.GetUsersCollection();
        }

        public async Task<Users?> GetUserByUsernameAsync(string username)
        {
            return await _usersCollection.Find(u => u.Username == username).FirstOrDefaultAsync();
        }

        public async Task<Users> CreateUserAsync(string username, string password)
        {
            int userId = await _mongoDbService.GetNextSequenceValue("Users");

            var newUser = new Users
            {
                Id = userId,
                Username = username,
                Password = BCrypt.Net.BCrypt.HashPassword(password)
            };

            await _usersCollection.InsertOneAsync(newUser);
            return newUser;
        }
    }
}
