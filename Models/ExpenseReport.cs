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

   [JsonPropertyName("RequestId")]
   public int? RequestId {get; set;} 

   [JsonPropertyName("request")]
   public virtual Request? Request {get; set;}

}


