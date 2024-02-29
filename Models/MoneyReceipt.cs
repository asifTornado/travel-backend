using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace backEnd.Models;


public class MoneyReceipt{
    
    [Key]
    [JsonPropertyName("_id")]
    public int Id {get; set;} 

     [JsonPropertyName("date")]
    public string? Date {get; set;}
    

     [JsonPropertyName("unit")]
    public string? Unit {get; set;}

     [JsonPropertyName("advanceMoneyInHand")]
    public string? AdvanceMoneyInHand {get; set;}
    

    [JsonPropertyName("section")]
    public string? Section {get; set;}

  
     [JsonPropertyName("i")]
    public string? I {get; set;}
    

     [JsonPropertyName("designation")]
    public string? Designation {get; set;}
    
     [JsonPropertyName("requiredTK")] 
    public string? RequiredTK {get; set;}
    

     [JsonPropertyName("taka")]
    public string? Taka {get; set;}
    

     [JsonPropertyName("asAdvanceAgainst")]
    public string? AsAdvanceAgainst {get; set;}

    [JsonPropertyName("status")]
    public string? Status {get; set;} 
    

     [JsonPropertyName("serialNo")]
    public string? SerialNo {get; set;}

    [JsonPropertyName("processed")]
    public bool? Processed {get; set;} = false;

    [JsonPropertyName("supervisorApproved")]
    public bool? SupervisorApproved {get; set;} = false;


    [JsonPropertyName("submitted")]
    public bool? Submitted {get; set;} = false;

    

     [JsonPropertyName("approvals")]
    public List<User>? Approvals {get; set;} = new List<User>();


    [JsonPropertyName("currentHandlerId")]
    public int? CurrentHandlerId {get; set;} 

    [JsonPropertyName("prevHandlerIds")]
    public List<int>? PrevHandlerIds {get; set;} = new List<int>();

    [NotMapped]
    [JsonPropertyName("currentHandler")]
    public virtual User? CurrentHandler {get; set;} 

    [JsonPropertyName("request")]
    public virtual Request? Request {get; set;}

    [JsonPropertyName("requestId")]
    public int? RequestId {get; set;}


    [JsonPropertyName("rejected")]
    public bool? Rejected {get; set;} = true;

    [JsonPropertyName("disbursed")]
    public bool? Disbursed {get; set;} = false;

 
    [JsonPropertyName("amountDisbursed")]
    public string? AmountDisbursed {get; set;} 

    [JsonPropertyName("bankAccountHolderName")]
    public string? BankAccountHolderName {get; set;}

    [JsonPropertyName("bankAccountNumber")]
    public string? BankAccountNumber {get; set;}
    
}






