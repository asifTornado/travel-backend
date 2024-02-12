using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;





namespace backEnd.Models.DTOs;


public class BudgetDTO{
      [Key]
    [JsonPropertyName("_id")]
    public int Id { get; set; }

    [JsonPropertyName("tripId")]
    public string? TripId {get; set;} = string.Empty;

    [JsonPropertyName("subject")]
    public string? Subject {get; set;} = string.Empty;

  
    
    // [JsonPropertyName("travellerName")]
    // public string? TravellerName {get; set;} 
        
    // [JsonPropertyName("travellerEmail")]
    // public string? TravellerEmail {get; set;} 
    
    // [JsonPropertyName("travelMode")]
    // public string? TravelMode {get; set;} 

    
    [JsonPropertyName("destination")]
    public string? Destination {get; set;} 

    
    // [JsonPropertyName("purpose")]
    // public string? Purpose {get; set;} 

    
    [JsonPropertyName("departure_date")]
    public string? DepartureDate {get; set;} 

    
    [JsonPropertyName("arrival_date")]
    public string? ArrivalDate {get; set;} 




    


    
}