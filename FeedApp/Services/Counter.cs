using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ASP.FeedApp.API.Models
{
    public class Counter
    {
        [BsonId]
        public string Id { get; set; }

        public int SequenceValue { get; set; }
    }
}