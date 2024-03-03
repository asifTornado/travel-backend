
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



namespace backEnd.Controllers.ExpenseReportControllers;




[Route("/")]
[ApiController]
public class ExpenseReportApprovalController : ControllerBase
{



    private IUsersService _usersService;
    private IExpenseReportService _expenseReportService;
    private RoleService _roleService;

    private IRequestService _requestService;

    private IReportGenerator _reportGenerator;

    private IFileHandler _fileHandler;

    private IMailer _mailer;

    private INotifier _notifier;
    private ILogService _logService;
    private MailerWorkFlow _mailerWorkFlow;




    public ExpenseReportApprovalController(IMailer mailer, IFileHandler fileHandler, 
    IUsersService usersService, IReportGenerator reportGenerator, 
    IRequestService requestService, IExpenseReportService expenseReportService, 
    RoleService roleService, ILogService logService, MailerWorkFlow mailerWorkFlow,
        INotifier notifier)
    {
      _expenseReportService = expenseReportService;
      _roleService = roleService;
      _requestService = requestService;
      _reportGenerator = reportGenerator;
      _fileHandler = fileHandler;
      _mailer = mailer;
       _logService = logService;
       _notifier = notifier;
       _mailerWorkFlow = mailerWorkFlow;
    }

  
 
  [HttpPost]
  [Route("expenseReportSupervisorApprove")]
  public async Task<IActionResult> ExpenseReportSupervisorApprove(IFormCollection data){
    
    var expenseReportId = data["id"];
    var expenseReport  = await _expenseReportService.GetExpenseReport(int.Parse(expenseReportId));
    var user = JsonSerializer.Deserialize<User>(data["user"]); 
    expenseReport.Approvals = new List<User>();
    expenseReport.Approvals.Add(user);
    expenseReport.Status = "Being Processed";
    expenseReport.PrevHandlerIds.Add(user.Id);

        var accounts = await _roleService.GetAccountsReceiverForExpenseReport();
        var request = await _requestService.GetAsync(expenseReport.RequestId);
        var fileName = $"ExpenseReport_{request?.Id}.pdf";
        var auditor = await _roleService.GetAuditor();

        
     
        var pdf = await _reportGenerator.GenerateExpenseReport(fileName, expenseReport, this.ControllerContext);
        
        await _fileHandler.SaveFile(pdf, fileName);
       
    _mailer.SendExpenseReport(accounts.MailAddress, fileName, request, auditor.MailAddress);
 
    expenseReport.CurrentHandlerId = accounts.Id;
    expenseReport.SupervisorApproved = true;
    expenseReport.Rejected = false;
    await _expenseReportService.UpdateExpenseReport(expenseReport);

     var message = $"{user.EmpName} has approved an expense report for the trip numbered {request.BudgetId} ";

  await _notifier.InsertNotification(message, user.Id, expenseReport.CurrentHandlerId, expenseReport.Id, Events.ExpenseReportApprovedSupervisor, "expenseReport");
  await _logService.InsertLog(expenseReport.RequestId, user.Id, expenseReport.CurrentHandlerId, Events.ExpenseReportTravelManagerSubmitted);

    return Ok(expenseReport);

  } 



  [HttpPost]
  [Route("expenseReportSupervisorReject")]
  public async Task<IActionResult> ExpenseReportSupervisorReject(IFormCollection data){
    
     var expenseReportId = data["id"];
    var expenseReport  = await _expenseReportService.GetExpenseReport(int.Parse(expenseReportId));
    var user = JsonSerializer.Deserialize<User>(data["user"]);
    var manager = await _roleService.GetTravelManager();
    

    var request = await _requestService.GetAsync(expenseReport.RequestId);
    
    expenseReport.Status = "Seeking Rectification";
    
    expenseReport.Rejected = false;
    expenseReport.TravelManagerSubmitted = false;
  
    

    expenseReport.CurrentHandlerId = manager.Id;
    await _expenseReportService.UpdateExpenseReport(expenseReport);


     var message = $"{user.EmpName} has rejected your expense report for the trip numbered {request.BudgetId} ";

  await _notifier.InsertNotification(message, user.Id, expenseReport.CurrentHandlerId, expenseReport.Id, Events.ExpenseReportRejected, "expenseReport");
  await _logService.InsertLog(expenseReport.RequestId, user.Id, expenseReport.CurrentHandlerId, Events.ExpenseReportRejected);

   _mailerWorkFlow.WorkFlowMail(manager.MailAddress, message, expenseReport.Id, "expenseReport");
  
    return Ok(expenseReport);

  } 







   
    

}

 

