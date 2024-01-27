using backEnd.Models;
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



namespace backEnd.Models.DTOs;

public class QuotationDTO{
   [JsonPropertyName("_id")]
   [Key]
   public int Id { get; set; }
  


   [JsonPropertyName("agent")]
   public Agent? Agent {get; set;} = null;

      [JsonPropertyName("agentId")]
   public int? AgentId => Agent?.Id;

   [JsonPropertyName("quotationText")]
   public string? QuotationText {get; set;}  = string.Empty;

   [JsonPropertyName("selected")]
   public bool? Selected {get; set;} = false;

   [JsonPropertyName("book")]
   public bool? Booked {get; set;} = false;

   [JsonPropertyName("confirmed")]
   public bool? Confirmed {get; set;} = false;

   [JsonPropertyName("hovered")]
   public bool? Hovered {get; set;} = false;

   [JsonPropertyName("custom")]
   public bool? Custom {get; set;} = false;

  [JsonPropertyName("requestId")]
  public int? RequestId {get; set;} = null;

  [JsonPropertyName("request")] 
  public Request? Request {get; set;} = null;
}