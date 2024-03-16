
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

namespace backEnd.Controllers.TripControllers;



[Route("/")]
[ApiController]

public class TripHotelQuoteController : ControllerBase
{
    private IBudgetsService _budgetsService;
    private ITripService _tripService;
    private IMapper _imapper;
    private IFileHandler _fileHandler;
    private IUsersService _usersService;
    private IMailer _mailer;
    private ILogService _logService;
    private IIDCheckService _idCheckService;
    private readonly IJwtTokenConverter _jwtTokenConverter;
    private INotifier _notifier;
    private MailerWorkFlow _mailerWorkFlow;
    

    
    public TripHotelQuoteController(IJwtTokenConverter jwtTokenConverter, IIDCheckService idCheckService, 
    IMailer mailer, ILogService logService, IUsersService usersService, 
    IBudgetsService budgetsService, IMapper mapper, ITripService tripService, IFileHandler fileHandler,
    INotifier notifier, MailerWorkFlow mailerWorkFlow
    )
    {    
        _idCheckService = idCheckService;
        _budgetsService = budgetsService;
        _imapper = mapper;
        _tripService = tripService;
        _fileHandler = fileHandler;
        _usersService = usersService;
        _mailer = mailer;
        _logService = logService;
        _jwtTokenConverter = jwtTokenConverter;
        _notifier = notifier;
        _mailerWorkFlow = mailerWorkFlow;
        


    }


  



