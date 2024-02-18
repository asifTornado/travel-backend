
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


namespace backEnd.Controllers.ExpenseReportControllers;




[Route("/")]
[ApiController]
public class ExpenseReportController : ControllerBase
{



    private IUsersService _usersService;
    private IExpenseReportService _expenseReportService;



    public ExpenseReportController(IUsersService usersService, IExpenseReportService expenseReportService)
    {
      _expenseReportService = expenseReportService;
    }

  
 
  

   [HttpPost]
  [Route("getExpenseReportSolo")]
  public async Task<IActionResult> GetExpenseReport(IFormCollection data){
    var id = int.Parse(data["id"]);
    var result = await _expenseReportService.GetExpenseReport(id);
    return Ok(result);

  }

  [HttpPost]
  [Route("disburse")]
  public async Task<IActionResult> Disburse(IFormCollection data){
    var expenseReport = JsonSerializer.Deserialize<ExpenseReport>(data["expenseReport"]);
    var amount = data["amount"];
    expenseReport.ExpenseDisbursed = true;
    expenseReport.AmountDisbursed = amount;
    
    await _expenseReportService.UpdateExpenseReport(expenseReport);

    return Ok(expenseReport);

  } 
  } 
  






   
    



 

