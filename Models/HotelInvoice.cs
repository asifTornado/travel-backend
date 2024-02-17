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

namespace backEnd.Models;





public class HotelInvoice {


     [JsonPropertyName("_id")]
     [Key]

    public int Id {get; set;}
    

     [JsonPropertyName("type")]
    public string? Type {get; set;}  = string.Empty;

     
     [JsonPropertyName("filename")]
     public string? Filename {get; set;} = string.Empty;


     [JsonPropertyName("filePath")]
     public string? FilePath {get; set;} = string.Empty;


     public virtual List<HotelQuotation> Quotations {get; set;} = new List<HotelQuotation>();

}









