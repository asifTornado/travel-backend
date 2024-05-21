
using AutoMapper;
using backEnd.Helpers.IHelpers;
using backEnd.Models;
using backEnd.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using backEnd.Mappings;
using System.Diagnostics;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;
using Amazon.Util.Internal;
using backEnd.Helpers;
using backEnd.Services;
using backEnd.Helpers.Mails;
using Org.BouncyCastle.Security;
using backEnd.Models.DTOs;
using System.Text;


namespace backEnd.Controllers;




[Route("/")]
[ApiController]
public class CSVController : ControllerBase
{
  private ReportService _reportService;

    public CSVController(
        ReportService reportService

        )
    {
      _reportService = reportService;

    }

 [HttpGet]
 [Route("/getCSV")]
 public async Task<IActionResult> GetCSV(){
    var budgets = await _reportService.GetReportsForDownload();
    var builder = new StringBuilder();
    builder.AppendLine("Sl,Traveler's Name,From Date,To Date,Places Visited,Budgeted Air Ticket,Budgeted Hotel,Budgeted Total Booking Cost,Budgeted Transport Expense,Budgeted Total Daily Allowance,Budgeted Emerygency Expense,Budgeted Others Cost,Budgeted Total Trip Cost,Supervisor Approved Air Tickets,Supervisor Approved Hotel,Supervisor Approved Total Booking Cost,Supervisor Approved Transport Expense,Supervisor Approved Total Daily Allowance,Supervisor approved Emergency Expense,Supervisor Approved Others Expense,Supervisor Approved Total Trip Cost,Advance Taken,Actual Paid Air Tickets Cost,Actual Paid Hotel Cost,Actual Paid Total Booking Cost,Actual Paid Transport Expense,Actual Paid Total Daily Allowance,Actual Paid Others Expense,Actual paid Total Trip cost,Air Tickets (Budget - Supervisor Approved),Hotel Cost (Budget - Supervisor Approved),Total Booking Cost (Budget - Supervisor Approved),Transport Expense (Budget - Supervisor Approved),Total Daily Allowance (Budget - Supervisor Approved),Total Trip Cost (Budget - Supervisor Approved),Air Tickets (Budget - Actual Paid),Hotel (Budget - Actual Paid),Total Booking Cost (Budget - Actual Paid),Transport Expense (Budget - Actual Paid),Total Daily Allowance (Budget - Actual Paid),Total Trip Cost (Budget - Actual Paid),Air Tickets (Supervisor Approved - Actual Paid),Hotel (Supervisor Approved - Actual Paid),Total Booking Cost (Supervisor Approved - Actual Paid),Transport Expense (Supervisor Approved - Actual Paid),Total Daily Allowance (Supervisor Approved - Actual Paid),Total Trip Cost (Supervisor Approved - Actual Paid),Other Expenses (Budget - Supervisor Approved),Other Expenses (Budget - Actual Paid),Others Expense (Supervisor Approved - Actual Paid),Department,Days count,Total expense per day,Hotel Expense per Day");


    foreach(var budget in budgets){
        foreach(var request in budget.Requests){
            var budgetCSV = GetBudget(budget, request);
            var supervisorCSV = GetSupervisor(request);
            var actualCSV = GetActual(budget, request); 
            var calculated = GetCalculated(budgetCSV, supervisorCSV, actualCSV);
            var extras = GetExtras(request, actualCSV);
            var csvString = GetCSVString(request, budgetCSV, supervisorCSV, actualCSV, calculated, extras);
            builder.AppendLine(csvString);
            
        }
    }

   return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", "report.csv");

 }


 private CSVSupervisor GetSupervisor(Request request){
      var c = new CSVSupervisor();
       c.Air = 0;
       c.Hotel = 0;
       c.Transport = 0;
       c.Daily = 0;
       c.Others = 0;
       c.Emergency = 0 ;

       foreach (var item in request.RequestBudget.Breakdown)
       {
        switch(item.ExpenseType){
            case "air-ticket":
              c.Air += float.Parse(item.Total);
            break;
            case "hotel":
              c.Hotel += float.Parse(item.Total);
            break;
            case "others":
               c.Others += float.Parse(item.Total);
            break;
            case "daily expense":
               c.Daily += float.Parse(item.Total);
            break;
            case "transport":
               c.Others += float.Parse(item.Total);
            break;
        }
       }

       return c;
 }


