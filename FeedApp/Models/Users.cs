using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ASP.FeedApp.API.Models
{
    public class Users
    {
        [BsonId]
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
    }
}
