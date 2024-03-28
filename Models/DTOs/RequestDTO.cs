using backEnd.Models;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace backEnd.Models.DTOs;

public class RequestDTO {
   
[JsonPropertyName("_id")]
[Key]
public int Id { get; set; }

[JsonPropertyName("tripId")]
public string TripId { get; set; } = "";
        
[JsonPropertyName("destination")]
public string? Destination { get; set; }  = string.Empty;

[JsonPropertyName("purpose")]
public string? Purpose { get; set; }  = string.Empty;

[JsonPropertyName("number")]
public string? Number {get; set;} = string.Empty;

[JsonPropertyName("status")]
public string? Status {get; set;} = string.Empty; 

[JsonPropertyName("startDate")]
public string? StartDate {get; set;} 

[JsonPropertyName("endDate")]
public string? EndDate {get; set;}

[JsonPropertyName("currentHandlerName")]
public string? CurrentHandlerName {get; set;}

[JsonPropertyName("supervisorApproved")]
public bool?  SupervisorApproved {get; set;} = false;

[JsonPropertyName("departmentHeadApproved")]
public bool? DepartmentHeadApproved {get; set;} = false;

[JsonPropertyName("custom")]
public bool? Custom {get; set;} = false;

}