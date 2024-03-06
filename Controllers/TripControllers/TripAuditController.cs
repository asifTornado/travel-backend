
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
public class TripAuditController : ControllerBase
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



    public TripAuditController(IUsersService usersService, 
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
  [Route("auditTripForward")]
  public async Task<IActionResult> AuditTripForward(IFormCollection data){
     
   
    
    var tripId = data["tripId"];
    var trip  = await _budgetService.GetAsync(int.Parse(tripId));
    var nextId = int.Parse(data["nextId"]);
    var user = JsonSerializer.Deserialize<User>(data["user"]);


  
    var allowed = _idCheckService.CheckCurrent(trip.CurrentAuditHandlerId, data["token"]);

    if(allowed == false){
      return Ok(false);
    }




    trip.AuditPrevHandlerIds.Add(user.Id); 
    trip.CurrentAuditHandlerId = nextId;

    await _budgetService.UpdateAsync(trip.Id, trip);

     var message = $"{user.EmpName} has forwarded to you a trip with the number {trip.Id} ";

  await _notifier.InsertNotification(message, user.Id, trip.CurrentAuditHandlerId, trip.Id, Events.TripForwardAudit, "trip");
  await _logService.InsertLog(trip.Id, user.Id, trip.CurrentAuditHandlerId, Events.TripForwardAudit);
  

  
     var recipient = await _userService.GetOneUser(nextId);

     var emailToken = _jwtTokenConver.GenerateToken(recipient);

    _mailerWorkFlow.WorkFlowMail(recipient.MailAddress, message, trip.Id, "trip", emailToken);


    return Ok(true);

  } 



  [HttpPost]
  [Route("auditTripBackWard")]
  public async Task<IActionResult> AuditTripBackward(IFormCollection data){
    
    var tripId = data["tripId"];
    var trip  = await _budgetService.GetAsync(int.Parse(tripId));

    var allowed = _idCheckService.CheckCurrent(trip.CurrentAuditHandlerId, data["token"]);

    if(allowed == false){
      return Ok(false);
    }
    
    var user = JsonSerializer.Deserialize<User>(data["user"]); 

    if(trip.AuditPrevHandlerIds.Count < 2){

      return Ok(false);
    
    }
   
  trip.CurrentAuditHandlerId = trip.AuditPrevHandlerIds.LastOrDefault();
    
    if(trip.AuditPrevHandlerIds.Count > 0){
       trip.AuditPrevHandlerIds.RemoveAt(trip.AuditPrevHandlerIds.Count -1);

    }
    
 
    
    await _budgetService.UpdateAsync(trip.Id, trip);

     var message = $"{user.EmpName} has sent back to you a trip numbered {trip.Id} ";

  await _notifier.InsertNotification(message, user.Id, trip.CurrentAuditHandlerId, trip.Id, Events.TripBackwardAudit, "trip");
  await _logService.InsertLog(trip.Id, user.Id, trip.CurrentAuditHandlerId, Events.TripBackwardAudit);

    var recipient = await _userService.GetOneUser(trip.CurrentAuditHandlerId);

     var emailToken = _jwtTokenConver.GenerateToken(recipient);

    _mailerWorkFlow.WorkFlowMail(recipient.MailAddress, message, trip.Id, "trip", emailToken);


    return Ok(true);

  } 

  
   [HttpPost]
  [Route("auditTripComplete")]
  public async Task<IActionResult> AuditTripComplete(IFormCollection data){
    
    var tripId = data["tripId"];
    var trip  = await _budgetService.GetAsync(int.Parse(tripId));


    var allowed = _idCheckService.CheckCurrent(trip.CurrentAuditHandlerId, data["token"]);

    if(allowed == false){
      return Ok(false);
    }
    

    var user = JsonSerializer.Deserialize<User>(data["user"]); 
   
    
    trip.AuditProcessed = true;
    trip.BeingProcessedAudit = false;
  
    trip.AuditPrevHandlerIds.Add(user.Id);
    
    
    await _budgetService.UpdateAsync(trip.Id, trip);

    var manager = await _roleService.GetTravelManager();


      var message = $"{user.EmpName} has completed processing the trip numbered {trip.Id} from the accounts department";

  await _notifier.InsertNotification(message, user.Id, manager.Id, trip.Id, Events.TripCompleteAudit, "trip");
  await _logService.InsertLog(trip.Id, user.Id, manager.Id, Events.TripCompleteAudit);
  

     var emailToken = _jwtTokenConver.GenerateToken(manager);

    _mailerWorkFlow.WorkFlowMail(manager.MailAddress, message, manager.Id, "trip", emailToken);

    return Ok(true);

  } 


  








   
    

}

 

