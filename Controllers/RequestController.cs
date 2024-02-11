
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
using backEnd.services;
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

    private BudgetsService _budgetService;
    
    private ILogService _logService;


    private TripService _tripService;
    private IJwtTokenConverter _jwtTokenConverter;




   


    public RequestController(IJwtTokenConverter jwtTokenConverter, TripService tripService, BudgetsService budgetsService, ILogService logService, IQuotationService quotationService, TravelContext travelContext, IHelperClass helperClass, IFileHandler fileHandler, IUsersService usersService, IAgentsService agentsService, IMapper mapper, IRequestService requestService, IMailer mailer, INotifier notifier)
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
    [Route("submitRequest")]
    public async Task<IActionResult> SubmitRequest(IFormCollection data){

        var request = JsonSerializer.Deserialize<Request>(data["request"]);
        var user = JsonSerializer.Deserialize<User>(data["user"]);
        
   
        
      

        

        request.Requester = user;
    
      
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
        budget.Travelers.Add(request.Requester);
        budget.Requests = new List<Request>();
        

        var BudgetId = await _budgetService.CreateBudgetId(budget);
      
            
        //     var agentEmails = new List<Agent>();
        //  foreach(var agent in agents){
        //     request.Agents.Add(agent);
        //  }

   
      request.BudgetId = BudgetId;
      
      var requestId = await _requestService.CreateAsync(request); 

      var token = _jwtTokenConverter.GenerateToken(request.Requester.SuperVisor!);
            
            
            // foreach(var agent in agents){
            //       agentEmails.Add(agent);
                    
            // }

    //  _mailer.SendMail(agentEmails, request.Id, request);
        
      string message = $"A travel request from {request.Requester.EmpName} requires your approval for a new trip";


      await _notifier.InsertNotification(message, request.Requester.Id, request.Requester.SuperVisorId, request.Id, Events.SupervisorApprovalTrip);
      
      await _logService.InsertLog(requestId, request.Requester.Id, request.CurrentHandlerId, Events.SupervisorApprovalTrip);

      _mailer.SeekSupervisorApprovalTrip(request, token);


        return Ok(true);


}


[HttpPost]
[Route("getMyRequests")]
public async Task<IActionResult> GetMyRequests(IFormCollection data){
     

     var id = data["id"];
     var user = await _usersService.GetOneUser(int.Parse(id));

     Console.WriteLine("these are the user");
     Console.WriteLine(user.EmpName);
     var result =await _requestService.GetRequestssRaisedByUser(user);

     var resultDTO = _imapper.Map<List<RequestDTO>>(result);

     return Ok(resultDTO);



}



[HttpPost]
[Route("getAllRequests")]
public async Task<IActionResult> GetAllRequests(IFormCollection data){
     
     var id = data["id"];
     
     var result =await _requestService.GetAllRequests();


    var resultDTO = _imapper.Map<List<RequestDTO>>(result);

     return Ok(resultDTO);

 



}



[HttpPost]
[Route("getRequestsForMe")]
public async Task<IActionResult> GetRequestsForMe(IFormCollection data){
     
     var user = JsonSerializer.Deserialize<User>(data["user"]);
     var result = await _requestService.GetRequestsForMe(user);
   
     var resultDTO = _imapper.Map<List<RequestDTO>>(result);

     return Ok(resultDTO);



}



[HttpPost]
[Route("getRequestsProcessedByMe")]
public async Task<IActionResult> GetRequestsProcessedByMe(IFormCollection data){
     
     var user = JsonSerializer.Deserialize<User>(data["user"]);
     var result = await _requestService.GetRequestsProcessedByMe(user);

     var resultDTO = _imapper.Map<List<RequestDTO>>(result);

     return Ok(resultDTO);



}



