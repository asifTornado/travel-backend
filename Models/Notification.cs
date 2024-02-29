using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backEnd.Models
{
 
    public class Notification
    {


    [JsonPropertyName("_id")]
    [Key]

    public int Id { get; set; }
   
    [JsonPropertyName("time")] 
    public string? Time { get; set; } = string.Empty;

     [JsonPropertyName("message")]
    public string? Message { get; set; } = string.Empty;


    [JsonPropertyName("sourceId")]
    public int? SourceId { get; set; } = 0;


    [JsonPropertyName("from")]
    public int? From { get; set; }

    
    [JsonPropertyName("to")]
    public int? To { get; set; } = null; 

    
    [JsonPropertyName("type")]
    public string? Type { get; set; } = "normal";


    [JsonPropertyName("event")]
    public string? Event {get; set;} = string.Empty;



    }





  
}
