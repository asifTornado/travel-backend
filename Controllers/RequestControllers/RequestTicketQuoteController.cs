
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

public class RequestTicketQuoteController : ControllerBase
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
    private IIDCheckService _idCheckService;




   


    public RequestTicketQuoteController(
    IJwtTokenConverter jwtTokenConverter, TripService tripService, 
    IBudgetsService budgetsService, ILogService logService, 
    IQuotationService quotationService, TravelContext travelContext, 
    IHelperClass helperClass, IFileHandler fileHandler, 
    IUsersService usersService, IAgentsService agentsService, 
    IMapper mapper, IRequestService requestService, 
    IMailer mailer, INotifier notifier,
    IIDCheckService idCheckService
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
    }




    [HttpPost]
    [Route("giveQuotation")]
    public async Task<IActionResult> GiveQuote(IFormCollection data){

           var allowed = await _idCheckService.CheckAdminOrManager(data["token"]);

        if(allowed == false){
            return Ok(false);
        }

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
        [Route("bookQuotation")]
        public async Task<IActionResult> BookQuotation(IFormCollection data){

                var allowed = await _idCheckService.CheckAdminOrManager(data["token"]);

        if(allowed == false){
            return Ok(false);
        }

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
        [Route("confirm")]
        public async Task<IActionResult> Confirm(IFormCollection data){

                var allowed = await _idCheckService.CheckAdminOrManager(data["token"]);

        if(allowed == false){
            return Ok(false);
        }

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


                var allowed = await _idCheckService.CheckAdminOrManager(data["token"]);

        if(allowed == false){
            return Ok(false);
        }

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
        [Route("unBook")]
        public async Task<IActionResult> UnBook(IFormCollection data){

                var allowed = await _idCheckService.CheckAdminOrManager(data["token"]);

        if(allowed == false){
            return Ok(false);
        }

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




    





     

        


}




 

