
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
public class MoneyReceiptApprovalController : ControllerBase
{



    private IUsersService _usersService;
    private MoneyReceiptService _moneyReceiptService;
    private RoleService _roleService;

    private IRequestService _requestService;
    private ILogService _logService;
    private INotifier _notifier;



    public MoneyReceiptApprovalController(ILogService logService, INotifier notifier, IUsersService usersService, IRequestService requestService, MoneyReceiptService moneyReceiptService, RoleService roleService)
    {
      _moneyReceiptService = moneyReceiptService;
      _roleService = roleService;
      _requestService = requestService;
      _logService = logService;
      _notifier = notifier;
    }

  
 
  [HttpPost]
  [Route("moneyReceiptSupervisorApprove")]
  public async Task<IActionResult> MoneyReceiptSupervisorApprove(IFormCollection data){
    
       var moneyReceiptId = data["id"];
    var moneyReceipt = await _moneyReceiptService.GetMoneyReceipt(int.Parse(moneyReceiptId));
    var user = JsonSerializer.Deserialize<User>(data["user"]); 
    moneyReceipt.Approvals.Add(user);
    moneyReceipt.Status = "Being Processed";
    var accounts = await _roleService.GetAccountsReceiverForMoneyReceipt();
    moneyReceipt.PrevHandlerIds.Add(user.Id);
    moneyReceipt.CurrentHandlerId = accounts.Id;
    moneyReceipt.SupervisorApproved = true;
    moneyReceipt.Rejected = false;
    await _moneyReceiptService.UpdateMoneyReceipt(moneyReceipt);

     var message = $"{user.EmpName} has approved an advance payment form for {moneyReceipt.I}";

  await _notifier.InsertNotification(message, user.Id, accounts.Id, moneyReceipt.Id, Events.AdvancePaymentFormApprovedSupervisor, "moneyReceipt");
  await _logService.InsertLog(moneyReceipt.RequestId, user.Id, accounts.Id, Events.AdvancePaymentFormApprovedSupervisor);

    return Ok(moneyReceipt);

  } 



  [HttpPost]
  [Route("moneyReceiptSupervisorReject")]
  public async Task<IActionResult> MoneyReceiptSupervisorReject(IFormCollection data){
    
       var moneyReceiptId = data["id"];
    var moneyReceipt = await _moneyReceiptService.GetMoneyReceipt(int.Parse(moneyReceiptId));
    var user = JsonSerializer.Deserialize<User>(data["user"]);
    

    var request = await _requestService.GetAsync(moneyReceipt.RequestId);
    
    moneyReceipt.Status = "Seeking Rectification";
    moneyReceipt.Submitted = false;
    moneyReceipt.Rejected = true;

    moneyReceipt.CurrentHandlerId = request.RequesterId;
    await _moneyReceiptService.UpdateMoneyReceipt(moneyReceipt);

     var message = $"{user.EmpName} has rejected your money receipt for the trip numbered {request.BudgetId}";

  await _notifier.InsertNotification(message, user.Id, request.RequesterId, moneyReceipt.Id, Events.AdvancePaymentFormRejected, "moneyReceipt");
  await _logService.InsertLog(moneyReceipt.RequestId, user.Id, request.RequesterId, Events.AdvancePaymentFormRejected);


    return Ok(moneyReceipt);

  } 
  






   
    

}

 

