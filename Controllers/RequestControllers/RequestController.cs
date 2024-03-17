
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

namespace backEnd.Controllers.RequestControllers;




[Route("/")]
[ApiController]
public class RequestController : ControllerBase
{


    private IMapper _imapper;
    private IRequestService _requestService;
    private IAgentsService _agentsService;
    private IMailer _mailer;
    private IUsersService _usersService;
    private IFileHandler _fileHandler;
    private INotifier _notifier;
    private IHelperClass _helperClass;
    private TravelContext _travelContext;
    private IQuotationService _quotationService;
    private IBudgetsService _budgetService;
    private ILogService _logService;
    private ITripService _tripService;
    private IJwtTokenConverter _jwtTokenConverter;

    private IIDCheckService _idCheckService;
    
    private RoleService _roleService;




   


    public RequestController(
        IIDCheckService idCheckService,
        IJwtTokenConverter jwtTokenConverter, ITripService tripService, 
        IBudgetsService budgetsService, ILogService logService, 
        IQuotationService quotationService, TravelContext travelContext, 
        IHelperClass helperClass, IFileHandler fileHandler, 
        IUsersService usersService, IAgentsService agentsService, 
        IMapper mapper, IRequestService requestService, 
        IMailer mailer, INotifier notifier,
        RoleService roleService
        )
    {
        _imapper = mapper;
        _requestService = requestService;
        _agentsService = agentsService;
        _mailer = mailer;
        _usersService = usersService;
        _fileHandler = fileHandler;
        _notifier = notifier;
        _helperClass = helperClass;
        _travelContext = travelContext;
        _quotationService = quotationService;
        _logService = logService;
        _budgetService = budgetsService;
        _tripService = tripService;
        _jwtTokenConverter = jwtTokenConverter;
        _idCheckService = idCheckService;
        _roleService = roleService;
    }


    [HttpPost]
    [Route("submitRequest")]
    public async Task<IActionResult> SubmitRequest(IFormCollection data){

        var request = JsonSerializer.Deserialize<Request>(data["request"]);
        var user = JsonSerializer.Deserialize<User>(data["user"]);
        var token = data["token"];
        var brand = data["brand"];
        request.Requester = user;
        request.RequesterId = user.Id;
        var supervisor = await _usersService.GetOneUser(request.Requester.SuperVisorId);
        request.Requester.SuperVisor = supervisor;

        


        var allowed = _idCheckService.CheckSupervisor(request, token);

        if(allowed == false){
            return Ok(false);
        }
        
   
        
      

        

      
    
      
        request.Status = "Seeking Supervisor Approval For Trip";
        request.CurrentHandlerId = request.Requester.SuperVisorId;
        request.Date = _helperClass.GetCurrentTime();
        request.Custom = true;


        var budget = new Budget();

        budget.Subject = request.Purpose;
        budget.ArrivalDate = request.EndDate;
        budget.DepartureDate = request.StartDate;
        budget.Destination = request.Destination;
        budget.Initiated = "Yes";
        budget.Travelers = new List<User>();
        budget.Brand = brand;
        budget.Travelers.Add(request.Requester);
        budget.Requests = new List<Request>();
        budget.Custom = true;
        

        var BudgetId = await _budgetService.CreateBudgetId(budget);
      
            
        //     var agentEmails = new List<Agent>();
        //  foreach(var agent in agents){
        //     request.Agents.Add(agent);
        //  }

   
      request.BudgetId = BudgetId;
      
      var requestId = await _requestService.CreateAsync(request); 

      var token2 = _jwtTokenConverter.GenerateToken(request.Requester.SuperVisor!);
            
            
            // foreach(var agent in agents){
            //       agentEmails.Add(agent);
                    
            // }

    //  _mailer.SendMail(agentEmails, request.Id, request);
        
      string message = $"A travel request from {request.Requester.EmpName} requires your approval for a new trip";


      await _notifier.InsertNotification(message, request.Requester.Id, request.Requester.SuperVisorId, request.Id, Events.SupervisorApprovalTrip,   "unapproved");
      
      await _logService.InsertLog(requestId, request.Requester.Id, request.CurrentHandlerId, Events.SupervisorApprovalTrip);

      _mailer.SeekSupervisorApprovalTrip(request, token2);


        return Ok(true);


}






[HttpPost]
[Route("getRequest")]
public async Task<IActionResult> GetRequest(IFormCollection data){
    var id = data["id"];
    var result = await _requestService.GetAsync(int.Parse(id));
    
  

    await _notifier.DeleteNotification(int.Parse(id), result.Requester.ZonalHead.Id, Events.ZonalHeadApproval);
  

    return Ok(result);
}

