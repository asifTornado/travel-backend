using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backEnd.Models;





public class ExpenseReport {


   [JsonPropertyName("_id")]
   [Key]
   public int Id {get; set;}

   [JsonPropertyName("employeeName")]
   public string? EmployeeName {get; set;}

   [JsonPropertyName("employeeId")]
   public int? EmployeeId {get; set;}

  [JsonPropertyName("department")]
  public string? Department {get; set;}
  
  [JsonPropertyName("startDate")]
  public string? StartDate {get; set;}

  [JsonPropertyName("endDate")]
  public string? EndDate {get; set;}
  
  [JsonPropertyName("expenses")]
  public virtual List<Expenses>? Expenses {get; set;}

  [JsonPropertyName("requestId")]
  public int? RequestId {get; set;} 

  [JsonPropertyName("request")]
  public virtual Request? Request {get; set;}

  [JsonPropertyName("status")]
  public string? Status {get; set;} 

  [JsonPropertyName("processed")]
  public bool? Processed {get; set;} = false; 

  [JsonPropertyName("supervisorApproved")]
  public bool? SupervisorApproved {get; set;} = false;


    [JsonPropertyName("submitted")]
    public bool? Submitted {get; set;} = false;

    [JsonPropertyName("approvals")]
    public List<User>? Approvals {get; set;} = new List<User>();

    [JsonPropertyName("currentHandlerId")]
    public int? CurrentHandlerId {get; set;} 

    [JsonPropertyName("prevHandlerIds")]
    public List<int>? PrevHandlerIds {get; set;} = new List<int>();

     [NotMapped]
    [JsonPropertyName("currentHandler")]
    public virtual User? CurrentHandler {get; set;} 

    [JsonPropertyName("rejected")]
    public bool? Rejected {get; set;} = true;

    [JsonPropertyName("travelManagerSubmitted")]
    public bool?  TravelManagerSubmitted {get; set;} = false;

    [JsonPropertyName("disbursed")]
    public bool? Disbursed {get; set;} = false;

    [JsonPropertyName("amountDisbursed")]
    public string? AmountDisbursed {get; set;} 


    [JsonPropertyName("bankAccountHolderName")]
    public string? BankAccountHolderName {get; set;}

    [JsonPropertyName("bankAccountNumber")]
    public string? BankAccountNumber {get; set;}

}


