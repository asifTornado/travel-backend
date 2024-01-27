using System.ComponentModel.DataAnnotations;
using backEnd.services;
using backEnd.Services;

namespace backEnd.Models;


public class Trip{
    
    [Key]
    public int Id { get; set; }

    public string? Subject {get; set;} = string.Empty;

    public string? Brand {get; set;} = string.Empty;

    public string? TentativeDate {get; set;} = string.Empty;

    public string? TravelDuration {get; set;} = string.Empty;

    public string? Location {get; set;} = string.Empty;

    public string? NumberOfTravellers {get; set;} = string.Empty;



    public string? AirTicketBudget {get; set;} = string.Empty;

    public string? HotelBudget {get; set;} = string.Empty;

    public string? TotalBookingCost {get; set;} = string.Empty;

    public string? TransportExpense {get; set;} = string.Empty;

    public string? IncentalExpense {get; set;} = string.Empty;

    public string? TotalTripBudget {get; set;} = string.Empty;


    public virtual List<User> Travellers { get; set; } = new List<User>();
    public virtual List<Budget>? Budgets { get; set; } = new List<Budget>();





}