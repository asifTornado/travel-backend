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

[NotMapped]
public class RequestBudget{
    

  
    [JsonPropertyName("travelSupervisor")]
    public string? TravelSupervisor {get; set;} = string.Empty; 

    [JsonProperty("totalDailyAllowance")]
    public string? TotalDailyAllowance {get; set;} = string.Empty;

    [JsonProperty("totalBudget")]
    public string? TotalBudget {get; set;} = string.Empty;

    [JsonProperty("approvalStatus")]
    public string? ApprovalStatus {get; set;} = string.Empty;

    [JsonProperty("notes")]
    public string? Notes {get; set;} = string.Empty;


    [NotMapped]
    [JsonProperty("breakdown")]
    public List<RequestBudgetBreakDown> Breakdown {get; set;} =  new List<RequestBudgetBreakDown>(){
        new RequestBudgetBreakDown()
    };
}

[NotMapped]
public class RequestBudgetBreakDown {



    [JsonPropertyName("item")]
    public string? Item {get; set;} = string.Empty;

    [JsonPropertyName("quantity")]
    public int? Quantity {get; set;} = 0;
     
     [JsonPropertyName("cost")]
     public int? Cost {get; set;} = 0;
     
     [JsonPropertyName("total")]
     public int? Total {get; set;} = 0; 
}