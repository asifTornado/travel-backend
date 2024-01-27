using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace backEnd.Models;



public class Budget
{

    
     
    [Key]
    [JsonPropertyName("_id")]
    public int Id { get; set; }

    [JsonPropertyName("tripId")]
    public string? TripId {get; set;} = string.Empty;

    [JsonPropertyName("subject")]
    public string? Subject {get; set;} = string.Empty;

    [JsonPropertyName("brand")]
    public string? Brand {get; set;} = string.Empty;
    
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


    [JsonPropertyName("numberOfDays")]
    public string? NumberOfDays {get; set;} 

    [JsonPropertyName("numberOfTravelers")]
    public string? NumberOfTravelers {get; set;}



        
    [JsonPropertyName("airTicketBudget")]
    public string? AirTicketBudget {get; set;} 


    [JsonPropertyName("hotelBudget")]
    public string? HotelBudget {get; set;}

    [JsonPropertyName("totalBookingCost")]
    public string? TotalBookingCost {get; set;}

    [JsonPropertyName("transportExpense")]
    public string? TransportExpense {get; set;}

    [JsonPropertyName("incidentalExpense")]
    public string? IncidentalExpense {get; set;}

    [JsonPropertyName("totalTripBudget")]
    public string? TotalTripBudget {get; set;}



    [JsonPropertyName("initiated")]
    public string? Initiated {get; set;} = "No";



    [JsonPropertyName("creationDate")]
    public string? CreationDate {get; set;}






    [JsonPropertyName("travelers")]
    public virtual List<User>? Travelers {get; set;}


    [JsonPropertyName("requests")]
    public virtual List<Request>? Requests {get; set;}


    









}






