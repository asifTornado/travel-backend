using backEnd.Models;

namespace backEnd.Models.DTOs;


public class Return{
    public bool Valid {get; set;} = false;
    public int? UserId {get; set;} = 0;
}