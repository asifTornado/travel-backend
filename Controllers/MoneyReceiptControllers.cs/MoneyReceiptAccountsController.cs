
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
using backEnd.services;


namespace backEnd.Controllers.MoneyReceiptControllers;




[Route("/")]
[ApiController]
public class MoneyReceiptAccountsController : ControllerBase
{



    private IUsersService _usersService;
    private MoneyReceiptService _moneyReceiptService;
    private RoleService _roleService;

    private IRequestService _requestService;



    public MoneyReceiptAccountsController(IUsersService usersService, IRequestService requestService, MoneyReceiptService moneyReceiptService, RoleService roleService)
    {
      _moneyReceiptService = moneyReceiptService;
      _roleService = roleService;
      _requestService = requestService;
    }

  
 
  [HttpPost]
  [Route("moneyReceiptForward")]
  public async Task<IActionResult> MoneyReceiptSupervisorApprove(IFormCollection data){
    
    var moneyReceipt = JsonSerializer.Deserialize<MoneyReceipt>(data["moneyReceipt"]);
    var user = JsonSerializer.Deserialize<User>(data["user"]);
    var next = JsonSerializer.Deserialize<User>(data["next"]);

    moneyReceipt.Approvals.Add(user);

    moneyReceipt.Status = "Being Processed";

    moneyReceipt.PrevHandlerId = user.Id;
    moneyReceipt.CurrentHandlerId = next.Id;

    await _moneyReceiptService.UpdateMoneyReceipt(moneyReceipt);

    return Ok(moneyReceipt);

  } 



  [HttpPost]
  [Route("moneyReceiptBackWard")]
  public async Task<IActionResult> MoneyReceiptBackward(IFormCollection data){
    
    var moneyReceipt = JsonSerializer.Deserialize<MoneyReceipt>(data["moneyReceipt"]);
    var user = JsonSerializer.Deserialize<User>(data["user"]); 
   
    moneyReceipt.Status = "Being Processed";
  
    moneyReceipt.CurrentHandlerId = moneyReceipt.PrevHandlerId;
    moneyReceipt.PrevHandlerId = user.Id;
    
    await _moneyReceiptService.UpdateMoneyReceipt(moneyReceipt);

    return Ok(true);

  } 

  
   [HttpPost]
  [Route("moneyReceiptProcessingComplete")]
  public async Task<IActionResult> MoneyReceiptProcessingComplete(IFormCollection data){
    
    var moneyReceipt = JsonSerializer.Deserialize<MoneyReceipt>(data["moneyReceipt"]);
    var user = JsonSerializer.Deserialize<User>(data["user"]); 
   
    moneyReceipt.Status = "Processing Complete";
    moneyReceipt.Processed = true;
    
    await _moneyReceiptService.UpdateMoneyReceipt(moneyReceipt);

    return Ok(moneyReceipt);

  } 











   
    

}

 

