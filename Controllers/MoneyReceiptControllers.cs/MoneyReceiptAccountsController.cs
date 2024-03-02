
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


namespace backEnd.Controllers.MoneyReceiptControllers;




[Route("/")]
[ApiController]
public class MoneyReceiptAccountsController : ControllerBase
{



    private IUsersService _usersService;
    private MoneyReceiptService _moneyReceiptService;
    private RoleService _roleService;

    private IRequestService _requestService;
    private INotifier _notifier;
    private ILogService _logService;

    private IIDCheckService _idCheckService;



    public MoneyReceiptAccountsController(
      INotifier notifier, ILogService logService,
      IUsersService usersService, IRequestService requestService, 
      MoneyReceiptService moneyReceiptService, RoleService roleService,
      IDCheckService idCheckService
      )
    {
      _moneyReceiptService = moneyReceiptService;
      _roleService = roleService;
      _requestService = requestService;
      _logService = logService;
      _notifier = notifier;
      _idCheckService = idCheckService;
      
    }

  
 
  [HttpPost]
  [Route("moneyReceiptForward")]
  public async Task<IActionResult> MoneyReceiptSupervisorApprove(IFormCollection data){
    
    var moneyReceiptId = data["id"];
    var moneyReceipt = await _moneyReceiptService.GetMoneyReceipt(int.Parse(moneyReceiptId));

    var allowed =  _idCheckService.CheckCurrent(moneyReceipt.CurrentHandlerId, data["token"]);

    if(allowed == false){
      return Ok(false);
    }

    var user = JsonSerializer.Deserialize<User>(data["user"]);
    var next = int.Parse(data["next"]);

    moneyReceipt.Approvals.Add(user);

    moneyReceipt.Status = "Being Processed";

    moneyReceipt.PrevHandlerIds.Add(user.Id);
    moneyReceipt.CurrentHandlerId = next;
    moneyReceipt.Rejected = false;

    await _moneyReceiptService.UpdateMoneyReceipt(moneyReceipt);

     var message = $"{user.EmpName} has forwarded an advance payment form from {moneyReceipt.I} to you";

  await _notifier.InsertNotification(message, user.Id, next, moneyReceipt.Id, Events.AdvancePaymentFormForward, "moneyReceipt");
  await _logService.InsertLog(moneyReceipt.RequestId, user.Id, next, Events.AdvancePaymentFormForward);


    return Ok(moneyReceipt);

  } 



  [HttpPost]
  [Route("moneyReceiptBackWard")]
  public async Task<IActionResult> MoneyReceiptBackward(IFormCollection data){
    
     var moneyReceiptId = data["id"];
    var moneyReceipt = await _moneyReceiptService.GetMoneyReceipt(int.Parse(moneyReceiptId));


      var allowed =  _idCheckService.CheckCurrent(moneyReceipt.CurrentHandlerId, data["token"]);

    if(allowed == false){
      return Ok(false);
    }


    var user = JsonSerializer.Deserialize<User>(data["user"]); 
   
    if(moneyReceipt.PrevHandlerIds.Count < 2){
      moneyReceipt.Status = "Seeking Supervisor's Approval";
      moneyReceipt.SupervisorApproved = false;
    }else{
    moneyReceipt.Status = "Being Processed";

    }

  
    moneyReceipt.CurrentHandlerId = moneyReceipt.PrevHandlerIds.LastOrDefault();
    if(moneyReceipt.PrevHandlerIds.Count > 0){
       moneyReceipt.PrevHandlerIds.RemoveAt(moneyReceipt.PrevHandlerIds.Count -1);

    }
    moneyReceipt.Rejected = true;
    
    await _moneyReceiptService.UpdateMoneyReceipt(moneyReceipt);

         var message = $"{user.EmpName} has rejected an advance payment form from {moneyReceipt.I} ";

  await _notifier.InsertNotification(message, user.Id, moneyReceipt.CurrentHandlerId, moneyReceipt.Id, Events.AdvancePaymentFormRejected, "moneyReceipt");
  await _logService.InsertLog(moneyReceipt.RequestId, user.Id, moneyReceipt.CurrentHandlerId, Events.AdvancePaymentFormForward);

    return Ok(moneyReceipt);

  } 

  
   [HttpPost]
  [Route("moneyReceiptProcessingComplete")]
  public async Task<IActionResult> MoneyReceiptProcessingComplete(IFormCollection data){
    
     var moneyReceiptId = data["id"];
    var moneyReceipt = await _moneyReceiptService.GetMoneyReceipt(int.Parse(moneyReceiptId));
    var user = JsonSerializer.Deserialize<User>(data["user"]); 

    var travelManager = await _roleService.GetTravelManager();
   
    moneyReceipt.Status = "Processing Complete";
    moneyReceipt.Processed = true;
    moneyReceipt.Rejected = false;
    moneyReceipt.Approvals.Add(user);
    moneyReceipt.PrevHandlerIds.Add(user.Id);
    await _moneyReceiptService.UpdateMoneyReceipt(moneyReceipt);


      var message = $"{user.EmpName} has completed processing the advance payment form for {moneyReceipt.I} ";

  await _notifier.InsertNotification(message, user.Id, travelManager.Id, moneyReceipt.Id, Events.AdvancePaymentFormProcessed, "moneyReceipt");
  await _logService.InsertLog(moneyReceipt.RequestId, user.Id, travelManager.Id, Events.AdvancePaymentFormProcessed);


    return Ok(moneyReceipt);

  } 




   [HttpPost]
  [Route("moneyReceiptMoneyDisburse")]
  public async Task<IActionResult> Disburse(IFormCollection data){
    var moneyReceipt = JsonSerializer.Deserialize<MoneyReceipt>(data["moneyReceipt"]);
    
    moneyReceipt.Disbursed = true;
   
    
    await _moneyReceiptService.UpdateMoneyReceipt(moneyReceipt);
    

    return Ok(moneyReceipt);

  } 
  







   
    

}

 

