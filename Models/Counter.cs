using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;

namespace backEnd.Models
{
    public class Counter
    {
   
        [BsonElement("_id")]
        [JsonPropertyName("_id")]
        public string? Id { get; set; }


        [BsonElement("count")]
        [JsonPropertyName("count")]
        public int Count { get; set; } = 0;
    }


    public class UserCounter{
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("_id")]
        [JsonPropertyName("_id")]
        public string? Id { get; set; }


        [BsonElement("count")]
        [JsonPropertyName("count")]
        public string Count { get; set; } = "0";

    }
}
