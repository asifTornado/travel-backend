
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


namespace backEnd.Controllers;




[Route("/")]
[ApiController]
public class ReportController: ControllerBase
{
 
    private IIDCheckService _idCheckService;
    private ReportService _reportService;

    public ReportController(
         ReportService reportService

        )
    {
      _reportService = reportService;

    }



 [HttpPost]
 [Route("/getReport")]
 public async Task<IActionResult> GetReport(IFormCollection data){

    var id = int.Parse(data["id"]);

    var result = await _reportService.GetReport(id);

    var requestReportsBudget = new List<RequestReport>();
    var requestReportsActual = new List<RequestReport>();


    foreach(var request in result.Requests){
      var requestReportBudget = new RequestReport();
      var requestReportActual = new RequestReport();


      foreach(var breakdown in request.RequestBudget.Breakdown){
        if(breakdown.ExpenseType == "air-ticket"){
          requestReportBudget.AirTicket += float.Parse(breakdown.Total);
        }else if(breakdown.ExpenseType == "hotel"){
          requestReportBudget.Hotel += float.Parse(breakdown.Total);
        }else if(breakdown.ExpenseType == "transport"){
          requestReportBudget.Transport += float.Parse(breakdown.Total);
        }else if(breakdown.ExpenseType == "incidental"){
          requestReportBudget.Incidental += float.Parse(breakdown.Total);
        }
        else if(breakdown.ExpenseType == "miscellaneuous"){
          requestReportBudget.Miscellaneuous += float.Parse(breakdown.Total);
        }
      }


      foreach(var breakdown in request.ExpenseReport.Expenses){
        if(breakdown.ExpenseType == "air-ticket"){
          requestReportActual.AirTicket += float.Parse(breakdown.Amount);
        }else if(breakdown.ExpenseType == "hotel"){
          requestReportActual.Hotel += float.Parse(breakdown.Amount);
        }else if(breakdown.ExpenseType == "transport"){
          requestReportActual.Transport += float.Parse(breakdown.Amount);
        }else if(breakdown.ExpenseType == "incidental"){
          requestReportActual.Incidental += float.Parse(breakdown.Amount);
        }
        else if(breakdown.ExpenseType == "miscellaneuous"){
          requestReportActual.Miscellaneuous += float.Parse(breakdown.Amount);
        }
      }
      
      requestReportsBudget.Add(requestReportBudget);
      requestReportsActual.Add(requestReportActual);

    }

    result.RequestReportsBudgeted = requestReportsBudget;
    result.RequestReportsActual = requestReportsActual;

    //get the budget as a number
     return Ok(result);


 }



    

}

 

