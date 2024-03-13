using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace backEnd.Models;




public class FlyerNos{

    [Key]
     [JsonPropertyName("_id")]
     public int? Id { get; set; }

      
      [JsonPropertyName("airline")]
      public string? Airline {get; set;} = string.Empty;


    [JsonPropertyName("number")]
      public string? Number {get; set;} = string.Empty;
      
     

      [NotMapped]
      [JsonPropertyName("user")] 
      public virtual User? User {get; set;} = null;


      [JsonPropertyName("userId")]
      public int? UserId {get; set;}
}





public class User
{
    [JsonPropertyName("_id")]
    [Key]    
    public int Id { get; set; }


    [JsonPropertyName("empId")]
    public string? EmpId { get; set; } = string.Empty;

    
    [JsonPropertyName("empName")]
    public string? EmpName { get; set; }  = string.Empty;

    
    [JsonPropertyName("empCode")]
    public string? EmpCode { get; set; }  = string.Empty;


    [JsonPropertyName("designation")]
    public string? Designation { get; set; }  = string.Empty;

    

     [JsonPropertyName("mailAddress")]
    public string? MailAddress { get; set; } = string.Empty;
    


     [JsonPropertyName("unit")]
    public string? Unit { get; set; }  = string.Empty; 

    
     [JsonPropertyName("section")]
    public string? Section { get; set; } = string.Empty;

    
     [JsonPropertyName("wing")]
    public string? Wing { get; set; } = string.Empty;

    

     [JsonPropertyName("team")]
    public string? Team { get; set; } = string.Empty;


    
     [JsonPropertyName("department")]
    public string? Department { get; set; } = string.Empty;


     [JsonPropertyName("roles")]
     public List<Role> Roles { get; set; }


      [JsonPropertyName("password")]
     public string? Password { get; set; } = string.Empty;


  
     [JsonPropertyName("userType")]
    public string? UserType { get; set; }  = string.Empty;



   
    [JsonPropertyName("available")]
    public bool? Available { get; set; } = true;
    

    [JsonPropertyName("rating")]
    public int? Rating { get; set; } = 0;

    

    [JsonPropertyName("raters")]
    public int? Raters { get; set; } = 0;


    [JsonPropertyName("extension")]
    public string? Extension { get; set; } = string.Empty;


    [JsonPropertyName("mobileNo")]
    public string? MobileNo { get; set; }   = string.Empty;


     [JsonPropertyName("location")]
    public string? Location { get; set; } = string.Empty;

    
     [JsonPropertyName("numbers")]
    public int? Numbers { get; set; } = 0;



     [JsonPropertyName("superVisorId")]
    public int? SuperVisorId { get; set; }


    [NotMapped]

     [JsonPropertyName("superVisor")]
    public virtual User? SuperVisor { get; set; } 

    

   
    


      [JsonPropertyName("zonalHeadId")]
    public int? ZonalHeadId { get; set; }



    [NotMapped]
    [JsonPropertyName("zonalHead")]
    public virtual User? ZonalHead { get; set; }  




    [JsonPropertyName("travelHandlerId")]
    public int? TravelHandlerId { get; set; }



    [NotMapped]
    [JsonPropertyName("travelHandler")]
    public virtual User? TravelHandler { get; set; } 

    

    [JsonPropertyName("passportNo")]
    public string? PassportNo { get; set; } 


    [NotMapped]
    [JsonPropertyName("flyerNos")]
    public virtual List<FlyerNos>? FlyerNos { get; set; }

    
    [JsonPropertyName("preferenceImage")]
    public string? PreferenceImage {get; set;} = string.Empty;

    [JsonPropertyName("preferences")]
    public string? Preferences { get; set; } = string.Empty;


    [JsonPropertyName("hasFrequentFlyerNo")]
    public string? HasFrequentFlyerNo {get; set;} = string.Empty;
    
  
    
    [JsonPropertyName("budgets")]
    public virtual List<Budget>? Budgets { get; set; }


    [NotMapped]
    [JsonPropertyName("requests")]
    public virtual List<Request>? Requests {get; set;} = new List<Request>();

    [NotMapped]
    [JsonPropertyName("superVised")]
    public virtual List<User>? SuperVised {get; set;} = new List<User>();

    
 
     
   [NotMapped]
   [JsonPropertyName("head")] 
   public virtual List<User>? Head {get; set;} =  new List<User>();

    [NotMapped]
    [JsonPropertyName("travelHandled")]
    public virtual List<User>? TravelHandled {get; set;} = new List<User>();



     [NotMapped]
    [JsonPropertyName("currentHandled")]
    public virtual  List<Request>? CurrentHandled {get; set;} = new List<Request>();

    [NotMapped]
    [JsonPropertyName("ticketApproved")]
    public virtual List<Quotation>? TicketApproved {get; set;} = new List<Quotation>();


    [NotMapped]
    [JsonPropertyName("budgetTicketsApproved")]
    public virtual List<Budget>? BudgetTicketsApproved {get; set;} = new List<Budget>();
    
    [NotMapped]
    [JsonPropertyName("hotelApproved")]
    public virtual List<HotelQuotation>? HotelApproved {get; set;} = new List<HotelQuotation>();


    [JsonPropertyName("currentReceiptsHandled")]
    public virtual List<MoneyReceipt>? CurrentReceiptsHandled {get; set;} = new List<MoneyReceipt>();

        [JsonPropertyName("currentExpenseReportsHandled")]
    public virtual List<ExpenseReport>? CurrentExpenseReportsHandled {get; set;} = new List<ExpenseReport>();


}









