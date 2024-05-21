namespace backEnd.Models;

public class CSVBudget{
       public string? TripId {get; set;}
       public string? Name {get; set;}
       public string? From {get; set;}
       public string? To {get; set;}
       public string? Location {get; set;}
       public string? Brand {get; set;}
       public string? Days {get; set;}
       public float? Air {get; set;}
       public float? Hotel {get; set;}
       public float? Total {get; set;}
       public float? Transport {get; set;}
       public float? Others {get; set;}
       public float? TotalTrip {get; set;}
}