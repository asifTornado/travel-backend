
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

namespace backEnd.Controllers;




[Route("/")]
[ApiController]

public class RequestQuoteController : ControllerBase
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


    private TripService _tripService;
    private IJwtTokenConverter _jwtTokenConverter;




   


    public RequestQuoteController(IJwtTokenConverter jwtTokenConverter, TripService tripService, IBudgetsService budgetsService, ILogService logService, IQuotationService quotationService, TravelContext travelContext, IHelperClass helperClass, IFileHandler fileHandler, IUsersService usersService, IAgentsService agentsService, IMapper mapper, IRequestService requestService, IMailer mailer, INotifier notifier)
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
    }


   














    [HttpPost]
    [Route("supervisorApprove")]
    public async Task<IActionResult> SupervisorApprove(IFormCollection data){


        var requestFront = JsonSerializer.Deserialize<Request>(data["request"]);
        var what = data["what"];
        var approval = data["approval"];
        var message = data["message"];
        var messageObject = new Message();
        var relatedRequests = await _tripService.GetRelatedRequests(requestFront);
        var user = JsonSerializer.Deserialize<User>(data["user"]);
    

        var requests = new List<Request>();
       
        requests.AddRange(relatedRequests);
    
        string notmessage;
        string newEvent;
    
      foreach(var request in requests){

 if(approval == "approved"){

    if(what == "ticket"){

        request.Status = "Seeking Confirmation";
        messageObject.Status = "Your ticket quotation was approved";
        messageObject.Content = message;
        // request.TicketApprovals.Add(request.Requester.SuperVisor);

        notmessage = $"{request.Requester.SuperVisor.EmpName} has approved your {what} ticket quotation for the request id {request.Id}";
        newEvent = Events.SupervisorApprovalTicket;

    

        // _mailer.SendMailBook(quotation.Agent, request, quotation);
    

        }else if(what == "hotel"){
           request.Status = "Seeking Hotel Confirmation";
           messageObject.Status = "Your hotel quotations was approved";
           messageObject.Content = message;
          

           notmessage = $"{request.Requester.SuperVisor.EmpName} has approved your {what} hotel quotation for the request id {request.Id}";
           newEvent = Events.SupervisorApprovalHotel;
       
        }else{
            notmessage = string.Empty;
            newEvent = string.Empty;
        }

  }else if(approval == "rejected"){
    if(what == "ticket"){

        request.Booked = false;
        request.Status = "Seeking Quotations";
    
         messageObject.Status = "Your ticket quotation was rejected";
        messageObject.Content = message;

        notmessage = $"{request.Requester.SuperVisor.EmpName} has rejected your {what} ticket quotation for the request id {request.Id}";
        newEvent = Events.SupervisorRejectedTicket;

    }else if(what == "hotel"){
 
        request.HotelBooked = false;
        request.Status = "Seeking Quotations For Hotel";
     
         messageObject.Status = "Your hotel quotation was rejected";
        messageObject.Content = message;

         notmessage = $"{request.Requester.SuperVisor.EmpName} has rejected your {what} hotel quotation for the request id {request.Id}";
         newEvent = Events.SupervisorRejectHotel;
    }else{
        notmessage = string.Empty;
        newEvent = string.Empty;
    }
  }else {
    notmessage = string.Empty;
    newEvent = string.Empty;
  }
       
        
        

        request.Messages.Add(messageObject);
        



        
        request.CurrentHandlerId = request.Requester.TravelHandler.Id;
        
        


        await _requestService.UpdateAsync(request);

        
        string prevEvent;

        if(what == "ticket"){
            prevEvent = Events.SupervisorApprovalTicket;
            }else if(what == "hotel"){
                prevEvent = Events.SupervisorApprovalHotel;
            }else{
                prevEvent = string.Empty;
            }


        await _notifier.InsertNotification(notmessage, request.Requester.SuperVisor.Id, request.Requester.Id, request.Id, newEvent);
        await _notifier.DeleteNotification(request.Id, request.Requester.SuperVisor.Id, prevEvent);
        await _logService.InsertLog(request.Id, request.Requester.SuperVisor.Id, request.Requester.TravelHandler.Id,  newEvent);
      }


      if(what == "ticket"){

        var quotation = requestFront.Quotations.FirstOrDefault(x => x.Booked == true);

        var quotations = await _tripService.GetRelatedTicketQuotations(quotation);

        foreach(var quotation3 in quotations){
            quotation3.Booked = approval == "approved";
            quotation3.Approved = approval == "approved";
        
            quotation3.TicketApprovals.Add(user);
         
        }

        await _quotationService.UpdateTicketQuotations(quotations);
        
      }else{


        var quotation = requestFront.HotelQuotations.FirstOrDefault(x => x.Booked == true);

        var quotations = await _tripService.GetRelatedHotelQuotations(quotation);

        foreach(var quotation3 in quotations){
            quotation3.Booked = approval == "approved";
            quotation3.Approved = approval == "approved";
            quotation3.HotelApprovals.Add(user);
         
        }

        await _quotationService.UpdateHotelQuotations(quotations);
      
     
}

        return Ok(true);

}












    [HttpPost]
    [Route("addCustomQuote")]
    public async Task<IActionResult> AddCustomQuote(IFormCollection data){

        var user = JsonSerializer.Deserialize<User>(data["user"]);
        var what = data["what"];
        var quoteGiver = data["quoteGiver"];
        var quotationString = data["quotation"];
      
        var agent = new Agent();
        var id = data["id"];
        agent.Email = user.MailAddress;
    
        agent.Phone = user.MobileNo;
        agent.Name = quoteGiver;
        agent.Professional = false;
       
       if(what == "hotel"){

        var quotation = new HotelQuotation();

        quotation.Custom = true;
        // quotation.Agent = agent;
        quotation.QuotationText = quotationString;

        var request = await _requestService.GetAsync(int.Parse(id));

        if(request.HotelQuotations == null){
                request.HotelQuotations = new List<HotelQuotation>();
        }
        request.HotelQuotations.Add(quotation);


        await _requestService.UpdateAsync(request);




        var message = $"{agent.Name} has submitted quote for your travel request number {request.Id}";



        // _notifier.InsertNotification(message, agent.Name, request.Requester.EmpName, request.Id);


        // _mailer.SendMailQuoteReceived(request, agent);

        return Ok(quotation);


       }else if(what == "ticket"){

         var quotation = new Quotation();
        quotation.Custom = true;
        
        // quotation.Agent = agent;
        quotation.QuotationText = quotationString;
     


        var request = await _requestService.GetAsync(int.Parse(id));
        // quotation.Requests = new List<Request>();
     
        if(request.Quotations == null){
              request.Quotations = new List<Quotation>();
        }

           request.Quotations.Add(quotation);
          request.AgentNumbers--;

        
  
        // request.AgentIds.Remove(agent.Id);

        await _requestService.UpdateAsync(request);




        var message = $"{agent.Name} has submitted quote for your travel request number {request.Id}";

        await _notifier.DeleteNotification(request.Id, request.Requester.TravelHandler.Id, Events.RequestRaised);

        await _logService.InsertLog(request.Id, user.Id, request.Requester.TravelHandler.Id, Events.QuotationSent);

       
      

        // _notifier.InsertNotification(message, agent.Id, request.Requester.TravelHandler.EmpName, request.Id, Events.QuotationSent);
        
        // _logService.InsertLog(request.Id, agent.Id, request.Requester.TravelHandler.Id, Events.QuotationSent);


        // _mailer.SendMailQuoteReceived(request, agent);

        return Ok(request);


       }else{
        return Ok("There was an error");
       }

       


        }



   



     







      





       


     

        


}




 