[HttpPost]
[Route("getRequest")]
public async Task<IActionResult> GetRequest(IFormCollection data){
    var id = data["id"];
    var result = await _requestService.GetAsync(int.Parse(id));
    
  

    await _notifier.DeleteNotification(int.Parse(id), result.Requester.ZonalHead.Id, Events.ZonalHeadApproval);
    await _notifier.DeleteNotification(int.Parse(id), result.Requester.TravelHandler.Id, Events.RequestRaised);

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



//     [HttpPost]
//     [Route("departmentHeadApprove")]
//     public async Task<IActionResult> DepartmentHeadApprove(IFormCollection data){

//            Console.WriteLine("supervisor approve called");

//         var request = JsonSerializer.Deserialize<Request>(data["request"]);
        
//         var what = data["what"];
//         var approval = data["approval"];
//         var message = data["message"];
//         var messageObject = new Message();
//         string notmessage;
//         string newEvent;


//         Console.WriteLine("here are the values");
//         Console.WriteLine(request); 
//         Console.WriteLine(what);
//         Console.WriteLine(message);
//         Console.WriteLine(messageObject);
      
      
        

      

//  if(approval == "approved"){

  
//         request.DepartmentHeadApproved = true;
//         request.Status = "Seeking Quotations";
//         messageObject.Status = "Your travel request was approved";
//         messageObject.Content = message;
//         // request.TicketApprovals.Add(request.Requester.ZonalHead);
//         notmessage = $"{request.Requester.ZonalHead.EmpName} has approved your travel request with the id {request.Id}";
//         newEvent = Events.ZonalHeadApproval;
    

  

//   }else if(approval == "rejected"){
   
       
//         request.Status = "Request Rejected";
    
//          messageObject.Status = "Your travel request was rejected";
//         messageObject.Content = message;
//         notmessage = $"{request.Requester.ZonalHead.EmpName} has rejected your travel request with the id {request.Id}";
//         newEvent = Events.ZonalHeadReject;
  
//   }else{
//     notmessage = string.Empty;
//     newEvent = string.Empty;
//   }
       
        
        

//       request.Messages.Add(messageObject);



       
        
//         request.CurrentHandlerId = request.Requester.TravelHandler.Id;

     

        

//         await _requestService.UpdateAsync(request);

        
                    
     



//         await _notifier.InsertNotification(notmessage, request.Requester.Id, request.Requester.ZonalHead.Id, request.Id, newEvent);
//         await _notifier.DeleteNotification(request.Id, request.Requester.ZonalHead.Id, Events.ZonalHeadApproval);
//         await _logService.InsertLog(request.Id, request.Requester.ZonalHead.Id, request.Requester.TravelHandler.Id,  newEvent);
      
//         return Ok(request);

// }




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




    [HttpPost]
    [Route("giveQuotation")]
    public async Task<IActionResult> GiveQuote(IFormCollection data){

        var id = data["id"];
        var agentId = data["agentId"];
        var quotationString = data["quotation"];
        var quotation = new Quotation();
        var agent = await _agentsService.GetAsync(int.Parse(agentId));
        // quotation.Agent = agent;
        quotation.QuotationText = quotationString;



        var request = await _requestService.GetAsync(int.Parse(id));


        request.Quotations.Add(quotation);
        request.AgentNumbers--;

    

        // request.AgentIds.Remove(agent.Id);

        await _requestService.UpdateAsync(request);




        var message = $"{agent.Name} has submitted a quote for your travel request number {request.Id}";



        await _notifier.InsertNotification(message, agent.Id, request.Requester.Id, request.Id, Events.QuotationSent);
        await _logService.InsertLog(request.Id, request.Requester.Id, request.Requester.TravelHandler.Id, Events.QuotationSent);


        var travelHandler = await _usersService.GetUserByMail(request.Requester.TravelHandler.MailAddress);

       

      


/*        _mailer.SendMailQuoteReceived(request, agent, travelHandler.Id);
*/
        return Ok(request);



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



    [HttpPost]
    [Route("addCustomHotelQuote")]
    public async Task<IActionResult> AddCustomHotelQuote(IFormCollection data){

        var user = JsonSerializer.Deserialize<User>(data["user"]);
        var quoteGiver = data["quoteGiver"];
        var quotationString = data["quotation"];
        var quotation = new HotelQuotation();
        var agent = new Agent();
        var id = data["id"];
        agent.Email = user.MailAddress;
    
        agent.Phone = user.MobileNo;
        agent.Name = user.EmpName;
        agent.Professional = false;
        quotation.Custom = false;
        
        // quotation.Agent = agent;
        quotation.QuotationText = quotationString;



        var request = await _requestService.GetAsync(int.Parse(id));
        if(request.HotelQuotations == null){
              request.HotelQuotations = new List<HotelQuotation>();
        }

        request.HotelQuotations.Add(quotation);
        // // request.Agents--;

    

        // request.AgentIds.Remove(agent.Id);

        await _requestService.UpdateAsync(request);




        var message = $"{agent.Name} has submitted quote for hotel for your travel request number {request.Id}";



        // _notifier.InsertNotification(message, agent.Name, request.Requester.EmpName, request.Id);


        // _mailer.SendMailQuoteReceived(request, agent);


       await _notifier.DeleteNotification(request.Id, request.Requester.TravelHandler.Id, Events.RequestRaised);


       await _logService.InsertLog(request.Id, user.Id, request.Requester.TravelHandler.Id, Events.HotelQuotationSent);

        return Ok(request);



        }


        [HttpPost]
        [Route("bookQuotation")]
        public async Task<IActionResult> BookQuotation(IFormCollection data){
            var request = JsonSerializer.Deserialize<Request>(data["request"]);
            var quotationForFrontEnd = JsonSerializer.Deserialize<Quotation>(data["quotation"]);
            var user = JsonSerializer.Deserialize<User>(data["user"]);
            var best = data["best"];


      
            var quotation = new Quotation();
            if(quotationForFrontEnd.Custom == false){
             quotation = request.Quotations.FirstOrDefault(x => x.QuoteGiver == quotationForFrontEnd.QuoteGiver); 
            }else{
             quotation = request.Quotations.FirstOrDefault(x => x.QuoteGiver == quotationForFrontEnd.QuoteGiver);
            }

        
        
            quotation.Booked = true;
            request.Booked = true;
            request.Selected = true;

            // request.Agents = 0;
            // request.AgentIds = new List<string>()
            if( best == "Yes"){
            request.Status = "Seeking Confirmation";
            request.CurrentHandlerId = request.Requester.TravelHandler.Id;
            request.CurrentHandler = request.Requester.TravelHandler;
            quotation.Approved = true;
        
            //  _mailer.SendMailBook(quotation.Agent, request, quotation);
            }else{
                request.Status = "Seeking Supervisor's Approval";
                request.CurrentHandlerId = request.Requester.SuperVisor.Id;
                request.CurrentHandler = request.Requester.SuperVisor;
                var message = $"{request.Requester.EmpName} is seeking your approval regarding his travel request number {request.Id}";
                _notifier.InsertNotification(message, request.Requester.Id, request.Requester.SuperVisor.Id, request.Id, Events.SupervisorApprovalTicket);
                // _mailer.SeekSupervisorApproval(request, quotation.QuotationText, "ticket");
            }


            // var requestDTO = _imapper.Map<RequestDTO>(request);
            // var quotationDTO = _imapper.Map<QuotationDTO>(quotation);
            // await _requestService.UpdateAsyncDapper(requestDTO, quotationDTO, "dbo.Quotations");
           await _requestService.UpdateAsync(request);           
           await _logService.InsertLog(request.Id, user.Id, request.Requester.TravelHandler.Id, Events.QuotationBooked);
 
       
           
            
            return Ok(request); // need to be changed
        }



        [HttpPost]
        [Route("bookHotelQuotation")]
        public async Task<IActionResult> BookHotelQuotation(IFormCollection data){
            var request = JsonSerializer.Deserialize<Request>(data["request"]);
            var quotationFrontEnd = JsonSerializer.Deserialize<HotelQuotation>(data["quotation"]);
            var user = JsonSerializer.Deserialize<User>(data["user"]);
            var best = data["best"];
            var token = data["token"];
            


      

                 
            var quotation = new HotelQuotation();

            if(quotationFrontEnd.Custom == false){
           
             quotation = request.HotelQuotations.FirstOrDefault(x => x.QuoteGiver == quotationFrontEnd.QuoteGiver); 
            }else{
             quotation = request.HotelQuotations.FirstOrDefault(x => x.QuoteGiver == quotationFrontEnd.QuoteGiver);
            }

       

            quotation.Booked = true;

         

            request.HotelBooked = true;


            // request.Agents = 0;
            // request.AgentIds = new List<string>();

       
            if( best == "Yes"){

            request.Status = "Seeking Hotel Confirmation";
            request.CurrentHandlerId = request.Requester.TravelHandler.Id;
            request.CurrentHandler = request.Requester.TravelHandler;
            request.HotelBooked = true;
            quotation.Approved = true;
            }else{
                request.Status = "Seeking Supervisor's Approval For Hotel";
                request.CurrentHandlerId =request.Requester.SuperVisor.Id;
                request.CurrentHandler = request.Requester.SuperVisor;
                request.HotelBooked = true;
                var message = $"{request.Requester.EmpName} is seeking your approval regarding his travel request number {request.Id}";
                _notifier.InsertNotification(message, request.Requester.Id, request.Requester.SuperVisor.Id, request.Id, Events.SupervisorApprovalHotel);
                 _mailer.SeekSupervisorApproval(request, quotation.QuotationText, "hotel", token);
            }
             
             var requestDTO   =   _imapper.Map<RequestDTO>(request);
             var quotationDTO =   _imapper.Map<QuotationDTO>(quotation);

            //  foreach(var hotelQuotes in request.HotelQuotations){
            //     hotelQuotes.Requests = new List<Request>();
            //  }


            await _requestService.UpdateAsync(request);

            // _mailer.SendMailBook(quotation.Agent, request, quotation);


            await _logService.InsertLog(request.Id, user.Id, request.Requester.TravelHandler.Id, Events.HotelQuotationBooked);


   


            
      

            return Ok(request);
        }




        [HttpPost]
        [Route("confirm")]
        public async Task<IActionResult> Confirm(IFormCollection data){
            var request = JsonSerializer.Deserialize<Request>(data["request"]);
            var quotationFrontEnd = JsonSerializer.Deserialize<Quotation>(data["quotation"]);
            var user = JsonSerializer.Deserialize<User>(data["user"]);
            
          

            
      
            var quotation = new Quotation();

            if(quotationFrontEnd.Custom == false){
            quotation = request.Quotations.FirstOrDefault(x => x.QuoteGiver == quotationFrontEnd.QuoteGiver); 
            }else{
                quotation = request.Quotations.FirstOrDefault(x => x.QuoteGiver == quotationFrontEnd.QuoteGiver);
            }
            
            quotation.Confirmed = true;
            
            request.Confirmed = true;

            request.CurrentHandlerId = request.Requester.TravelHandler.Id;
            request.CurrentHandler = request.Requester.TravelHandler;
            request.Status = "Seeking Quotes For Hotel";
            request.SeekingInvoices = true;

            // await _requestService.UpdateAsync(request.Id, request);

            var requestDTO = _imapper.Map<RequestDTO>(request);
            var quotationDTO = _imapper.Map<QuotationDTO>(quotation);

            // await _requestService.UpdateAsyncDapper(requestDTO, quotationDTO, "dbo.Quotations");

            await _requestService.UpdateAsync(request);

           

            //  _mailer.SendMailConfirm(quotation.Agent, request, quotation);



            await _logService.InsertLog(request.Id, user.Id, request.Requester.TravelHandler.Id, Events.QuotationConfirmed);





           
            return Ok(request);
        }



        [HttpPost]
        [Route("revoke")]
        public async Task<IActionResult> Revoke(IFormCollection data){
            var request = JsonSerializer.Deserialize<Request>(data["request"]);
            var quotationFrontEnd = JsonSerializer.Deserialize<Quotation>(data["quotation"]);
            var user = JsonSerializer.Deserialize<User>(data["user"]);


             var quotation = new Quotation();

            if(quotationFrontEnd.Custom == false){
            quotation = request.Quotations.FirstOrDefault(x => x.QuoteGiver == quotationFrontEnd.QuoteGiver); 
            }else{
                quotation = request.Quotations.FirstOrDefault(x => x.QuoteGiver == quotationFrontEnd.QuoteGiver);
            }
            
            quotation.Confirmed = false;
            
            request.Confirmed = false;

            request.CurrentHandlerId = request.Requester.TravelHandler.Id;
            request.CurrentHandler = request.Requester.TravelHandler;
            request.Status = "Seeking Confirmation";
            request.SeekingInvoices = false;


            var requestDTO = _imapper.Map<RequestDTO>(request);
            var quotationDTO = _imapper.Map<QuotationDTO>(quotation);

            await _requestService.UpdateAsync(request);


             await _logService.InsertLog(request.Id, user.Id, request.Requester.TravelHandler.Id, Events.QuotationRevoked);

            // await _requestService.UpdateAsyncDapper(requestDTO, quotationDTO, "dbo.Quotations")
            //  _mailer.SendMailConfirm(quotation.Agent, request, quotation);

            return Ok(request);
        }




          [HttpPost]
        [Route("hotelRevoke")]
        public async Task<IActionResult> HotelRevoke(IFormCollection data){
            var request = JsonSerializer.Deserialize<Request>(data["request"]);
            var quotationFrontEnd = JsonSerializer.Deserialize<HotelQuotation>(data["quotation"]);
            var user = JsonSerializer.Deserialize<User>(data["user"]);
            var quotationFromDbo = await _quotationService.GetHotelQuotationById(quotationFrontEnd.Id);
            
 
             
                    var quotation = new HotelQuotation();

                 if(quotationFrontEnd.Custom == false){
                 quotation = request.HotelQuotations.FirstOrDefault(x => x.QuoteGiver == quotationFrontEnd.QuoteGiver); 
                 }else{
                     quotation = request.HotelQuotations.FirstOrDefault(x => x.QuoteGiver == quotationFrontEnd.QuoteGiver);
                 }
                 
                 quotation.Confirmed = false;
                 
                 request.HotelConfirmed = false;
     
                 request.CurrentHandler.Id = request.Requester.TravelHandler.Id;
                 request.CurrentHandler = request.Requester.TravelHandler;
                 request.Status = "Seeking Hotel Confirmation";
                 request.SeekingHotelInvoices = false;
     
     
                 var requestDTO = _imapper.Map<RequestDTO>(request);
                 var quotationDTO = _imapper.Map<QuotationDTO>(quotation);
     
                 // await _requestService.UpdateAsyncDapper(requestDTO, quotationDTO, "dbo.HotelQuotation");
     
                 await _requestService.UpdateAsync(request);
     
                 //  _mailer.SendMailConfirm(quotation.Agent, request, quotation);
     
                 await _logService.InsertLog(request.Id, user.Id, request.Requester.TravelHandler.Id, Events.HotelQuotationRevoked);




          

            return Ok(request);
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
        [Route("giveInvoice")]
        public async Task<IActionResult> GiveInvoice(IFormCollection data){
            var request = await _requestService.GetAsync(int.Parse(data["id"]));
            var agent = await _agentsService.GetAsync(int.Parse(data["agentId"]));
            request.SeekingInvoices = false;
            request.TicketInvoiceUploaded = true;

            long maxFileSize = 2 * 1024 * 1024;
            

            var fileName = _fileHandler.GetUniqueFileName(data.Files[0].FileName);

            var  filePath =   await _fileHandler.SaveFile(fileName, data.Files[0]);


            // var invoice =   new Invoice{
            //     Filename = filePath,
            //     Type = "ticket",
               
            // };

            // request.TicketInvoices.Add(filePath);
           

            // _mailer.SendMailInvoiceAccounts(request, agent);
            // _mailer.SendMailInvoiceRequester(request, agent);

            // request.Status = "Being Processed";

            request.CurrentHandler.Id = request.Requester.TravelHandler.Id;
            request.CurrentHandler = request.Requester.TravelHandler;

            // await _requestService.GiveInvoiceProfessional(request, invoice);


        
        var message = $"{agent.Name} is has submitted a new invoice for the travel request with the id {request.Id}";

        // _notifier.InsertNotification(message, agent.Id, request.Requester.TravelHandler.Id, request.Id, Events.InvoiceSent);

        
     
            return Ok(request);

       }



         [HttpPost]
        [Route("giveCustomInvoice")]
        public async Task<IActionResult> GiveCustomInvoice(IFormCollection data){


            var user = JsonSerializer.Deserialize<User>(data["user"]);
            
            long maxFileSize = 2 * 1024 * 1024;

            if(data.Files != null  && data.Files.Count > 0){
               if(data.Files[0].Length > maxFileSize){
                return Ok("size");
               }else{

                //  var request = await _requestService.GetAsync(int.Parse(data["id"]));
            // var agent = await _agentsService.GetAsync(data["agentId"]);
            var what = data["what"];


            var fileName = _fileHandler.GetUniqueFileName(data.Files[0].FileName);

            var  filePath =   await _fileHandler.SaveFile(fileName, data.Files[0]);

            var id = int.Parse(data["id"]);
            
        

           var (requester, travelHandler) =  await _requestService.UpdateInvoice(id, filePath, what);


        


  

        // var invoice = new Invoice{
        //     Filename = filePath,
        //     Type = what,
           
        // };
            




            // return Ok(invoice);
            return Ok(true);

               }
            }else{
                return Ok("empty");
            }

            
           
       
            

       }





        [HttpPost]
        [Route("processed")]
        public async Task<IActionResult> Processed(IFormCollection data){
            
            
            var request = JsonSerializer.Deserialize<Request>(data["request"]);

            request.Status = "Processing Complete";

            request.Processed = true;

            await _requestService.UpdateStatus(request);


            // _mailer.SendMailProcessed(request);


             string msg = $"Your travel request with the number {request.Id} has been processed.";

            await _notifier.InsertNotification(msg, request.Requester.TravelHandler.Id, request.Requester.Id, request.Id, Events.Processed);

            await _notifier.DeleteNotification(request.Id, request.Requester.TravelHandler.Id, Events.AirTicketInvoiceSent);
            await _notifier.DeleteNotification(request.Id, request.Requester.TravelHandler.Id, Events.HotelInvoiceSent);

            await _logService.InsertLog(request.Id, request.Requester.TravelHandler.Id, request.Requester.Id, Events.Processed);




            return Ok(request.Status);

       }



        [HttpPost]
        [Route("unBook")]
        public async Task<IActionResult> UnBook(IFormCollection data){
            var request = await _requestService.GetAsync(int.Parse(data["id"]));
            var quotationFrontEnd = JsonSerializer.Deserialize<Quotation>(data["quotation"]);
            var user = JsonSerializer.Deserialize<User>(data["user"]);
  
          var quotation = new Quotation();
            
            if(quotationFrontEnd.Custom == false){
                quotation = request.Quotations.FirstOrDefault(x=>x.QuoteGiver == quotationFrontEnd.QuoteGiver);
            }else{
                quotation = request.Quotations.FirstOrDefault(x=>x.QuoteGiver == quotationFrontEnd.QuoteGiver);
            }


            quotation.Booked = false;

            request.Booked = false;

            request.Status = "Seeking Quotations";

            request.CurrentHandlerId = request.Requester.TravelHandler.Id;
               request.CurrentHandler = request.Requester.TravelHandler;


            var requestDTO = _imapper.Map<RequestDTO>(request);
            var quotationDTO = _imapper.Map<QuotationDTO>(quotation);

            // await _requestService.UpdateAsyncDapper(requestDTO, quotationDTO, "dbo.Quotations");

            await _requestService.UpdateAsync(request);

            // string msg = $"Your hotel quotation for the travel number {request.Id} for {request.Requester.EmpName} ({request.Requester.Designation}) has been unbooked. Please contact Hameem Group for clarification.";

            // _mailer.Revert(request, quotation.Agent, msg, "Your quotation has been unbooked");

           await _logService.InsertLog(request.Id, user.Id, request.Requester.TravelHandler.Id, Events.QuotationUnbooked);



         
         
            return Ok(request);
            

       }




        [HttpPost]
        [Route("hotelunbook")]
        public async Task<IActionResult> HotelUnBook(IFormCollection data){
            var request = await _requestService.GetAsync(int.Parse(data["id"]));
            var quotationFrontEnd = JsonSerializer.Deserialize<Quotation>(data["quotation"]);
            var user = JsonSerializer.Deserialize<User>(data["user"]);

            
          
                  
                  var quotation = new HotelQuotation();
            
            if(quotationFrontEnd.Custom == false){
                quotation = request.HotelQuotations.FirstOrDefault(x=>x.QuoteGiver == quotationFrontEnd.QuoteGiver);
            }else{
                quotation = request.HotelQuotations.FirstOrDefault(x=>x.QuoteGiver == quotationFrontEnd.QuoteGiver);
            }


            quotation.Booked = false;

            request.HotelBooked = false;

            request.Status = "Seeking Hotel Quotations";

            request.CurrentHandlerId = request.Requester.TravelHandler.Id;
               request.CurrentHandler = request.Requester.TravelHandler;



            var requestDTO = _imapper.Map<RequestDTO>(request);
            var quotationDTO = _imapper.Map<QuotationDTO>(quotation);
            // await _requestService.UpdateAsyncDapper(requestDTO, quotationDTO, "dbo.HotelQuotation");
            await _requestService.UpdateAsync(request);


            await _logService.InsertLog(request.Id, user.Id, request.Requester.TravelHandler.Id, Events.HotelQuotationUnbooked);


            return Ok(request);

       }



         [HttpPost]
        [Route("hotelconfirm")]
        public async Task<IActionResult> HotelConfirm(IFormCollection data){
            var request = await _requestService.GetAsync(int.Parse(data["id"]));
            var quotationFrontEnd = JsonSerializer.Deserialize<Quotation>(data["quotation"]);
            var user = JsonSerializer.Deserialize<User>(data["user"]);

         

                 var quotation = new HotelQuotation();

            if(quotationFrontEnd.Custom == false){
            quotation = request.HotelQuotations.FirstOrDefault(x => x.QuoteGiver == quotationFrontEnd.QuoteGiver); 
            }else{
                quotation = request.HotelQuotations.FirstOrDefault(x => x.QuoteGiver == quotationFrontEnd.QuoteGiver);
            }
            
            quotation.Confirmed = true;
            
            request.HotelConfirmed = true;

            request.CurrentHandlerId = request.Requester.TravelHandler.Id;
               request.CurrentHandler = request.Requester.TravelHandler;
            request.Status = "Seeking Invoices";
            request.SeekingHotelInvoices = true;


            var requestDTO = _imapper.Map<RequestDTO>(request);
            var quotationDTO = _imapper.Map<QuotationDTO>(quotation);

            // await _requestService.UpdateAsyncDapper(requestDTO, quotationDTO, "dbo.HotelQuotation");


            await _requestService.UpdateAsync(request);

            //  _mailer.SendMailConfirm(quotation.Agent, request, quotation);

            await _logService.InsertLog(request.Id, user.Id, request.Requester.TravelHandler.Id, Events.HotelQuotationConfirmed);
            return Ok(request);
        }




        [HttpPost]
        [Route("emailRequest")]
        public async Task<IActionResult> EmailReqeust(IFormCollection data){
            var user = JsonSerializer.Deserialize<User>(data["user"]);
            var request = JsonSerializer.Deserialize<Request>(data["request"]);
            var recipient = data["recipient"];
            var whom = data["whom"];
            var type = data["type"];
            
            User auditor;

            auditor = whom == "accounts" ? await _usersService.GetAuditor() : null;

            

            request.Status = whom == "accounts" ? "Being Processed" : request.Status;


            await _requestService.UpdateStatus(request);

            _mailer.EmailRequest(request, recipient, auditor, type, this.ControllerContext, user);

           await _notifier.DeleteNotification(request.Id, user.Id, Events.AirTicketInvoiceSent);

            return Ok(request.Status);


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

    [HttpPost("getCustomRequests")]
    public async Task<IActionResult> GetCustomRequests(IFormCollection data){
        var result = await _requestService.GetCustomRequests();
        return Ok(result);
    }











     

        


}




 

