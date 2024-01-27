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
using System.ComponentModel.Design;
using backEnd.Models.IModels;

namespace backEnd.Models;








public class Quotation{


    [JsonPropertyName("_id")]
    [Key]
    public int Id { get; set; }



    [JsonPropertyName("linker")]
    public Guid? Linker {get; set;}
  

  //  [JsonPropertyName("agentId")]
  // public int? AgentId {get; set;} 

  //  [JsonPropertyName("agent")]
  // public virtual Agent? Agent {get; set;} 

  [JsonPropertyName("quoteGiver")]
public string? QuoteGiver {get; set;} = string.Empty;


   [JsonPropertyName("quotationText")]
  public string? QuotationText {get; set;}  = string.Empty;


 [JsonPropertyName("selected")]
 public bool? Selected {get; set;} = false;


 [JsonPropertyName("booked")]
 public bool? Booked {get; set;} = false;



 [JsonPropertyName("confirmed")]
 public bool? Confirmed {get; set;} = false;


 [JsonPropertyName("hovered")]
 public bool? Hovered {get; set;} = false;


 [JsonPropertyName("custom")]
 public bool? Custom {get; set;} = false;


 [JsonPropertyName("requestIds")]
  public List<int>? RequestIds {get; set;} = new List<int>();

 

  [JsonPropertyName("requestId")]
  public int? RequestId {get; set;}


[JsonPropertyName("request")] 
 public virtual Request? Request {get; set;}


 [JsonPropertyName("invoices")]
 public virtual List<TicketInvoice>? Invoices {get; set;}

 [NotMapped]
[JsonPropertyName("ticketApprovals")]
public virtual List<User>? TicketApprovals {get; set;}  = new List<User>();


[JsonPropertyName("approved")]
public bool? Approved {get; set;} = false;


[JsonPropertyName("totalCosts")]
public List<TravelerCost> TotalCosts = new List<TravelerCost>();


}






public class HotelQuotation{

    
  [JsonPropertyName("_id")]
  [Key]
  public int Id { get; set; }
        
//  [JsonPropertyName("agentId")]
//  public int? AgentId {get; set;} = 0;


//   [JsonPropertyName("agent")]
//   public virtual Agent? Agent {get; set;} = new Agent();


    [JsonPropertyName("linker")]
    public Guid? Linker {get; set;}


    
 [JsonPropertyName("requestIds")]
  public List<int>? RequestIds {get; set;} = new List<int>();


[JsonPropertyName("quoteGiver")]
public string? QuoteGiver {get; set;} = string.Empty;


 [JsonPropertyName("quotationText")]
  public string? QuotationText {get; set;}  = string.Empty;


 [JsonPropertyName("selected")]
 public bool? Selected {get; set;} = false;

 [JsonPropertyName("booked")]
 public bool? Booked {get; set;} = false;


 [JsonPropertyName("confirmed")]
 public bool? Confirmed {get; set;} = false;



 
 [JsonPropertyName("hovered")]
 public bool? Hovered {get; set;} = false;
 

 [JsonPropertyName("custom")]
 public bool? Custom {get; set;} = false;


[JsonPropertyName("requestId")]
public int? RequestId {get; set;}



[JsonPropertyName("request")]
public virtual Request? Request {get; set;}


[JsonPropertyName("invoices")]
public virtual List<HotelInvoice>? Invoices {get; set;}


[NotMapped]
[JsonPropertyName("hotelApprovals")]
public  virtual List<User>? HotelApprovals {get; set;} = new List<User>();


[JsonPropertyName("approved")]
public bool? Approved {get; set;} = false;




[JsonPropertyName("totalCosts")]
public List<TravelerCost> TotalCosts = new List<TravelerCost>();




}