    [HttpPost("TAddHotelQuote")]
    public async Task<IActionResult> TAddHotelQuote(IFormCollection data){
           var token = data["token"];
              var allowed = await _idCheckService.CheckAdminOrManager(token);
              if(allowed != true){
                return Ok(false);
              };
        var quote = data["quoteString"];
        var quoteGiver  = data["quoteGiver"];
        var tripId = data["tripId"];
        var requestIds = JsonSerializer.Deserialize<List<int>>(data["requestIds"]);
        var travelerCosts = JsonSerializer.Deserialize<List<TravelerCost>>(data["travelerCosts"]);
        var userId = int.Parse(data["userId"]);

        var guid = Guid.NewGuid();

        var hotelQuotations = new List<HotelQuotation>();

        foreach(var id in requestIds){
            var newHotelQuotation = new HotelQuotation();
            newHotelQuotation.Linker = guid;
            newHotelQuotation.QuotationText = quote;
            newHotelQuotation.QuoteGiver = quoteGiver;
            newHotelQuotation.RequestId = id;
            newHotelQuotation.RequestIds = requestIds;
            newHotelQuotation.TotalCosts = travelerCosts;
            hotelQuotations.Add(newHotelQuotation);
           
            
        }

        await _logService.InsertLogs(requestIds, userId, userId, Events.HotelQuotationSent);

        await _tripService.AddQuotations<HotelQuotation>(hotelQuotations);

   
     


          return Ok(hotelQuotations[0]);

    }

   

    
    [HttpPost("THotelBook")]
    public async Task<IActionResult> THotelBook(IFormCollection data){

          var token = data["token"];
              var allowed = await _idCheckService.CheckAdminOrManager(token);
              if(allowed != true){
                return Ok(false);
              };
       var quotation = JsonSerializer.Deserialize<HotelQuotation>(data["quotation"]);
       var requests = await _tripService.GetRelatedRequestsFromHotelQuotation(quotation);
       var quotations = await _tripService.GetRelatedHotelQuotations(quotation);
       var userId = int.Parse(data["userId"]);
       var requestIds = requests.Select( x => x.Id).ToList();

       var best = data["condition"];
       
       foreach(var request in requests){
              request.HotelBooked = true;
              if(best == "Yes"){
                request.Status = "Seeking Hotel Confirmation";
                request.CurrentHandlerId = null;

                  foreach(var quotation2 in quotations ){
                          quotation2.Booked = true;   
                          quotation2.Approved = true;
         
                         }

                  var mailToken = _jwtTokenConverter.GenerateToken(request.Requester);
                 var message = $"A Hotel Quotation has been booked for your trip numbered {requests[0].BudgetId}";
                 await _notifier.InsertNotification(message, userId, request.Requester.Id, request.Id, Events.HotelQuotationBooked);
                 _mailerWorkFlow.WorkFlowMail(request.Requester.MailAddress, message, request.Id, "showRequest", mailToken, "Hotel Quotation Booked");
                 await _logService.InsertLog(request.Id, userId, request.Requester.Id, Events.HotelQuotationBooked);
              }else{

                  foreach(var quotation2 in quotations ){
                          quotation2.Booked = true;   
                          quotation2.Approved = false;
         
                         }

                request.Status = "Seeking Supervisor's Approval For Hotel";
                 var message = $"An Hotel Quotation for the trip numbered {requests[0].BudgetId} requires your approval";
                 await _notifier.InsertNotification(message, userId, request.Requester.Id, request.Id, Events.SupervisorApprovalHotel);
                request.CurrentHandlerId = request.Requester?.SuperVisor.Id;
                var token2 = _jwtTokenConverter.GenerateToken(request.Requester.SuperVisor);
                _mailer.SeekSupervisorApproval(request, quotations[0].QuotationText, "hotel", token2);
                await _logService.InsertLog(request.Id, userId, request.Requester.Id, Events.SupervisorApprovalHotel);

              }

             
       }

     

    await _tripService.UpdateHotelQuotationsAndRequests(quotations, requests);
    var quotationToSend = quotations.FirstOrDefault(x => x.Id == quotation.Id);
     var requestToSend = requests.FirstOrDefault(x => x.Id == quotationToSend.RequestId);

            var result = new {
              quotation=quotationToSend,
              request = requestToSend
            };



 return Ok(result);

       
    }

  
    [HttpPost("THotelUnBook")]
    public async Task<IActionResult> THotelUnBook(IFormCollection data){
           var token = data["token"];
              var allowed = await _idCheckService.CheckAdminOrManager(token);
              if(allowed != true){
                return Ok(false);
              };
        var quotation = JsonSerializer.Deserialize<HotelQuotation>(data["quotation"]);

        var requests = await _tripService.GetRelatedRequestsFromHotelQuotation(quotation);
        var requestIds = requests.Select(x => x.Id).ToList();
        var quotations  = await _tripService.GetRelatedHotelQuotations(quotation);
           var userId = int.Parse(data["userId"]);

        foreach(var request in requests){

              request.HotelBooked = false;
              request.Status = "Seeking Hotel Quotations";

              
                var mailToken = _jwtTokenConverter.GenerateToken(request.Requester);
                var message = $"A Hotel Quotation has been unbooked for your trip numbered {requests[0].BudgetId}";
                await _notifier.InsertNotification(message, userId, request.Requester.Id, request.Id, Events.HotelQuotationBooked);
                await _logService.InsertLog(request.Id, userId, request.Requester.Id, Events.HotelQuotationBooked);
                _mailerWorkFlow.WorkFlowMail(request.Requester.MailAddress, message, request.Id, "showRequest", mailToken, "Hotel Quotation Quotation UnBooked");
   
          }


          foreach(var quotation2 in quotations){
                quotation2.Booked = false;
                quotation2.Approved = false;
          }

          await _tripService.UpdateHotelQuotationsAndRequests(quotations, requests);
    var quotationToSend = quotations.FirstOrDefault(x => x.Id == quotation.Id);

     var requestToSend = requests.FirstOrDefault(x => x.Id == quotationToSend.RequestId);

            var result = new {
              quotation=quotationToSend,
              request = requestToSend
            };

 return Ok(result);

    }


