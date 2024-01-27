using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backEnd.Models;


public class Agent
{
    [JsonPropertyName("_id")]     
    [Key]
    public int Id { get; set; }   

    [JsonPropertyName("name")]
    public string? Name { get; set; } 

    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("phone")]
    public string? Phone { get; set; } 

    [JsonPropertyName("Professional")]
    public bool? Professional { get; set; } = true;

    [JsonPropertyName("quotations")]
    public virtual List<Quotation>? Quotations { get; set; }


    [JsonPropertyName("hotelQuotations")]
    public virtual List<HotelQuotation>? HotelQuotations { get; set; }

    [JsonPropertyName("requests")]
    public virtual List<Request>? Requests {get; set;} 
}










