
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

namespace backEnd.Controllers.RequestControllers;




[Route("/")]
[ApiController]

public class RequestHotelQuoteController : ControllerBase
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




   


    public RequestHotelQuoteController(IJwtTokenConverter jwtTokenConverter, TripService tripService, IBudgetsService budgetsService, ILogService logService, IQuotationService quotationService, TravelContext travelContext, IHelperClass helperClass, IFileHandler fileHandler, IUsersService usersService, IAgentsService agentsService, IMapper mapper, IRequestService requestService, IMailer mailer, INotifier notifier)
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




      
     

        


}




 

