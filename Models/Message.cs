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










