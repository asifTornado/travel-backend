
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
public class MoneyReceiptListsController : ControllerBase
{



    private IUsersService _usersService;
    private MoneyReceiptService _moneyReceiptService;

    private IIDCheckService _idCheckService;



    public MoneyReceiptListsController(IUsersService usersService, IIDCheckService idCheckService, MoneyReceiptService moneyReceiptService)
    {
      _moneyReceiptService = moneyReceiptService;
      _idCheckService = idCheckService;
    }

  
  [HttpPost]
  [Route("getMoneyReceiptsForMe")]
  public async Task<IActionResult> GetMoneyReceiptsForMe(IFormCollection data){
    var user = JsonSerializer.Deserialize<User>(data["user"]);
    var result = await _moneyReceiptService.GetMoneyReceiptsForMe(user);
  
    return Ok(result);




  } 


    
  [HttpPost]
  [Route("getMoneyReceiptsProcessedByMe")]
  public async Task<IActionResult> GetMoneyReceiptsProcessedByMe(IFormCollection data){
    var user = JsonSerializer.Deserialize<User>(data["user"]);
    var result = await _moneyReceiptService.GetMoneyReceiptsProcessedByMe(user);
  
    return Ok(result);

  } 

    
  [HttpPost]
  [Route("getMyMoneyReceipts")]
  public async Task<IActionResult> GetMyMoneyReceipts(IFormCollection data){
    var user = JsonSerializer.Deserialize<User>(data["user"]);
    var result = await _moneyReceiptService.GetMyMoneyReceipts(user);
  
    return Ok(result);

  } 



      
  [HttpPost]
  [Route("getAllMoneyReceipts")]
  public async Task<IActionResult> GetAllMoneyReceipts(IFormCollection data){

    var allowed = await _idCheckService.CheckAdminOrManager(data["token"]);

    if(allowed == false){
      return Ok(false);
    }
    
    var result = await _moneyReceiptService.GetAllMoneyReceipts();
    return Ok(result);

  } 


  




   
    

}

 

