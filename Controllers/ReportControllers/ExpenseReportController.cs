
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


namespace backEnd.Controllers;




[Route("/")]
[ApiController]
public class ExpenseReportController : ControllerBase
{



    private IExpenseReportService _expenseReportService;
    private IRequestService _requestService;
    private IFileHandler _fileHandler;
    private IUsersService _usersService;
    private IMailer _mailer;

    private IReportGenerator _reportGenerator;

    private RoleService _roleService;
    private INotifier _notifier;
    private ILogService _logService;
    private MailerWorkFlow _mailerWorkFlow;

    public ExpenseReportController(
        IReportGenerator reportGenerator, RoleService roleService,  
        IMailer mailer, IExpenseReportService expenseReportService, 
        IRequestService requestService, IFileHandler fileHandler,  
        IUsersService usersService,
        ILogService logService,
        INotifier notifier,
        MailerWorkFlow mailerWorkFlow
        
        )
    {
       _expenseReportService = expenseReportService;
       _requestService = requestService;
       _fileHandler = fileHandler;
       _usersService = usersService;
       _mailer = mailer;
       _reportGenerator = reportGenerator;
       _roleService = roleService;
       _logService = logService;
       _notifier = notifier;
       _mailerWorkFlow = mailerWorkFlow;

    }



    [HttpPost]
    [Route("/sendExpenseReport")]
    public async Task<IActionResult> SendExpenseReport(IFormCollection data){

        var expenseReport = JsonSerializer.Deserialize<ExpenseReport>(data["expenseReport"]);
        var requestId = data["requestId"];
        var request = await _requestService.GetAsync(int.Parse(requestId));
       
      
        var travelManager = await _roleService.GetTravelManager();
        expenseReport.RequestId = request.Id;
        request.ExpenseReportGiven = true;
        expenseReport.Rejected = false;
        expenseReport.CurrentHandlerId = travelManager.Id;
        expenseReport.Status = "Seeking Input From Travel Manager";
        expenseReport.Submitted = true;
        foreach(var expense in expenseReport.Expenses){
            expense.Source = "traveler";
        }
    
        await _requestService.UpdateAsync(request);
        await _expenseReportService.InsertExpenseReport(expenseReport);

 var message = $"{request.Requester.EmpName} has submitted an expense report for the trip numbered {request.BudgetId} ";

  await _notifier.InsertNotification(message, request.RequesterId, expenseReport.CurrentHandlerId, expenseReport.Id, Events.ExpenseReportSubmitted, "expenseReport");
  await _logService.InsertLog(expenseReport.RequestId, request.RequesterId, expenseReport.CurrentHandlerId, Events.ExpenseReportSubmitted);

        return Ok(expenseReport);
    }


    
    [HttpPost]
    [Route("/resendExpenseReport")]
    public async Task<IActionResult> ResendExpenseReport(IFormCollection data){

        var expenseReport = JsonSerializer.Deserialize<ExpenseReport>(data["expenseReport"]);
        var requestId = data["requestId"];
        var request = await _requestService.GetAsync(int.Parse(requestId));
       
      
        var travelManager = await _roleService.GetTravelManager();
      
        request.ExpenseReportGiven = true;
        expenseReport.Rejected = false;
        expenseReport.CurrentHandlerId = travelManager.Id;
        expenseReport.Status = "Seeking Input From Travel Manager";

        foreach(var expense in expenseReport.Expenses){
            expense.Source = "traveler";
        }
        await _requestService.UpdateAsync(request);
        await _expenseReportService.UpdateExpenseReport(expenseReport);


         var message = $"{request.Requester.EmpName} has resubmitted an expense report for the trip numbered {request.BudgetId} ";

  await _notifier.InsertNotification(message, request.RequesterId, expenseReport.CurrentHandlerId, expenseReport.Id, Events.ExpenseReportResubmitted, "expenseReport");
  await _logService.InsertLog(expenseReport.RequestId, request.RequesterId, expenseReport.CurrentHandlerId, Events.ExpenseReportResubmitted);
        return Ok(expenseReport);
    }



      [HttpPost]
    [Route("/travelManagerSubmitExpenseReport")]
    public async Task<IActionResult> TravelManagerSubmitExpenseReport(IFormCollection data){

        var expenseReport = JsonSerializer.Deserialize<ExpenseReport>(data["expenseReport"]);
        var request = await _requestService.GetAsync(expenseReport.RequestId);

        var travelManager = await _roleService.GetTravelManager();

        expenseReport.Rejected = false;
        expenseReport.CurrentHandlerId = request.Requester.SuperVisorId;
        expenseReport.Status = "Seeking Supervisor's Approval";
        expenseReport.TravelManagerSubmitted = true;
        await _requestService.UpdateAsync(request);
        await _expenseReportService.UpdateExpenseReport(expenseReport);

         var message = $"{travelManager.EmpName} has submitted an expense report for the trip numbered {request.BudgetId} ";

  await _notifier.InsertNotification(message, travelManager.Id, expenseReport.CurrentHandlerId, expenseReport.Id, Events.ExpenseReportTravelManagerSubmitted, "expenseReport");
  await _logService.InsertLog(expenseReport.RequestId, travelManager.Id, expenseReport.CurrentHandlerId, Events.ExpenseReportTravelManagerSubmitted);
        return Ok(expenseReport);
    }




        [HttpPost]
    [Route("/travelManagerRejectExpenseReport")]
    public async Task<IActionResult> TravelManagerRejectExpenseReport(IFormCollection data){

        var expenseReportId = data["id"];
        var requestId = data["requestId"];
        var expenseReport = await _expenseReportService.GetExpenseReport(int.Parse(expenseReportId));
        var request = await _requestService.GetAsync(int.Parse(requestId));
        
        var travelManager = await _roleService.GetTravelManager();
      
      
        
        request.ExpenseReportGiven = false;
        expenseReport.Rejected = true;
        expenseReport.CurrentHandlerId = request.RequesterId;
        expenseReport.Status = "Seeking Expense Report Rectification";
        expenseReport.TravelManagerSubmitted = false;
        await _requestService.UpdateAsync(request);
        await _expenseReportService.UpdateExpenseReport(expenseReport);


         var message = $"{travelManager.EmpName} has rejected your expense report for the trip numbered {request.BudgetId} ";

  await _notifier.InsertNotification(message, travelManager.Id, expenseReport.CurrentHandlerId, expenseReport.Id, Events.ExpenseReportRejected, "expenseReport");
  await _logService.InsertLog(expenseReport.RequestId, travelManager.Id, expenseReport.CurrentHandlerId, Events.ExpenseReportRejected);
  
        return Ok(expenseReport);
    }


    [HttpPost]
    [Route("/getExpenseReport")]
    public async Task<IActionResult> GetExpenseReport(IFormCollection data){
        var id = int.Parse(data["id"]);
        var result = await _expenseReportService.GetExpenseReportFromRequest(id);
        return Ok(result);
    }

    

}

 

