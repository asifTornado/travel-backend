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


public class RequestBudget{
    
    [JsonPropertyName("travelSupervisor")]
    public string? TravelSupervisor {get; set;} 

    [JsonPropertyName("totalDailyAllowance")]
    public string? TotalDailyAllowance {get; set;} 

    [JsonPropertyName("emergencyFund")]
    public string? EmergencyFund {get; set;} 

    [JsonPropertyName("totalBudget")]
    public string? TotalBudget {get; set;} 

    [JsonPropertyName("approvalStatus")]
    public string? ApprovalStatus {get; set;} 

    [JsonPropertyName("notes")]
    public string? Notes {get; set;} 


    [JsonPropertyName("breakdown")]
    public List<RequestBudgetBreakDown> Breakdown {get; set;} =  new List<RequestBudgetBreakDown>(){
        new RequestBudgetBreakDown()
    };

}


public class RequestBudgetBreakDown {

    [JsonPropertyName("item")]
    public string? Item {get; set;} 

    [JsonPropertyName("quantity")]
    public string? Quantity {get; set;} 
     
     [JsonPropertyName("cost")]
     public string? Cost {get; set;} 
     
     [JsonPropertyName("total")]
     public string? Total {get; set;} 
}