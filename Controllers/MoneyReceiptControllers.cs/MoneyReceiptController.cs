
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



    public MoneyReceiptController(IUsersService usersService, MoneyReceiptService moneyReceiptService)
    {
      _moneyReceiptService = moneyReceiptService;
    }

  
  [HttpPost]
  [Route("submitMoneyReceipt")]
  public async Task<IActionResult> SubmitMoneyReceipt(IFormCollection data){
    var request = JsonSerializer.Deserialize<Request>(data["request"]);
    var moneyReceipt = JsonSerializer.Deserialize<MoneyReceipt>(data["moneyReceipt"]);
    request.MoneyReceiptSubmitted = true;

    moneyReceipt.RequestId = request.Id;
    moneyReceipt.CurrentHandlerId = request.Requester.SuperVisorId;
    moneyReceipt.Approvals = new List<User>();
    moneyReceipt.Approvals.Add(request.Requester);
    moneyReceipt.Submitted = true;

    await _moneyReceiptService.SubmitMoneyReceipt(moneyReceipt, request);

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

 

