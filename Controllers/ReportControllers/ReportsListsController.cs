
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
using Microsoft.EntityFrameworkCore.Storage;


namespace backEnd.Controllers;




[Route("/")]
[ApiController]
public class ReportsListController : ControllerBase
{
 
    private IIDCheckService _idCheckService;
    private ReportService _reportService;

    public ReportsListController(
         ReportService reportService

        )
    {
      _reportService = reportService;

    }



 [HttpPost]
 [Route("/getReports")]
 public async Task<IActionResult> GetReports(IFormCollection data){

    var results = await _reportService.GetReports();
    

    var reports = new List<TripReportDTO>();

    //Iterate and get the total of all the expenses
    foreach(var result in results){
        float budget = 0;
        if(result.TotalTripBudget == null || result.TotalTripBudget == "NaN"){
            budget = 0;
        }else{
            budget = float.Parse(result.TotalTripBudget);
        }
        //Getting the report budget
        var report  = new TripReportDTO();
        report.Budget = budget;
        report.Subject = result.Subject;
        report.Arrival_date = result.ArrivalDate;
        report.Departure_date = result.DepartureDate;
        report.Destination = result.Destination;
        report.Id = result.Id;
        report.TripId = result.TripId;

       
        foreach(var request in result.Requests){
             report.NumberOfTravelers += 1;

            foreach(var expense in request.ExpenseReport.Expenses){
                report.Actual_cost += float.Parse(expense.Amount);
            }
        }

        reports.Add(report);
    }

    //get the budget as a number
     return Ok(reports);


 }



    

}

 

