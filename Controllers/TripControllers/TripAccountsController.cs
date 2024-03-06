
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
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using backEnd.Helpers.Mails;





namespace backEnd.Controllers.TripControllers;




[Route("/")]
[ApiController]
public class TripAccountsController : ControllerBase
{



    private IUsersService _usersService;
    private IExpenseReportService _expenseReportService;
    private RoleService _roleService;

    private IRequestService _requestService;

    private ILogService _logService;
    private INotifier _notifier;
    private IIDCheckService _idCheckService;
    private MailerWorkFlow _mailerWorkFlow;
    private UsersService _userService;
    private IJwtTokenConverter _jwtTokenConver;
    private IBudgetsService _budgetService;



    public TripAccountsController(IUsersService usersService, 
    IRequestService requestService, ILogService logService,
    INotifier notifier, MailerWorkFlow mailerWorkFlow,
    IExpenseReportService expenseReportService, RoleService roleService, 
    IIDCheckService idCheckService,
    UsersService userService,
    IJwtTokenConverter jwtTokenConverter
    )
    {
      _expenseReportService = expenseReportService;
      _roleService = roleService;
      _requestService = requestService;
      _logService = logService;
      _notifier = notifier;
      _idCheckService = idCheckService;
      _mailerWorkFlow = mailerWorkFlow;
      _userService = userService;
      _jwtTokenConver = jwtTokenConverter;
    }

  
 
  [HttpPost]
  [Route("accountsTripForward")]
  public async Task<IActionResult> AccountsTripForward(IFormCollection data){
     
   
    
    var tripId = data["tripId"];
    var trip  = await _budgetService.GetAsync(int.Parse(tripId));
    var nextId = int.Parse(data["nextId"]);
    var user = JsonSerializer.Deserialize<User>(data["user"]);


  
    var allowed = _idCheckService.CheckCurrent(trip.CurrentAccountsHandlerId, data["token"]);

    if(allowed == false){
      return Ok(false);
    }




    trip.AccountsPrevHandlerIds.Add(user.Id); 
    trip.CurrentAccountsHandlerId = nextId;

    await _budgetService.UpdateAsync(trip.Id, trip);

     var message = $"{user.EmpName} has forwarded to you a trip with the number {trip.Id} ";

  await _notifier.InsertNotification(message, user.Id, trip.CurrentAccountsHandlerId, trip.Id, Events.TripForwardAccounts, "trip");
  await _logService.InsertLog(trip.Id, user.Id, trip.CurrentAccountsHandlerId, Events.TripForwardAccounts);
  

  
     var recipient = await _userService.GetOneUser(nextId);

     var emailToken = _jwtTokenConver.GenerateToken(recipient);

    _mailerWorkFlow.WorkFlowMail(recipient.MailAddress, message, trip.Id, "trip", emailToken);


    return Ok(true);

  } 



  [HttpPost]
  [Route("accountsTripBackWard")]
  public async Task<IActionResult> AccountsTripBackward(IFormCollection data){
    
    var tripId = data["tripId"];
    var trip  = await _budgetService.GetAsync(int.Parse(tripId));

    var allowed = _idCheckService.CheckCurrent(trip.CurrentAccountsHandlerId, data["token"]);

    if(allowed == false){
      return Ok(false);
    }
    
    var user = JsonSerializer.Deserialize<User>(data["user"]); 

    if(trip.AccountsPrevHandlerIds.Count < 2){

      return Ok(false);
    
    }
   
  trip.CurrentAccountsHandlerId = trip.AccountsPrevHandlerIds.LastOrDefault();
    
    if(trip.AccountsPrevHandlerIds.Count > 0){
       trip.AccountsPrevHandlerIds.RemoveAt(trip.AccountsPrevHandlerIds.Count -1);

    }
    
 
    
    await _budgetService.UpdateAsync(trip.Id, trip);

     var message = $"{user.EmpName} has sent back to you a trip numbered {trip.Id} ";

  await _notifier.InsertNotification(message, user.Id, trip.CurrentAccountsHandlerId, trip.Id, Events.TripBackwardAccounts, "trip");
  await _logService.InsertLog(trip.Id, user.Id, trip.CurrentAccountsHandlerId, Events.TripBackwardAccounts);

    var recipient = await _userService.GetOneUser(trip.CurrentAccountsHandlerId);

     var emailToken = _jwtTokenConver.GenerateToken(recipient);

    _mailerWorkFlow.WorkFlowMail(recipient.MailAddress, message, trip.Id, "trip", emailToken);


    return Ok(true);

  } 

  
   [HttpPost]
  [Route("accountsTripComplete")]
  public async Task<IActionResult> AccountsTripComplete(IFormCollection data){
    
    var tripId = data["tripId"];
    var trip  = await _budgetService.GetAsync(int.Parse(tripId));


    var allowed = _idCheckService.CheckCurrent(trip.CurrentAccountsHandlerId, data["token"]);

    if(allowed == false){
      return Ok(false);
    }
    

    var user = JsonSerializer.Deserialize<User>(data["user"]); 
   
    
    trip.AccountsProcessed = true;
    trip.AccountsProcessed = false;
  
    trip.AccountsPrevHandlerIds.Add(user.Id);
    
    
    await _budgetService.UpdateAsync(trip.Id, trip);

    var manager = await _roleService.GetTravelManager();


      var message = $"{user.EmpName} has completed processing the trip numbered {trip.Id} from the accounts department";

  await _notifier.InsertNotification(message, user.Id, manager.Id, trip.Id, Events.TripCompleteAccounts, "trip");
  await _logService.InsertLog(trip.Id, user.Id, manager.Id, Events.TripCompleteAccounts);
  

     var emailToken = _jwtTokenConver.GenerateToken(manager);

    _mailerWorkFlow.WorkFlowMail(manager.MailAddress, message, manager.Id, "trip", emailToken);

    return Ok(true);

  } 


  








   
    

}

 

