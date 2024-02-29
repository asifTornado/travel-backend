
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

namespace backEnd.Controllers.TripControllers;



[Route("/")]
[ApiController]

public class TripTicketQuoteController : ControllerBase
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

    
    public TripTicketQuoteController(IJwtTokenConverter jwtTokenConverter, IIDCheckService idCheckService, IMailer mailer, ILogService logService, IUsersService usersService, IBudgetsService budgetsService, IMapper mapper, ITripService tripService, IFileHandler fileHandler)
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
        


    }



   

  



   


   

    [HttpPost("TTicketBook")]
    public async Task<IActionResult> TTicketBook(IFormCollection data)
    {
             var token = data["token"];
              var allowed = await _idCheckService.CheckAdmin(token);
              if(allowed != true){
                return Ok(false);
              };


       var quotation = JsonSerializer.Deserialize<Quotation>(data["quotation"]);
       var requestIds = quotation.RequestIds;
       var best = data["condition"];
       var userId = int.Parse(data["userId"]);

       var requests = await _tripService.GetRelatedRequestsFromQuotation(quotation);
       var quotations = await _tripService.GetRelatedTicketQuotations(quotation);

       foreach(var request in requests){
             request.Booked = true;

             if(best == "Yes"){
                request.Status = "Seeking Confirmation";
                request.CurrentHandlerId  = null;

                  foreach(var quotation2 in quotations){
                      quotation2.Booked = true;
                      quotation2.Approved = true;
                 }
       
              }else{
                request.Status =  "Seeking Supervisor's Approval"; 
                request.CurrentHandlerId = request.Requester?.SuperVisor.Id;

                    foreach(var quotation2 in quotations){
                      quotation2.Booked = true;
                      quotation2.Approved = false;
                 }
                var token2 = _jwtTokenConverter.GenerateToken(request.Requester.SuperVisor);
                _mailer.SeekSupervisorApproval(request, quotations[0].QuotationText, "air-ticket", token2);
              }

               
               
       }

    
         await _logService.InsertLogs(requestIds, userId, userId, Events.SupervisorApprovalTicket);
       await _tripService.UpdateTicketQuotationsAndRequests(quotations, requests);
        var quotationToSend = quotations.FirstOrDefault(x => x.Id == quotation.Id);
         var requestToSend = requests.FirstOrDefault(x => x.Id == quotationToSend.RequestId);

            var result = new {
              quotation=quotationToSend,
              request = requestToSend
            };
       return Ok(result);

}

    
   

    [HttpPost("TUnBook")]
    public async Task<IActionResult> TUnBook(IFormCollection data)
    {
         var token = data["token"];
              var allowed = await _idCheckService.CheckAdmin(token);
              if(allowed != true){
                return Ok(false);
              };
       var quotation = JsonSerializer.Deserialize<Quotation>(data["quotation"]);
          var userId = int.Parse(data["userId"]);
       var requestIds = quotation.RequestIds;


       var requests = await _tripService.GetRelatedRequestsFromQuotation(quotation);
       var quotations = requests.SelectMany(x => x.Quotations).Select(x => x).Where(x => x.Booked == true).ToList();
       

       foreach(var request in requests){
        request.Booked = false;
        request.Status = "Seeking Quotations";

       }

       foreach(var Dquotation in quotations){
            Dquotation.Booked = false;
            Dquotation.Approved = false;
       }
       
       await _logService.InsertLogs(requestIds, userId, userId, Events.QuotationUnbooked);
       await _tripService.UpdateTicketQuotationsAndRequests(quotations, requests);

           var quotationToSend = quotations.FirstOrDefault(x => x.Id == quotation.Id);

            var requestToSend = requests.FirstOrDefault(x => x.Id == quotationToSend.RequestId);

            var result = new {
              quotation=quotationToSend,
              request = requestToSend
            };

     return Ok(result);

}


    [HttpPost("TTicketConfirm")]
    public async Task<IActionResult> TTicketConfirm(IFormCollection data){
           var token = data["token"];
              var allowed = await _idCheckService.CheckAdmin(token);
              if(allowed != true){
                return Ok(false);
              };
        var quotation = JsonSerializer.Deserialize<Quotation>(data["quotation"]);
           var userId = int.Parse(data["userId"]);
        var requests =  await _tripService.GetRelatedRequestsFromQuotation(quotation);
        var requestIds = requests.Select(x => x.Id).ToList();
        var quotations = requests.SelectMany(x => x.Quotations).Select(x => x).Where(x => x.Booked == true).ToList();


        foreach(var request in requests){
            request.Confirmed = true;
            request.Status = "Seeking Quotes For Hotel";
        }

        foreach(var LQuotation in quotations){
            LQuotation.Confirmed = true;
        }
        

        await _logService.InsertLogs(requestIds, userId, userId, Events.QuotationConfirmed);
        await _tripService.UpdateTicketQuotationsAndRequests(quotations, requests);
        

            var quotationToSend = quotations.FirstOrDefault(x => x.Id == quotation.Id);
            var requestToSend = requests.FirstOrDefault(x => x.Id == quotationToSend.RequestId);

            var result = new {
              quotation=quotationToSend,
              request = requestToSend
            };

         return Ok(result);



    }
 

    [HttpPost("TTicketRevoke")]
    public async Task<IActionResult> TTicketRevoke(IFormCollection data){
           var token = data["token"];
              var allowed = await _idCheckService.CheckAdmin(token);
              if(allowed != true){
                return Ok(false);
              };
        var quotation = JsonSerializer.Deserialize<Quotation>(data["quotation"]);
           var userId = int.Parse(data["userId"]);
        var requests =  await _tripService.GetRelatedRequestsFromQuotation(quotation);
        var requestIds = requests.Select(x => x.Id).ToList();
        var quotations = requests.SelectMany(x => x.Quotations).Select(x => x).Where(x => x.Confirmed == true).ToList();


        foreach(var request in requests){
            request.Confirmed = false;
            request.Status = "Seeking Hotel Confirmation";
        }

        foreach(var LQuotation in quotations){
            LQuotation.Confirmed = false;
        }

        
        await _tripService.UpdateTicketQuotationsAndRequests(quotations, requests);
        await _logService.InsertLogs(requestIds, userId, userId, Events.QuotationRevoked);

            var quotationToSend = quotations.FirstOrDefault(x => x.Id == quotation.Id);
             var requestToSend = requests.FirstOrDefault(x => x.Id == quotationToSend.RequestId);

            var result = new {
              quotation=quotationToSend,
              request = requestToSend
            };

           return Ok(result);
    }




}