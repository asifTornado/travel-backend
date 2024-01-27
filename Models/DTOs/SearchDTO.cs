using backEnd.Models;
namespace models.DTOs;


public class SearchDTO {

public string Brand {get; set;}
public string Location {get; set;}
public string Hotel {get; set;}

public List<Room>? Rooms {get; set;}
public string Actual_rate {get; set;}

public string Average_rate {get; set;}


}