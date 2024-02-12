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
    public List<User>? Approvals {get; set;}


    [JsonPropertyName("currentHandlerId")]
    public int? CurrentHandlerId {get; set;} 

    [JsonPropertyName("prevHandlerId")]
    public int? PrevHandlerId {get; set;}

    [NotMapped]
    [JsonPropertyName("currentHandler")]
    public virtual User? CurrentHandler {get; set;} 

    [JsonPropertyName("request")]
    public virtual Request? Request {get; set;}

    [JsonPropertyName("requestId")]
    public int? RequestId {get; set;}
    
}







