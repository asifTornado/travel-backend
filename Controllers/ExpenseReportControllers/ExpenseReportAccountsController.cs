
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
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using backEnd.Helpers.Mails;





namespace backEnd.Controllers.ExpenseReportControllers;




[Route("/")]
[ApiController]
public class ExpenseReportAccountsController : ControllerBase
{



    private IUsersService _usersService;
    private IExpenseReportService _expenseReportService;
    private RoleService _roleService;

    private IRequestService _requestService;

    private ILogService _logService;
    private INotifier _notifier;
    private IIDCheckService _idCheckService;
    private MailerWorkFlow _mailerWorkFlow;
    private UsersService _userService;



    public ExpenseReportAccountsController(IUsersService usersService, 
    IRequestService requestService, ILogService logService,
    INotifier notifier, MailerWorkFlow mailerWorkFlow,
    IExpenseReportService expenseReportService, RoleService roleService, 
    IIDCheckService idCheckService,
    UsersService userService
    )
    {
      _expenseReportService = expenseReportService;
      _roleService = roleService;
      _requestService = requestService;
      _logService = logService;
      _notifier = notifier;
      _idCheckService = idCheckService;
      _mailerWorkFlow = mailerWorkFlow;
      _userService = userService;
    }

  
 
  [HttpPost]
  [Route("expenseReportForward")]
  public async Task<IActionResult> ExpenseReportForward(IFormCollection data){
     
   
    
     var expenseReportId = data["id"];
    var expenseReport  = await _expenseReportService.GetExpenseReport(int.Parse(expenseReportId));


  
    var allowed = _idCheckService.CheckCurrent(expenseReport.CurrentHandlerId, data["token"]);

    if(allowed == false){
      return Ok(false);
    }

    var user = JsonSerializer.Deserialize<User>(data["user"]);
    var next = int.Parse(data["next"]);

    expenseReport.Approvals.Add(user);

    expenseReport.Status = "Being Processed";
    expenseReport.Rejected = false;

    expenseReport.PrevHandlerIds.Add(user.Id); 
    expenseReport.CurrentHandlerId = next;

    await _expenseReportService.UpdateExpenseReport(expenseReport);

     var message = $"{user.EmpName} has forwarded to you an expense report for the trip numbered {expenseReport.Id} ";

  await _notifier.InsertNotification(message, user.Id, expenseReport.CurrentHandlerId, expenseReport.Id, Events.ExpenseReportForward, "expenseReport");
  await _logService.InsertLog(expenseReport.RequestId, user.Id, expenseReport.CurrentHandlerId, Events.ExpenseReportForward);
  

  
     var recipient = await _userService.GetOneUser(next);

    _mailerWorkFlow.WorkFlowMail(recipient.MailAddress, message, expenseReport.Id, "expenseReport");


    return Ok(expenseReport);

  } 



  [HttpPost]
  [Route("expenseReportBackWard")]
  public async Task<IActionResult> ExpenseReportBackward(IFormCollection data){
    
     var expenseReportId = data["id"];
    var expenseReport  = await _expenseReportService.GetExpenseReport(int.Parse(expenseReportId));

    var allowed = _idCheckService.CheckCurrent(expenseReport.CurrentHandlerId, data["token"]);

    if(allowed == false){
      return Ok(false);
    }
    
    var user = JsonSerializer.Deserialize<User>(data["user"]); 

    if(expenseReport.PrevHandlerIds.Count < 2){

    expenseReport.Status = "Seeking Supervisor's Approval";
    expenseReport.SupervisorApproved = false;
    }else{
      expenseReport.Status = "Being Processed";
    }
   
  expenseReport.CurrentHandlerId = expenseReport.PrevHandlerIds.LastOrDefault();
    
    if(expenseReport.PrevHandlerIds.Count > 0){
       expenseReport.PrevHandlerIds.RemoveAt(expenseReport.PrevHandlerIds.Count -1);

    }
    
    expenseReport.Rejected = true;
    
    await _expenseReportService.UpdateExpenseReport(expenseReport);

     var message = $"{user.EmpName} has rejected to you an expense report for the trip numbered {expenseReport.Id} ";

  await _notifier.InsertNotification(message, user.Id, expenseReport.CurrentHandlerId, expenseReport.Id, Events.ExpenseReportForward, "expenseReport");
  await _logService.InsertLog(expenseReport.RequestId, user.Id, expenseReport.CurrentHandlerId, Events.ExpenseReportForward);

    var recipient = await _userService.GetOneUser(expenseReport.CurrentHandlerId);

    _mailerWorkFlow.WorkFlowMail(recipient.MailAddress, message, expenseReport.Id, "expenseReport");


    return Ok(expenseReport);

  } 

  
   [HttpPost]
  [Route("expenseReportProcessingComplete")]
  public async Task<IActionResult> ExpenseReportProcessingComplete(IFormCollection data){
    
    var expenseReportId = data["id"];
    var expenseReport  = await _expenseReportService.GetExpenseReport(int.Parse(expenseReportId));


    var allowed = _idCheckService.CheckCurrent(expenseReport.CurrentHandlerId, data["token"]);

    if(allowed == false){
      return Ok(false);
    }
    

    var user = JsonSerializer.Deserialize<User>(data["user"]); 
   
    expenseReport.Status = "Processing Complete";
    expenseReport.Processed = true;
    expenseReport.Rejected = false;
    expenseReport.PrevHandlerIds.Add(user.Id);
    expenseReport.Approvals.Add(user);
    
    await _expenseReportService.UpdateExpenseReport(expenseReport);

    var manager = await _roleService.GetTravelManager();


      var message = $"{user.EmpName} has completed processing the expense report for the trip numbered {expenseReport.Id} ";

  await _notifier.InsertNotification(message, user.Id, manager.Id, expenseReport.Id, Events.ExpenseReportProcessed, "expenseReport");
  await _logService.InsertLog(expenseReport.RequestId, user.Id, manager.Id, Events.ExpenseReportProcessed);
  

 

    _mailerWorkFlow.WorkFlowMail(manager.MailAddress, message, manager.Id, "expenseReport");

    return Ok(expenseReport);

  } 


   [HttpPost]
  [Route("expenseReportMoneyDisburse")]
  public async Task<IActionResult> Disburse(IFormCollection data){
    var expenseReport = JsonSerializer.Deserialize<ExpenseReport>(data["expenseReport"]);
    var user = await _userService.GetOneUser(expenseReport.CurrentHandlerId);

    
  
    var allowed = _idCheckService.CheckCurrent(expenseReport.CurrentHandlerId, data["token"]);

    if(allowed == false){
      return Ok(false);
    }
    
  
    expenseReport.Disbursed = true;
    
    
    await _expenseReportService.UpdateExpenseReport(expenseReport);

     var manager = await _roleService.GetTravelManager();

var message = $"{user.EmpName} has disbursed money for the expense report for the trip numbered {expenseReport.Id} ";

  await _notifier.InsertNotification(message, user.Id, manager.Id, expenseReport.Id, Events.ExpenseReportMoneyDisbursed, "expenseReport");
  await _logService.InsertLog(expenseReport.RequestId, user.Id, manager.Id, Events.ExpenseReportProcessed);
  

 

    _mailerWorkFlow.WorkFlowMail(manager.MailAddress, message, manager.Id, "expenseReport");
    

    return Ok(expenseReport);

  } 











   
    

}

 

