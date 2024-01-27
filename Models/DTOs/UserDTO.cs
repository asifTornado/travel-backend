using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace backEnd.Models.DTOs;

public class UserDTO {
    [JsonPropertyName("_id")]
    [Key]    
    public int Id { get; set; }

    
    [JsonPropertyName("empName")]
    public string? EmpName { get; set; } 

    
    [JsonPropertyName("empCode")]
    public string? EmpCode { get; set; } 


    [JsonPropertyName("designation")]
    public string? Designation { get; set; } 

    

     [JsonPropertyName("mailAddress")]
    public string? MailAddress { get; set; } 
    


     [JsonPropertyName("unit")]
    public string? Unit { get; set; } 

    
     [JsonPropertyName("section")]
    public string? Section { get; set; } 

    
     [JsonPropertyName("wing")]
    public string? Wing { get; set; } 

    

     [JsonPropertyName("team")]
    public string? Team { get; set; } 


    
     [JsonPropertyName("department")]
    public string? Department { get; set; } 


     [JsonPropertyName("teamType")]
    public string? TeamType { get; set; } 


      [JsonPropertyName("password")]
     public string? Password { get; set; } 


  
     [JsonPropertyName("userType")]
    public string? UserType { get; set; }  



   
    [JsonPropertyName("available")]
    public bool? Available { get; set; } 
    

    [JsonPropertyName("rating")]
    public int? Rating { get; set; }


    [JsonPropertyName("raters")]
    public int? Raters { get; set; }

    [JsonPropertyName("extension")]
    public string? Extension { get; set; } 


    [JsonPropertyName("mobileNo")]
    public string? MobileNo { get; set; } 


     [JsonPropertyName("location")]
    public string? Location { get; set; } 

    
     [JsonPropertyName("numbers")]
    public int? Numbers { get; set; } 



     [JsonPropertyName("superVisor")]
    public User? SuperVisor { get; set; } 

     
    [JsonPropertyName("superVisorId")]
    public int? SuperVisorId => SuperVisor?.Id;




   

 

     [JsonPropertyName("zonalHead")]
    public User? ZonalHead { get; set; }  


    [JsonPropertyName("zonalHeadId")]
    public int? ZonalHeadId => ZonalHead?.Id; 




     [JsonPropertyName("travelHandler")]
    public User? TravelHandler { get; set; } 


    [JsonPropertyName("travelHandlerId")]
    public int? TravelHandlerId => TravelHandler?.Id; 

    

     [JsonPropertyName("passportNo")]
    public string? PassportNo { get; set; } 






    [JsonPropertyName("preferences")]
    public string? Preferences { get; set; } 

     [JsonPropertyName("hasFrequentFlyerNo")]
    public string? HasFrequentFlyerNo {get; set;} 

    




    // [JsonPropertyName("hierarchy")]
    // public Hierarchy? Hierarchy {get; set;}
    


}

    