 private CSVBudget GetBudget(Budget budget, Request request){
       var c = new CSVBudget();
       c.TripId = budget.TripId;
       c.Name = request.Requester.EmpName;
       c.From = request.StartDate;
       c.To =  request.EndDate;
       c.Location = request.Destination;
       c.Brand = budget.Brand;
       c.Days = request.NumberOfNights;
       c.Air =   float.Parse(budget.AirTicketBudget ?? "0");
       c.Hotel = float.Parse(budget.HotelBudget ?? "0");
       c.Total = float.Parse(budget.TotalBookingCost ?? "0");
       c.Transport = float.Parse(budget.TransportExpense ?? "0");
       c.Others = float.Parse(budget.IncidentalExpense ?? "0");
       c.TotalTrip = float.Parse(budget.TotalTripBudget ?? "0");

       return c;

 }


private CSVActual GetActual(Budget budget, Request request){
     var c = new CSVActual();
   //   c.Advance = request.MoneyReceipt.AmountDisbursed;
     c.Air = 0;
     c.Hotel = 0;
     c.Transport = 0;
     c.Daily = 0;
     c.Others = 0;
     
     string Department = request.Requester.Department;
     var days = request.NumberOfNights;
     var totalPerDay = 0;
     var hotelPerDay = 0;

     foreach(var item in request.ExpenseReport.Expenses){
        switch(item.ExpenseType){
             case "air-ticket":
              c.Air += float.Parse(item.Amount);
            break;
            case "hotel":
              c.Hotel += float.Parse(item.Amount);
            break;
            case "others":
               c.Others += float.Parse(item.Amount);
            break;
            case "daily expense":
               c.Daily += float.Parse(item.Amount);
            break;
            case "transport":
               c.Transport += float.Parse(item.Amount);
            break;
        }
     }


 return c;

     
}

private CSVCalculated GetCalculated(CSVBudget b, CSVSupervisor s, CSVActual a){
    var c = new CSVCalculated();
    c.AirBS = b.Air - s.Air;
    c.HotelBS = b.Hotel - s.Hotel;
    c.TotalBS = b.Total - (s.Air + s.Hotel + s.Daily + s.Transport + s.Others);
    c.TransportBS = b.Transport - s.Transport;
    c.OthersBS = b.Others - s.Others;
    c.TotalBS = b.Total + (s.Air + s.Hotel + s.Daily + s.Transport + s.Others);

    c.AirBA = b.Air - a.Air;
    c.HotelBA = b.Hotel - a.Hotel;
    c.TotalBA = b.Total - (a.Air + a.Hotel + a.Daily + a.Transport + a.Others);
    c.TransportBA = b.Transport - a.Transport;
    c.OthersBA = b.Others - a.Others;
    c.TotalBA = b.Total +  (a.Air + a.Hotel + a.Daily + a.Transport + a.Others);

    c.AirSA = s.Air - a.Air;
    c.HotelSA = s.Hotel - a.Hotel;
    c.TotalSA = (s.Air + s.Hotel + s.Daily + s.Transport + s.Others) - (a.Air + a.Hotel + a.Daily + a.Transport + a.Others);
    c.TransportSA = s.Transport - a.Transport;
    c.OthersSA = s.Others - a.Others;
    c.TotalSA = (s.Air + s.Hotel + s.Daily + s.Transport + s.Others) +  (a.Air + a.Hotel + a.Daily + a.Transport + a.Others);
    
    return c;
    
}




private CSVExtras GetExtras(Request request, CSVActual a){
       var c = new CSVExtras();
       c.Days = request.NumberOfNights;
       c.Department = request.Requester.Department;
       c.ExpensePerDay =  (a.Daily + a.Transport + a.Air + a.Hotel + a.Others) / (float.Parse(c.Days));
       c.HotelExpensePerDay = a.Hotel / (float.Parse(c.Days));

       return c;
}


private string GetCSVString(Request r, CSVBudget b, CSVSupervisor s, CSVActual a, CSVCalculated c, CSVExtras e){
    var budgetString = $"{b.TripId},{r.StartDate},{r.EndDate},{b.Location},{b.Brand},{b.Air},{b.Hotel},{b.Total},{b.Transport},{b.Others},{b.TotalTrip}";
    var superString = $"{s.Air},{s.Hotel},{s.Air + s.Hotel},{s.Transport},{s.Others},{s.Emergency},{s.Air + s.Hotel + s.Transport + s.Others}";
    var actualString = $"{a.Advance},{a.Air},{a.Hotel},{a.Air + a.Hotel},{a.Transport},{a.Daily},{a.Others},{a.Air + a.Hotel + a.Transport + a.Daily + a.Others}";
    var calculatedStringBS = $"{c.AirBS},{c.HotelBS},{c.AirBS + c.HotelBS},{c.TransportBS},{c.DailyBS},{c.OthersBS},{c.AirBS+c.HotelBS + c.TransportBS + c.OthersBS + c.DailyBS}";
    var calculatedStringBA = $"{c.AirBA},{c.HotelBA},{c.AirBA + c.HotelBA},{c.TransportBA},{c.DailyBA},{c.OthersBA},{c.AirBA+c.HotelBA + c.TransportBA + c.OthersBA + c.DailyBA}";
    var calculatedStringSA = $"{c.AirSA},{c.HotelSA},{c.AirSA + c.HotelSA},{c.TransportSA},{c.DailySA},{c.OthersSA},{c.AirSA+c.HotelSA + c.TransportSA + c.OthersSA + c.DailySA}";
    var extraSting = $"{e.Department},{e.Days},{e.HotelExpensePerDay},{e.ExpensePerDay}";

    var fullString = budgetString + superString + actualString + calculatedStringBS + calculatedStringBA + calculatedStringSA + extraSting;
    return fullString;
}

}


 

