using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using Org.BouncyCastle.Asn1.Mozilla;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http.Features;
using backEnd.services;
using Newtonsoft.Json;

namespace backEnd.Models;


public class Message{
    
[Key]
[JsonPropertyName("_id")]
public int Id { get; set; }

[JsonPropertyName("status")]
public string? Status {get; set;} = string.Empty;

[JsonPropertyName("content")]
public string? Content {get; set;} = string.Empty;

[JsonPropertyName("requestId")]
public int? RequestId {get; set;}

[JsonPropertyName("request")]
public virtual Request? Request {get; set;} = null; 


}


public class Request{

[JsonPropertyName("_id")]
[Key]
public int Id { get; set; }


[JsonPropertyName("custom")]
public bool? Custom {get; set;} = false;

[NotMapped]
[JsonPropertyName("objectives")]
public List<string>? Objectives {get; set;} = new List<string>(){
     " "
};

[JsonPropertyName("meetings")]
public List<Meeting>? Meetings {get; set;} = new List<Meeting>(){
     new(){
          Subject="",
          Attendees= new List<string>(),
          Agenda = ""
     }
};

[NotMapped]
[JsonPropertyName("items")]
public List<string>? Items {get; set;} = new List<string>(){
     " "
};

[NotMapped]
[JsonPropertyName("personnel")]
public List<string>? Personnel {get; set;} = new List<string>(){
     " "
};
        
[JsonPropertyName("destination")]
public string? Destination { get; set; }  = string.Empty;

[JsonPropertyName("purpose")]
public string? Purpose { get; set; }  = string.Empty;

[JsonPropertyName("mode")]
public string? Mode { get; set; } = string.Empty;

[JsonPropertyName("accomodationRequired")]
public string? AccomodationRequired { get; set; }  = string.Empty;

[JsonPropertyName("numberOfNights")]
public string? NumberOfNights { get; set; } = string.Empty;

[NotMapped]
[JsonPropertyName("costs")]
public virtual List<Cost>? Costs { get; set; } = new List<Cost>();

[JsonPropertyName("totalCost")]
public int? TotalCost { get; set; }  = 0;

[JsonPropertyName("requesterId")]
public int? RequesterId {get; set;}

[NotMapped]
private string _number;

[NotMapped]
[JsonPropertyName("requester")]
public virtual User? Requester {get; set;}  

[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
[JsonPropertyName("number")]
public string? Number {get; set;} = string.Empty;

[JsonPropertyName("status")]
public string? Status {get; set;} = string.Empty; 

[NotMapped]
[JsonPropertyName("quotations")]
public virtual List<Quotation>? Quotations {get; set;}  = new List<Quotation>();

[NotMapped]
[JsonPropertyName("hotelQuotations")]
public virtual List<HotelQuotation>? HotelQuotations {get; set;}  = new List<HotelQuotation>();
    
[JsonPropertyName("agentNumbers")]
public int? AgentNumbers {get; set;} = 0;

[JsonPropertyName("currentHandlerId")]
public int? CurrentHandlerId {get; set;} 

[NotMapped]
[JsonPropertyName("currentHandler")]
public virtual User? CurrentHandler {get; set;} 

[JsonPropertyName("date")]
public string? Date {get; set;}

[JsonPropertyName("startDate")]
public string? StartDate {get; set;} 

[JsonPropertyName("endDate")]
public string? EndDate {get; set;}

[JsonPropertyName("booked")]
public bool? Booked {get; set;}  = false;

[JsonPropertyName("confirmed")]
public bool? Confirmed {get; set;}  = false;

[JsonPropertyName("selected")]
public bool? Selected {get; set;} = false;


[JsonPropertyName("beingProcessed")]
public bool? BeingProcessed {get; set;} = false;


 [JsonPropertyName("processed")]
public bool? Processed {get; set;} = false;


 [JsonPropertyName("seekingInvoices")]
public bool? SeekingInvoices {get; set;}  = false;


 [JsonPropertyName("seekingHotelInvoices")]
public bool? SeekingHotelInvoices {get; set;} = false;



 [JsonPropertyName("inTrip")]
public bool? InTrip {get; set;} = false;


 [JsonPropertyName("best")]
public string? Best {get; set;} = string.Empty;



 [JsonPropertyName("bestHotel")]
public string? BestHotel {get; set;}  = string.Empty;


 [JsonPropertyName("hotelBooked")]
public bool? HotelBooked {get; set;}  = false;


 [JsonPropertyName("hotelConfirmed")]
public bool? HotelConfirmed {get; set;}  = false;



 [JsonPropertyName("ticketInvoiceUploaded")]
public bool? TicketInvoiceUploaded {get; set;} = false;


 [JsonPropertyName("hotelInvoiceUploaded")]
public bool? HotelInvoiceUploaded {get; set;} = false;


[JsonPropertyName("budgetId")]
public int? BudgetId {get; set;}

[JsonPropertyName("budget")]
public Budget? Budget {get; set;} = new Budget();

[JsonPropertyName("requestBudget")]
public RequestBudget? RequestBudget {get; set;} = new RequestBudget();


[JsonPropertyName("expenseReportGiven")]
public bool? ExpenseReportGiven {get; set;} = false;


[JsonPropertyName("expenseReport")]
public virtual ExpenseReport? ExpenseReport {get; set;}


[JsonPropertyName("departmentHeadApproved")]
public bool? DepartmentHeadApproved {get; set;} = false;


[JsonPropertyName("supervisorApproved")]
public bool? SupervisorApproved {get; set;} = false;

[JsonPropertyName("permanentlyRejected")]
public bool? PermanentlyRejected {get; set;} = false;

[NotMapped]
[JsonPropertyName("messages")]
public virtual List<Message>? Messages {get; set;}  = new List<Message>();

//  [JsonPropertyName("agents")]
// public List<Agent>? Agents {get; set;}

[NotMapped]
[JsonPropertyName("agents")]
public virtual List<Agent>? Agents {get; set;} = new List<Agent>();


[JsonPropertyName("logs")]
public virtual List<Log>? Logs {get; set;} = new List<Log>();


[JsonPropertyName("activities")]
public virtual List<Activity>? Activities {get; set;} = new List<Activity>(){
     new (){
          Date= " ",
          Description=" "
     }
};

  

}






public class TicketInvoice {


     [JsonPropertyName("_id")]
     [Key]

    public int Id {get; set;}
    

     [JsonPropertyName("type")]
    public string? Type {get; set;}  = string.Empty;

     
     [JsonPropertyName("filename")]
     public string? Filename {get; set;} = string.Empty;


     [JsonPropertyName("filePath")]
     public string? FilePath {get; set;} = string.Empty;


     public virtual List<Quotation> Quotations {get; set;} = new List<Quotation>();

}


public class HotelInvoice {


     [JsonPropertyName("_id")]
     [Key]

    public int Id {get; set;}
    

     [JsonPropertyName("type")]
    public string? Type {get; set;}  = string.Empty;

     
     [JsonPropertyName("filename")]
     public string? Filename {get; set;} = string.Empty;


     [JsonPropertyName("filePath")]
     public string? FilePath {get; set;} = string.Empty;


     public virtual List<HotelQuotation> Quotations {get; set;} = new List<HotelQuotation>();

}









