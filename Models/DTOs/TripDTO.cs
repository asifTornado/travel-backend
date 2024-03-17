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
    
    [JsonPropertyName("destination")]
    public string? Destination {get; set;}  

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

    [JsonPropertyName("custom")]
    public bool Custom {get; set;} = false;

    [JsonPropertyName("requests")]
    public List<Request>? Requests {get; set;}
    public List<Message> Messages { get; set; } = new List<Message>();
    public List<Quotation> Quotations { get; set; } = new List<Quotation>();
    public List<HotelQuotation> HotelQuotations { get; set; } = new List<HotelQuotation>();
    public List<User> TicketApprovers { get; set; } = new List<User>();
    public List<User> HotelApprovers { get; set; } = new List<User>();
    
    [JsonPropertyName("ticketApprovals")]
    public List<User>? TicketApprovals {get; set;} = new List<User>();

    [JsonPropertyName("hotelApprovals")]
    public List<User>? HotelApprovals {get; set;} = new List<User>();

    [JsonPropertyName("ticketsApprovedByAccounts")]
    public bool? TicketsApprovedByAccounts {get; set;} = false;

    [JsonPropertyName("seekingAccountsApprovalForTickets")]
    public bool? SeekingAccountsApprovalForTickets {get; set;} = false;

    [JsonPropertyName("currentHandlerId")]
    public int? CurrentHandlerId {get; set;} 

      [JsonPropertyName("ticketsMoneyDisbursed")]
    public bool? TicketsMoneyDisbursed {get; set;} = false;

    [JsonPropertyName("amountDisbursedTickets")]
    public string? AmountDisbursedTickets {get; set;}

    [JsonPropertyName("ticketsAccountNumber")]
    public string? TicketsAccountNumber {get; set;}
    
    [JsonPropertyName("ticketsAccountHolderName")]
    public string? TicketsAccountHolderNumber {get; set;}

    [JsonPropertyName("beingProcessed")]
    public bool? BeingProcessed {get; set;}

   [JsonPropertyName("beingProcessedAccounts")]
    public bool? BeingProcessedAccounts {get; set;} = false;

     [JsonPropertyName("beingProcessedAudit")]
    public bool? BeingProcessedAudit {get; set;} = false;
    
     [JsonPropertyName("AccountsProcessed")]
    public bool? AccountsProcessed {get; set;} = false;

        [JsonPropertyName("AuditProcessed")]
    public bool? AuditProcessed {get; set;} = false;


    [JsonPropertyName("currentAccountsHandlerId")]
    public int? CurrentAccountsHandlerId {get; set;}
    
    [JsonPropertyName("currentAuditHandlerId")]
    public int? CurrentAuditHandlerId {get; set;}

    [JsonPropertyName("accountsPrevHandlerIds")]
    public List<int>? AccountsPrevHandlerIds {get; set;} = new List<int>();

    [JsonPropertyName("auditPrevHandlerIds")]
    public List<int>? AuditPrevHandlerIds {get; set;} = new List<int>();

    
   
      
}