    [HttpPost("THotelConfirm")]
    public async Task<IActionResult> THotelConfirm(IFormCollection data){
           var token = data["token"];
              var allowed = await _idCheckService.CheckAdminOrManager(token);
              if(allowed != true){
                return Ok(false);
              };
        var quotation = JsonSerializer.Deserialize<HotelQuotation>(data["quotation"]);
           var userId = int.Parse(data["userId"]);

        var requests = await _tripService.GetRelatedRequestsFromHotelQuotation(quotation);
        var requestIds = requests.Select(x => x.Id).ToList();
        
        var quotations = requests.SelectMany(x => x.HotelQuotations).Select(x => x).Where(x => x.Booked == true).ToList();

        foreach(var request in requests){
            request.HotelConfirmed = true;
            request.Status = "Seeking Invoices";

             var mailToken = _jwtTokenConverter.GenerateToken(request.Requester);
              var message = $"A Hotel Quotation has been confirmed for your trip numbered {requests[0].BudgetId}";
              await _notifier.InsertNotification(message, userId, request.Requester.Id, request.Id, Events.HotelQuotationConfirmed);
              await _logService.InsertLog(request.Id, userId, request.Requester.Id, Events.HotelQuotationConfirmed);
              _mailerWorkFlow.WorkFlowMail(request.Requester.MailAddress, message, request.Id, "showRequest", mailToken, "Hotel Quotation Confirmed");
     
        }

        foreach(var quotation2 in quotations){
            quotation2.Confirmed = true;
        }
        
        await _tripService.UpdateHotelQuotationsAndRequests(quotations, requests);

            var quotationToSend = quotations.FirstOrDefault(x => x.Id == quotation.Id);

             var requestToSend = requests.FirstOrDefault(x => x.Id == quotationToSend.RequestId);

            var result = new {
              quotation=quotationToSend,
              request = requestToSend
            };

         return Ok(result);

    }

    [HttpPost("THotelRevoke")]
    public async Task<IActionResult> THotelRevoke(IFormCollection data){
           var token = data["token"];
              var allowed = await _idCheckService.CheckAdminOrManager(token);
              if(allowed != true){
                return Ok(false);
              };

          var quotation = JsonSerializer.Deserialize<HotelQuotation>(data["quotation"]);
             var userId = int.Parse(data["userId"]);

        var requests = await _tripService.GetRelatedRequestsFromHotelQuotation(quotation);
        var requestIds = requests.Select(x => x.Id).ToList();
        var quotations = requests.SelectMany(x => x.HotelQuotations).Select(x => x).Where(x => x.Confirmed == true).ToList();

        foreach(var request in requests){

              request.HotelConfirmed = false;
              request.Status = "Seeking Hotel Confirmation";
              await _logService.InsertLog(request.Id, userId, userId, Events.HotelQuotationRevoked);

               var mailToken = _jwtTokenConverter.GenerateToken(request.Requester);
            var message = $"A Hotel Quotation has been revoked for your trip numbered {requests[0].BudgetId}";
            await _notifier.InsertNotification(message, userId, request.Requester.Id, request.Id, Events.HotelQuotationRevoked);
            await _logService.InsertLog(request.Id, userId, request.Requester.Id, Events.HotelQuotationRevoked);
            _mailerWorkFlow.WorkFlowMail(request.Requester.MailAddress, message, request.Id, "showRequest", mailToken, "Hotel Quotation Revoked");
          }


 
        foreach(var quotation2 in quotations ){
                quotation2.Confirmed = false;
        
        }

          await _logService.InsertLogs(requestIds, userId, userId, Events.HotelQuotationRevoked);
          await _tripService.UpdateHotelQuotationsAndRequests(quotations, requests);

            var quotationToSend = quotations.FirstOrDefault(x => x.Id == quotation.Id);

             var requestToSend = requests.FirstOrDefault(x => x.Id == quotationToSend.RequestId);

            var result = new {
              quotation=quotationToSend,
              request = requestToSend
            };

         return Ok(result);
        
    }



    

}