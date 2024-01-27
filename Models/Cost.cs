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


public class Cost{

       
 
    [Key]
     [JsonPropertyName("_id")]
    public int Id { get; set; }

           [JsonPropertyName("item")]
        public string? Item { get; set; }



           [JsonPropertyName("itemCost")]
        public string? ItemCost { get; set; } 



        
           [JsonPropertyName("numberOfItems")]
        public string? NumberOfItems { get; set; } 



             
          [JsonPropertyName("totalItemCost")]
        public int? TotalItemCost { get; set; } 
         
         [JsonPropertyName("requestId")]
         public int? RequestId {get; set;}
        
           [JsonPropertyName("request")]
        public virtual Request? Request {get; set;} 
}
    



