using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Diagnostics.HealthChecks;


namespace backEnd.Models;




[BsonIgnoreExtraElements]
public class FlyerNosMongo{

      [JsonPropertyName("airline")]
      [BsonElement("airline")]
      public string? Airline {get; set;} = null;


      [JsonPropertyName("number")]
      [BsonElement("number")]
      public string? Number {get; set;} = null;
}













[BsonIgnoreExtraElements]
public class UserMongo
{

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    [JsonPropertyName("_id")]
    [BsonElement("_id")]
    public string? Id { get; set; }

    [BsonElement("empName")]
    [JsonPropertyName("empName")]
    public string? EmpName { get; set; }


    [BsonElement("empCode")]
    [JsonPropertyName("empCode")]
    public string? EmpCode { get; set; }

    [BsonElement("designation")]
    [JsonPropertyName("designation")]
    public string? Designation { get; set; }

    [BsonElement("mailAddress")]
    [JsonPropertyName("mailAddress")]
    public string? MailAddress { get; set; }

    [BsonElement("unit")]
    [JsonPropertyName("unit")]
    public string? Unit { get; set; }

    [BsonElement("section")]
    [JsonPropertyName("section")]
    public string? Section { get; set; }

    [BsonElement("wing")]
    [JsonPropertyName("wing")]
    public string? Wing { get; set; }

    [BsonElement("team")]
    [JsonPropertyName("team")]
    public string? Team { get; set; }

    [BsonElement("groups")]
    [JsonPropertyName("groups")]
    public List<string>? Groups { get; set; }

    [BsonElement("department")]
    [JsonPropertyName("department")]
    public string? Department { get; set; }

    [BsonElement("TeamType")]
    [JsonPropertyName("TeamType")]
    public string? TeamType { get; set; }

    [BsonElement("password")]
    [JsonPropertyName("password")]
    public string? Password { get; set; }

    
    [BsonElement("rank")]
    [JsonPropertyName("rank")]
    public int Rank { get; set; } = 2;

    [BsonElement("userType")]
    [JsonPropertyName("userType")]
    public string? UserType { get; set; } = "normal";


    
    [BsonElement("travelUserType")]
    [JsonPropertyName("travelUserType")]
    public string? TravelUserType { get; set; } = "normal";

    [BsonElement("available")]
    [JsonPropertyName("available")]
    public bool? Available { get; set; } = true;

    [BsonElement("rating")]
    [JsonPropertyName("rating")]
    public int? Rating { get; set; } = 0;


    [BsonElement("raters")]
    [JsonPropertyName("raters")]
    public int? Raters { get; set; } = 0;


    [BsonElement("extension")]
    [JsonPropertyName("extension")]
    public string? Extension { get; set; } = "Not Available";



    [BsonElement("mobileNo")]
    [JsonPropertyName("mobileNo")]
    public string? MobileNo { get; set; } = "Not Available";


    [BsonElement("location")]
    [JsonPropertyName("location")]
    public string? Location { get; set; } = "Not Available";


    [BsonElement("numbers")]
    [JsonPropertyName("numbers")]
    public int Numbers { get; set; } = 0;



    [BsonElement("superVisor")]
    [JsonPropertyName("superVisor")]
    public RequestUser? SuperVisor { get; set; } = new RequestUser();



    [BsonElement("departmentHead")]
    [JsonPropertyName("departmentHead")]
    public RequestUser? DepartmentHead { get; set; } = new RequestUser();

    [BsonElement("zonalHead")]
    [JsonPropertyName("zonalHead")]
    public RequestUser? ZonalHead { get; set; } = new RequestUser();



      
    [BsonElement("travelHandler")]
    [JsonPropertyName("travelHandler")]
    public RequestUser? TravelHandler { get; set; } = new RequestUser();


    [BsonElement("passportNo")]
    [JsonPropertyName("passportNo")]
    public string? PassportNo { get; set; } = null;



    [BsonElement("flyerNos")]
    [JsonPropertyName("flyerNos")]
    public List<FlyerNosMongo>? FlyerNos { get; set; } = new List<FlyerNosMongo>();



    [BsonElement("preferences")]
    [JsonPropertyName("preferences")]
    public string? Preferences { get; set; } = "";

     [BsonElement("hasFrequentFlyerNo")]
    [JsonPropertyName("hasFrequentFlyerNo")]
    public string? HasFrequentFlyerNo {get; set;} = "No";





}




[BsonIgnoreExtraElements]
public class RequestUser{

    [BsonElement("empName")]
    [JsonPropertyName("empName")]
    public string? EmpName { get; set; } = null;


    [BsonElement("empCode")]
    [JsonPropertyName("empCode")]
    public string? EmpCode { get; set; } = null;

    [BsonElement("designation")]
    [JsonPropertyName("designation")]
    public string? Designation { get; set; } = null;

    [BsonElement("mailAddress")]
    [JsonPropertyName("mailAddress")]
    public string? MailAddress { get; set; } = null;



    [BsonElement("department")]
    [JsonPropertyName("department")]
    public string? Department { get; set; } = null;



    [BsonElement("password")]
    [JsonPropertyName("password")]
    public string? Password { get; set; } = null;



    [BsonElement("superVisor")]
    [JsonPropertyName("superVisor")]
    public RequestUser? SuperVisor { get; set; } = null;



    [BsonElement("departmentHead")]
    [JsonPropertyName("departmentHead")]
    public RequestUser? DepartmentHead { get; set; } = null;



    
    [BsonElement("travelHandler")]
    [JsonPropertyName("travelHandler")]
    public RequestUser? TravelHandler { get; set; } = null;


    [BsonElement("zonalHead")]
    [JsonPropertyName("zonalHead")]
    public RequestUser? ZonalHead { get; set; } = null;



    
    [BsonElement("passportNo")]
    [JsonPropertyName("passportNo")]
    public string? PassportNo { get; set; } = null;



    [BsonElement("flyerNos")]
    [JsonPropertyName("flyerNos")]
    public List<FlyerNos>? FlyerNos { get; set; } = new List<FlyerNos>();

    [BsonElement("hasFrequentFlyerNo")]
    [JsonPropertyName("hasFrequentFlyerNo")]
    public string? HasFrequentFlyerNo {get; set;} = "No";


    
    [BsonElement("preferences")]
    [JsonPropertyName("preferences")]
    public string? Preferences { get; set; } = "";

}


