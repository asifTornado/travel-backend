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

using Newtonsoft.Json;


public class Meeting{

    [JsonPropertyName("subject")]
    public string? Subject {get; set;}

    [JsonPropertyName("attendees")]
    public List<string>? Attendees {get; set;}

    [JsonPropertyName("agenda")]
    public string? Agenda {get; set;} 
 }