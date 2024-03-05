
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
using backEnd.Helpers.Mails;
using backEnd.Helpers;
using backEnd.Services;


namespace backEnd.Controllers.MoneyReceiptControllers;




[Route("/")]
[ApiController]
public class MoneyReceiptController : ControllerBase
{


    private MailerMoneyReceipt _mailerMoneyReceipt;
    private IUsersService _usersService;
    private MoneyReceiptService _moneyReceiptService;
    private IRequestService _requestService;
    private RoleService _roleService;

    private INotifier _notifier;
    private ILogService _logService;
    private IReportGenerator _reportGenerator;
    private IFileHandler _fileHandler;
    private IIDCheckService _idCheckService;
    private MailerWorkFlow _mailerWorkFlow;
   




    public MoneyReceiptController(ILogService logService,  
    IUsersService usersService, 
    MoneyReceiptService moneyReceiptService, 
    INotifier notifier,
    IRequestService requestService,
    RoleService roleService,
    IReportGenerator reportGenerator,
    MailerMoneyReceipt mailerMoneyReceipt,
    IFileHandler fileHandler,
    IIDCheckService idCheckService,
    MailerWorkFlow mailerWorkFlow


    )
    
    {
      _moneyReceiptService = moneyReceiptService;
      _notifier = notifier;
      _logService = logService;
      _requestService = requestService;
      _roleService = roleService;
      _reportGenerator = reportGenerator;
      _mailerMoneyReceipt = mailerMoneyReceipt;
      _fileHandler = fileHandler;
      _idCheckService = idCheckService;
      _mailerWorkFlow = mailerWorkFlow;
    }

  
  [HttpPost]
  [Route("submitMoneyReceipt")]
  public async Task<IActionResult> SubmitMoneyReceipt(IFormCollection data){
    
   

    var request = JsonSerializer.Deserialize<Request>(data["request"]);
    
    var allowed = _idCheckService.CheckTraveler(request, data["token"]);

    if(allowed == false){
      return Ok(false);
    }


    var moneyReceipt = JsonSerializer.Deserialize<MoneyReceipt>(data["moneyReceipt"]);
    request.MoneyReceiptSubmitted = true;
    var accounts = await _roleService.GetAccountsReceiverForMoneyReceipt();
    var audit =await _roleService.GetAuditor();

    var fileName = $"MoneyRequisition_{request?.Id}.pdf";
     
    var moneyReceiptPdf = await _reportGenerator.GenerateAdvancePaymentForm(fileName, moneyReceipt, this.ControllerContext);
    await _fileHandler.SaveFile(moneyReceiptPdf, fileName);


     _mailerMoneyReceipt.SendMoneyReceipt(accounts.MailAddress, fileName, moneyReceipt.Id, request, data["token"], audit.MailAddress);

    moneyReceipt.RequestId = request.Id;
    moneyReceipt.CurrentHandlerId = request.Requester.SuperVisorId;
 
    moneyReceipt.Submitted = true;
    moneyReceipt.Rejected = false;

    await _moneyReceiptService.SubmitMoneyReceipt(moneyReceipt, request);

    var message = $"{request.Requester.EmpName} has submitted his advance payment form for the trip numbered {request.BudgetId}";

  await _notifier.InsertNotification(message, request.RequesterId, request.Requester.SuperVisorId, moneyReceipt.Id, Events.AdvancePaymentFormSubmitted, "moneyReceipt");
  await _logService.InsertLog(moneyReceipt.RequestId, request.RequesterId, request.Requester.SuperVisorId, Events.AdvancePaymentFormSubmitted);
  

    return Ok(true);

  } 



    [HttpPost]
  [Route("moneyReceiptResend")]
  public async Task<IActionResult> MoneyReceiptResend(IFormCollection data){
    var moneyReceipt = JsonSerializer.Deserialize<MoneyReceipt>(data["moneyReceipt"]);
    var request = await _requestService.GetAsync(moneyReceipt.RequestId);

    var allowed = _idCheckService.CheckTraveler(request, data["token"]);

    if(allowed == false){
      return Ok(false);
    }

    request.MoneyReceiptSubmitted = true;

    
    moneyReceipt.CurrentHandlerId = request.Requester.SuperVisorId;
 
    moneyReceipt.Submitted = true;
    moneyReceipt.Rejected = false;

    await _moneyReceiptService.UpdateMoneyReceipt(moneyReceipt);

     var accounts = await _roleService.GetAccountsReceiverForMoneyReceipt();
    var audit =await _roleService.GetAuditor();

    var fileName = $"MoneyRequisition_{request?.Id}.pdf";
     
    var moneyReceiptPdf = await _reportGenerator.GenerateAdvancePaymentForm(fileName, moneyReceipt, this.ControllerContext);
    await _fileHandler.SaveFile(moneyReceiptPdf, fileName);


     _mailerMoneyReceipt.SendMoneyReceiptAgain(accounts.MailAddress, fileName, moneyReceipt.Id, request, data["token"], audit.MailAddress);



    var message = $"{request.Requester.EmpName} has resubmitted his advance payment form for the trip numbered {request.BudgetId}";

  await _notifier.InsertNotification(message, request.RequesterId, request.Requester.SuperVisorId, moneyReceipt.Id, Events.AdvancePaymentFormResubmitted, "moneyReceipt");
  await _logService.InsertLog(moneyReceipt.RequestId, request.RequesterId, request.Requester.SuperVisorId, Events.AdvancePaymentFormResubmitted);


    return Ok(true);

  } 
  

  [HttpPost]
  [Route("getMoneyReceipt")]
  public async Task<IActionResult> GetMoneyReceipt(IFormCollection data){
    var id = int.Parse(data["id"]);
    var result = await _moneyReceiptService.GetMoneyReceipt(id);
    return Ok(result);

  } 









   
    

}

 

