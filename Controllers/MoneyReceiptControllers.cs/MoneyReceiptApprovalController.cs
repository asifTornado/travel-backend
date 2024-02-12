
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
public class MoneyReceiptApprovalController : ControllerBase
{



    private IUsersService _usersService;
    private MoneyReceiptService _moneyReceiptService;
    private RoleService _roleService;

    private IRequestService _requestService;



    public MoneyReceiptApprovalController(IUsersService usersService, IRequestService requestService, MoneyReceiptService moneyReceiptService, RoleService roleService)
    {
      _moneyReceiptService = moneyReceiptService;
      _roleService = roleService;
      _requestService = requestService;
    }

  
 
  [HttpPost]
  [Route("moneyReceiptSupervisorApprove")]
  public async Task<IActionResult> MoneyReceiptSupervisorApprove(IFormCollection data){
    
    var moneyReceipt = JsonSerializer.Deserialize<MoneyReceipt>(data["moneyReceipt"]);
    var user = JsonSerializer.Deserialize<User>(data["user"]); 
    moneyReceipt.Approvals.Add(user);
    moneyReceipt.Status = "Being Processed";
    var accounts = await _roleService.GetAccountsReceiverForMoneyReceipt();
    moneyReceipt.CurrentHandlerId = accounts.Id;
    moneyReceipt.SupervisorApproved = true;
    await _moneyReceiptService.UpdateMoneyReceipt(moneyReceipt);

    return Ok(moneyReceipt);

  } 



  [HttpPost]
  [Route("moneyReceiptSupervisorReject")]
  public async Task<IActionResult> MoneyReceiptSupervisorReject(IFormCollection data){
    
    var moneyReceipt = JsonSerializer.Deserialize<MoneyReceipt>(data["moneyReceipt"]);
    

    var request = await _requestService.GetAsync(moneyReceipt.RequestId);
    
    moneyReceipt.Status = "Seeking Rectification";
    moneyReceipt.Submitted = false;

    moneyReceipt.CurrentHandlerId = request.RequesterId;
    await _moneyReceiptService.UpdateMoneyReceipt(moneyReceipt);

    return Ok(true);

  } 
  






   
    

}

 

