
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
public class ExpenseReportListsController : ControllerBase
{



    private IUsersService _usersService;
    private IExpenseReportService _expenseReportService;
    private IIDCheckService _idCheckService;
    



    public ExpenseReportListsController(IUsersService usersService, IExpenseReportService expenseReportService, IIDCheckService idCheckService)
    {
      _expenseReportService = expenseReportService;
      _idCheckService = idCheckService;
    }

  
  [HttpPost]
  [Route("getExpenseReportsForMe")]
  public async Task<IActionResult> GetExpenseReportsForMe(IFormCollection data){
    var user = JsonSerializer.Deserialize<User>(data["user"]);
    var result = await _expenseReportService.GetExpenseReportsForMe(user);
  
    return Ok(result);




  } 


    
  [HttpPost]
  [Route("getExpenseReportsApprovedByMe")]
  public async Task<IActionResult> GetExpenseReportsApprovedByMe(IFormCollection data){
    var user = JsonSerializer.Deserialize<User>(data["user"]);
    var result = await _expenseReportService.GetExpenseReportsApprovedByMe(user);
  
    return Ok(result);

  } 

    
  [HttpPost]
  [Route("getMyExpenseReports")]
  public async Task<IActionResult> GetMyExpenseReports(IFormCollection data){
    var user = JsonSerializer.Deserialize<User>(data["user"]);
    var result = await _expenseReportService.GetMyExpenseReports(user);
  
    return Ok(result);

  } 



      
  [HttpPost]
  [Route("getAllExpenseReports")]
  public async Task<IActionResult> GetAllExpenseReports(IFormCollection data){

    var allowed = await _idCheckService.CheckAdminOrManager(data["token"]);

    if(allowed == false){
      return Ok(false);
    }

    
    var result = await _expenseReportService.GetAllExpenseReports();
    return Ok(result);

  } 


  




   
    

}

 

