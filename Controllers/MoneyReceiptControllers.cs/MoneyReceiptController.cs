
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
public class MoneyReceiptController : ControllerBase
{



    private IUsersService _usersService;
    private MoneyReceiptService _moneyReceiptService;

    private INotifier _notifier;
    private ILogService _logService;





    public MoneyReceiptController(ILogService logService,  IUsersService usersService, MoneyReceiptService moneyReceiptService, INotifier notifier)
    {
      _moneyReceiptService = moneyReceiptService;
      _notifier = notifier;
      _logService = logService;
    }

  
  [HttpPost]
  [Route("submitMoneyReceipt")]
  public async Task<IActionResult> SubmitMoneyReceipt(IFormCollection data){
    var request = JsonSerializer.Deserialize<Request>(data["request"]);
    var moneyReceipt = JsonSerializer.Deserialize<MoneyReceipt>(data["moneyReceipt"]);
    request.MoneyReceiptSubmitted = true;

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
  [Route("getMoneyReceipt")]
  public async Task<IActionResult> GetMoneyReceipt(IFormCollection data){
    var id = int.Parse(data["id"]);
    var result = await _moneyReceiptService.GetMoneyReceipt(id);
    return Ok(result);

  } 
  






   
    

}

 

