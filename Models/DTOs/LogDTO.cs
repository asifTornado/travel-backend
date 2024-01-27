using System.Text.Json.Serialization;

namespace backEnd.Models.DTOs;


public class LogDTO{

    [JsonPropertyName("Date")]
    public string Date {get; set;} = string.Empty;

    [JsonPropertyName("Event")]
    public string Event {get; set;} = string.Empty;

    [JsonPropertyName("From")]
    public string From {get; set;} = string.Empty;


    [JsonPropertyName("To")]
    public string To {get; set;} = string.Empty;
}