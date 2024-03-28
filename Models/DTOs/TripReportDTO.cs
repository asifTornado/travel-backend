
using backEnd.Models;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations.Schema;

namespace backEnd.Models.DTOs;


public class TripReportDTO{

    [JsonPropertyName("_id")]
    public int Id {get; set;}

    public string TripId {get; set;} = string.Empty;
    public string Subject {get; set;} = string.Empty;
    public string Destination {get; set;} = string.Empty;
    public string Departure_date {get; set;} = string.Empty;
    public string Arrival_date {get; set;} = string.Empty;

    public int NumberOfTravelers {get; set;} = 0;

    public int Budget {get; set;} = 0;
    public int Actual_cost {get; set;} = 0;
   
}


