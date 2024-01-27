using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace backEnd.Models;



[NotMapped]
public class Room{

    [JsonPropertyName("type")]
    public string? Type {get; set;} = string.Empty;

     [JsonPropertyName("average_rate")]
    public string? Average_rate {get; set;}  = "hello world";

    [JsonPropertyName("actual_rate")]
    public string? Actual_rate {get; set;} 

}


public class Hotels
{

    [JsonPropertyName("_id")]
    [Key] 
    public int Id { get; set; }


    
    [JsonPropertyName("hotelName")]
    public string? HotelName {get; set;} 

    [JsonPropertyName("hotelAddress")]
    public string? HotelAddress {get; set;}


     [JsonPropertyName("hotelLocationsId")]
     public int? HotelLocationsId {get; set;}

    [JsonPropertyName("hotelLocations")]
    public virtual HotelLocations? HotelLocation {get; set;} 


    [JsonPropertyName("rooms")]
    public List<Room>? Rooms {get; set;} = new List<Room>();
 


}





public class HotelLocations
{
    
    [JsonPropertyName("_id")]
    [Key]
    public int Id { get; set; }
    
    [JsonPropertyName("locationName")]
    public string? LocationName {get; set;} = string.Empty;

    

   [JsonPropertyName("hotels")]
    public virtual List<Hotels>? Hotels {get; set;}  = null;


    [JsonPropertyName("hotelForBrandsId")]
    public int? HotelForBrandsId {get; set;}

    
   
    [JsonPropertyName("hotelForBrands")]
    public virtual HotelForBrands? HotelForBrands {get; set;} = null;




}









public class HotelForBrands
{
   [JsonPropertyName("_id")]
    [Key]
    public int Id { get; set; }

    
    [JsonPropertyName("brand")]
    public string? Brand {get; set;} = string.Empty;

    [JsonPropertyName("brandOfficeAddress")]
    public string? BrandOfficeAddress {get; set;}= string.Empty;

    
    [JsonPropertyName("locations")]
    public virtual List<HotelLocations>? Locations {get; set;}  = null;



 


}









