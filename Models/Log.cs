using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace backEnd.Models;


public class Log{

    [Key]
    [JsonPropertyName("_Id")]
    public int Id {get; set;}

    [JsonPropertyName("Date")]
    public string Date {get; set;} = string.Empty;

    [JsonPropertyName("Event")]
    public string Event {get; set;} = string.Empty;

    [JsonPropertyName("FromId")]
    public int? FromId {get; set;}


    [JsonPropertyName("ToId")]
    public int? ToId {get; set;}


    [JsonPropertyName("RequestId")]
    public int? RequestId {get; set;}


    [JsonPropertyName("Request")]
    public Request? Request {get; set;}

   
}