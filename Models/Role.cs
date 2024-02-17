using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace backEnd.Models;



public class Role
{
    [JsonPropertyName("_id")]
    [Key]    
    public int Id { get; set; }


    [JsonPropertyName("value")]
    public string? Value { get; set; } = string.Empty;

    
    [JsonPropertyName("users")]
    public virtual List<User> Users {get; set;}

}









