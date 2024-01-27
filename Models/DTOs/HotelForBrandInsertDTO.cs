using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace backEnd.Models.DTOs;





public class HotelsDTO
{

    


    
    [JsonPropertyName("hotelName")]
    public string? HotelName {get; set;} 

    
    [JsonPropertyName("average_rate")]
    public string? Average_rate {get; set;} 



    [JsonPropertyName("actual_rate")]
    public string? Actual_rate {get; set;} 


    //  [JsonPropertyName("hotelLocationsId")]
    //      public int? HotelLocationsId {get; set;}

    [JsonPropertyName("hotelLocations")]
    public HotelLocationsDTO? HotelLocation {get; set;} 
 


}





public class HotelLocationsDTO
{
   
    
    [JsonPropertyName("locationName")]
    public string? LocationName {get; set;} = string.Empty;

    

   [JsonPropertyName("hotels")]
    public List<Hotels>? Hotels {get; set;}  = null;

    
   
    [JsonPropertyName("hotelForBrands")]
    public List<HotelForBrandsDTO>? HotelForBrands {get; set;} = null;




}









public class HotelForBrandsDTO
{
 

    
    [JsonPropertyName("brand")]
    public string? Brand {get; set;} = string.Empty;

    
    [JsonPropertyName("locations")]
    public List<HotelLocations>? Locations {get; set;}  = null;
 


}




