using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ASP.FeedApp.API.Models
{
    public class Users
    {
        [BsonId]
        public int Id { get; set; }

        [BsonElement("username")]
        public string Username { get; set; } = null!;

        [BsonElement("password")]
        public string Password { get; set; } = null!;

        public void HashPassword()
        {
            Password = BCrypt.Net.BCrypt.HashPassword(Password);
        }
        public bool VerifyPassword(string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, Password);
        }
    }
}