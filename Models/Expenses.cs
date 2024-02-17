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


public class Expenses{
    [JsonPropertyName("_id")]
    [Key]
    public int Id {get; set;}

    [JsonPropertyName("date")]
    public string? Date {get; set;}

    [JsonPropertyName("expenseType")]
    public string? ExpenseType {get; set;}

    [JsonPropertyName("description")]
    public string? Description {get; set;}

    [JsonPropertyName("amount")]
    public string? Amount {get; set;}

    [JsonPropertyName("notes")]
    public string? Notes {get; set;}

    [JsonPropertyName("expenseReportId")]
    public int? ExpenseReportId {get; set;}

    [JsonPropertyName("expenseReport")]
    public virtual ExpenseReport? ExpenseReport {get; set;}

    [JsonPropertyName("invoice")]
    public string? Voucher {get; set;}

    [JsonPropertyName("voucherGiven")]
    public bool? VoucherGiven {get; set;} = false;

    [JsonPropertyName("source")]
    public string? Source {get; set;} = "traveler";
    
}