using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace backEnd.Models.DTOs;
using System.ComponentModel.DataAnnotations.Schema;



public class TripDTO : Trip{
      
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






    [JsonPropertyName("travelers")]
    public List<User>? Travelers {get; set;}


    [JsonPropertyName("requests")]
    public List<Request>? Requests {get; set;}




    



      public List<Message> Messages { get; set; } = new List<Message>();
      public List<Quotation> Quotations { get; set; } = new List<Quotation>();
      public List<HotelQuotation> HotelQuotations { get; set; } = new List<HotelQuotation>();

      public List<User> TicketApprovers { get; set; } = new List<User>();
      public List<User> HotelApprovers { get; set; } = new List<User>();
      
}