   [HttpPost]
   [Route("supervisorApproveTrip")]
   public async Task<IActionResult> SupervisorApproveTrip(IFormCollection data){
    var approval = data["approval"];
    var request = JsonSerializer.Deserialize<Request>(data["request"]);
     
    if(approval == "approved"){
      request.Status = "Seeking Department Head's Approval";
      request.SupervisorApproved = true;
      request.CurrentHandlerId = request.Requester.ZonalHeadId;
      await _requestService.UpdateAsync(request);
      return Ok(true);

    }else{
        request.Status = "Request Rejected By Supervisor";
        request.CurrentHandlerId = null;
        await _requestService.UpdateAsync(request);
        return Ok(true);
    }
    


   }





    [HttpPost]
    [Route("getRequestForQuotation")]
    public async Task<IActionResult> GetQuote(IFormCollection data){

        var id = data["id"];
        var agentId = data["agentId"];

        var request = await _requestService.GetAsync(int.Parse(id));

        if(request == null){
            Console.WriteLine("entered if 1");
            return Ok(false);
        
        // }else if(request.AgentIds.Contains(agentId)){
            
        //     return Ok(request);

        }else{
            Console.WriteLine("entered if 2");
            return Ok(request);
        }


        }









        // [HttpPost]
        // [Route("getRequestForInvoice")]
        // public async Task<IActionResult> GetRequestForInvoice(IFormCollection data){
        //     var request = await _requestService.GetAsync(int.Parse(data["id"]));
        //     if(request.SeekingInvoices != true){
        //         return Ok(false);
        //     }

        //     var agent = request.Quotations.FirstOrDefault(x=> x.Agent.Id == int.Parse(data["agentId"]));
        //     if(agent.Confirmed != true){
        //         return Ok(false);
        //     }

        //     return Ok(request);
        // }




        [HttpPost]
        [Route("processed")]
        public async Task<IActionResult> Processed(IFormCollection data){
            
            
            var request = JsonSerializer.Deserialize<Request>(data["request"]);

            var allowed = await _idCheckService.CheckAdminOrManager(data["token"]);

            if(allowed == false){
                return Ok(false);
            }

            request.Status = "Processing Complete";

            request.Processed = true;

            await _requestService.UpdateStatus(request);


            // _mailer.SendMailProcessed(request);


             string msg = $"Your travel request with the number {request.Id} has been processed.";

            // await _notifier.InsertNotification(msg, request.Requester.TravelHandler.Id, request.Requester.Id, request.Id, Events.Processed);

            // await _notifier.DeleteNotification(request.Id, request.Requester.TravelHandler.Id, Events.AirTicketInvoiceSent);
            // await _notifier.DeleteNotification(request.Id, request.Requester.TravelHandler.Id, Events.HotelInvoiceSent);

            // await _logService.InsertLog(request.Id, request.Requester.TravelHandler.Id, request.Requester.Id, Events.Processed);




            return Ok(request);

       }




        [HttpPost]
        [Route("emailRequest")]
        public async Task<IActionResult> EmailReqeust(IFormCollection data){
            var user = JsonSerializer.Deserialize<User>(data["user"]);
            var request = JsonSerializer.Deserialize<Request>(data["request"]);
            var recipient = await _roleService.GetAccountsReceiverForMoneyReceipt();
            var whom = data["whom"];
            var type = data["type"];

            
            
            User auditor;

            auditor = whom == "accounts" ? await _usersService.GetAuditor() : null;

            request.CurrentHandlerId = recipient.Id;

            request.Status = whom == "accounts" ? "Being Processed" : request.Status;
            request.BeingProcessed = true;


            await _requestService.UpdateStatus(request);


            var emailToken = _jwtTokenConverter.GenerateToken(recipient);

            _mailer.EmailRequest(request, recipient.MailAddress, auditor, type, this.ControllerContext, emailToken, user);

           await _notifier.DeleteNotification(request.Id, user.Id, Events.AirTicketInvoiceSent);

            return Ok(request);


        }



       [HttpPost]
       [Route("getEmailRequest")]
        async Task<IActionResult> GetEmailRequest(IFormCollection data){
            var requestId = int.Parse(data["requestId"]);
            var userId = int.Parse(data["userId"]);

            var user = await _usersService.GetOneUser(userId);

            if(user == null){
                return Ok(false);
            }


            var request = await _requestService.GetAsync(requestId);
            
            var newobject = new {
                 Request = request,
                 User = user
            };

            return Ok(new JsonResult(newobject));


        }



   [HttpPost("getCustomRequest")]
    public async Task<IActionResult> GetCustomRequest(IFormCollection data){
        var id = int.Parse(data["id"]);
        var result = await _requestService.GetCustomRequest(id);
        return Ok(result);
    }


    














     

        


}




 

