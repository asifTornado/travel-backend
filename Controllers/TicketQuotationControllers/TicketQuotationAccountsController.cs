
using Microsoft.AspNetCore.Mvc;
using backEnd.Models;
using backEnd.Mappings;
using System.Text.Json;
using MongoDB.Bson;
using System.Runtime.CompilerServices;
using MongoDB.Driver.Core.Authentication;
using Org.BouncyCastle.Ocsp;
using System.IO;
using MongoDB.Driver.Core.Operations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Text;
using backEnd.Services.IServices;
using backEnd.Helpers.IHelpers;

using Microsoft.AspNetCore.Authorization;








using MailKit;
using AutoMapper;
using backEnd.Helpers;
using System.Security.AccessControl;
using backEnd.Services;
using backEnd.Helpers;
using Org.BouncyCastle.Asn1.X509;
using MimeKit.Encodings;
using Microsoft.VisualBasic;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using backEnd.Models.DTOs;
using System.Reflection;
using System.Linq.Expressions;
using ZstdSharp.Unsafe;
using Microsoft.EntityFrameworkCore.Update;
using Org.BouncyCastle.Asn1.IsisMtt.Ocsp;
using System.Diagnostics.CodeAnalysis;
using Org.BouncyCastle.Bcpg;
using backEnd.Helpers.Mails;

namespace backEnd.Controllers.TicketQuotationControllers;




[Route("/")]
[ApiController]

public class TicketQuotationAccountsController : ControllerBase
{


     private IBudgetsService _budgetService;
     private IMapper _imapper;
     private INotifier _notifier;
     private ILogService _logService;
     private IUsersService _userService;
     private RoleService _roleService;
     private IIDCheckService _idCheckService;
     private MailerWorkFlow _mailerWorkFlow;



    public TicketQuotationAccountsController(RoleService roleService, MailerWorkFlow mailerWorkFlow, IUsersService  usersService, 
     IBudgetsService budgetsService, IMapper mapper, 
     INotifier notifier, ILogService logService,
     IIDCheckService idCheckService
     )
    {
        _budgetService = budgetsService;
        _imapper = mapper;
        _notifier = notifier;
        _logService = logService;
        _userService = usersService;
        _roleService = roleService;
        _idCheckService = idCheckService;
        _mailerWorkFlow = mailerWorkFlow;
        
    }
    


   
  [HttpPost]
  [Route("ticketQuotationForward")]
  public async Task<IActionResult> MoneyReceiptSupervisorApprove(IFormCollection data){

    
    
    var tripId = data["id"];
    var trip = await _budgetService.GetAsync(int.Parse(tripId));
    
    var allowed = _idCheckService.CheckCurrent(trip.CurrentHandlerId, data["token"]);

    if(allowed == false){
      return Ok(false);
    }


    var user = JsonSerializer.Deserialize<User>(data["user"]);
    var next = int.Parse(data["next"]);
    var admin = await _userService.GetAdmin();


  var budgetTicketApprovals = new BudgetTicketApprovals{
    UserId = user.Id,
    BudgetId = trip.Id
  };

  
  await _budgetService.InsertBudgetTicketApprover(budgetTicketApprovals);

   
    trip.TicketApprovals.Add(user);
    trip.PrevHandlerIds.Add(user.Id);
    trip.CurrentHandlerId = next;
    trip.Rejected = false;

    await _budgetService.UpdateAsync(trip.Id, trip);

    var message = $"{user.EmpName} has forwarded ticket quotations for the Trip numbered {trip.TripId}";

    await _notifier.InsertNotification(message, user.Id, next, admin.Id, Events.TicketQuotationsForwarded, "ticketQuotations");
    await _logService.InsertLogs(trip.Requests.Select(x => x.Id).ToList(), user.Id, next, Events.TicketQuotationsForwarded);
    
    var newData = new {
      Id = trip.Id,
      TicketApprovals = trip.TicketApprovals
    };

    var recipient = await _userService.GetOneUser(next);

    _mailerWorkFlow.WorkFlowMail(recipient.MailAddress, message, trip.Id, "ticketQuotations");
   
    return Ok(newData);

  } 



  [HttpPost]
  [Route("ticketQuotationBackward")]
  public async Task<IActionResult> TicketQuotationBackward(IFormCollection data){

      

    
  var tripId = data["id"];
  var trip = await _budgetService.GetAsync(int.Parse(tripId));

  var allowed = _idCheckService.CheckCurrent(trip.CurrentHandlerId, data["token"]);

    if(allowed == false){
      return Ok(false);
    }
  
  var user = JsonSerializer.Deserialize<User>(data["user"]); 
   
  if(trip.PrevHandlerIds.Count < 2){
    return Ok(true);
  }



  
  trip.CurrentHandlerId = trip.PrevHandlerIds.LastOrDefault(); 

  if(trip.PrevHandlerIds.Count > 0){
     trip.PrevHandlerIds.RemoveAt(trip.PrevHandlerIds.Count -1);
       }
    
  trip.Rejected = true;
    
  await _budgetService.UpdateAsync(trip.Id, trip);


  var message = $"{user.EmpName} has backwarded a trip numbered {trip.Id} ";

  await _notifier.InsertNotification(message, user.Id, trip.CurrentHandlerId, trip.Id, Events.TicketQuotationsRejected, "ticketQuotations");
  await _logService.InsertLogs(trip.Requests.Select(x => x.Id).ToList(), user.Id, trip.CurrentHandlerId, Events.TicketQuotationsRejected);

     var recipient = await _userService.GetOneUser(trip.CurrentHandlerId);

    _mailerWorkFlow.WorkFlowMail(recipient.MailAddress, message, trip.Id, "ticketQuotations");
    
    
    return Ok(trip);

  } 

  
   [HttpPost]
  [Route("ticketQuotationsProcessingComplete")]
  public async Task<IActionResult> TicketQuotationsProcessingComplete(IFormCollection data){
    
     var tripId = data["id"];
    var trip = await _budgetService.GetAsync(int.Parse(tripId));


    var allowed = _idCheckService.CheckCurrent(trip.CurrentHandlerId, data["token"]);

    if(allowed == false){
      return Ok(false);
    }

    
    var user = JsonSerializer.Deserialize<User>(data["user"]); 

    var travelManager = await _roleService.GetTravelManager();
   
   
    trip.TicketsApprovedByAccounts = true;
    trip.Rejected = false;
    trip.TicketApprovals.Add(user);
    trip.PrevHandlerIds.Add(user.Id);
    trip.CurrentHandlerId = travelManager.Id;
    await _budgetService.UpdateAsync(trip.Id, trip);


      var message = $"{user.EmpName} has completed processing the air ticket quotations for the trip numbered {trip.Id} ";

  await _notifier.InsertNotification(message, user.Id, travelManager.Id, trip.Id, Events.TicketQuotationsProcessed, "ticketQuotations");
  await _logService.InsertLogs(trip.Requests.Select(x => x.Id).ToList(), user.Id, travelManager.Id, Events.TicketQuotationsProcessed);
   
    
    


    _mailerWorkFlow.WorkFlowMail(travelManager.MailAddress, message, trip.Id, "ticketQuotations");

    return Ok(travelManager.Id);

  } 


    

    
   

}