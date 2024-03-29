
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

public class TripQuoteController : ControllerBase
{
    private IBudgetsService _budgetsService;
    private ITripService _tripService;
    private IMapper _imapper;
    private IFileHandler _fileHandler;
    private IUsersService _usersService;
    private IMailer _mailer;
    private ILogService _logService;
    private IIDCheckService _idCheckService;

    private INotifier _notifier;

    private MailerWorkFlow _mailerWorkFlow;

    private IRequestService _requestService;
    private readonly IJwtTokenConverter _jwtTokenConverter;

    
    public TripQuoteController(IJwtTokenConverter jwtTokenConverter, IIDCheckService idCheckService,
     IMailer mailer, ILogService logService, IUsersService usersService,
      IBudgetsService budgetsService, IMapper mapper, ITripService tripService, 
      IFileHandler fileHandler, MailerWorkFlow mailerWorkFlow, INotifier notifier,
      IRequestService requestService
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
        _mailerWorkFlow = mailerWorkFlow;
        _notifier = notifier;
        _requestService = requestService;
        


    }


  


    [HttpPost("TAddCustomQuote")]
    public async Task<IActionResult> TAddCustomQuote(IFormCollection data)
    {         var token = data["token"];
              var allowed = await _idCheckService.CheckAdminOrManager(token);
              if(allowed != true){
                return Ok(false);
              };
              var quotation = data["quotation"];
              var quoteGiver = data["quoteGiver"];
              var tripId = int.Parse(data["tripId"]);
              var requestIds = JsonSerializer.Deserialize<List<int>>(data["requestIds"]);
              var what = data["what"];
              var userId = int.Parse(data["userId"]);
              var travelerCosts = JsonSerializer.Deserialize<List<TravelerCost>>(data["travelerCosts"]);
              List<Log> logs = new List<Log>();

              if(what == "ticket"){
                List<Quotation> quotations = new List<Quotation>();
                      var guid = Guid.NewGuid();
                      foreach(var id in requestIds){

                       var newQuotation  = new Quotation();
                       newQuotation.Linker = guid;
                       newQuotation.QuotationText = quotation;
                       newQuotation.QuoteGiver = quoteGiver;
                       newQuotation.RequestId = id;
                       newQuotation.Custom = true;
                       newQuotation.RequestIds = requestIds;
                       newQuotation.TotalCosts = travelerCosts;
                      
                       quotations.Add(newQuotation);
             
                }
                    
                     await _tripService.AddQuotations<Quotation>(quotations);

                      var newRequestIds = await _requestService.GetRequestsFromRequestIds(requestIds);
                   


                   var message = $"A new hotel quotation has been added for your trip numbered {tripId}";
                     
                     foreach(var request in newRequestIds){
                       var mailToken = _jwtTokenConverter.GenerateToken(request.Requester);
                        await _logService.InsertLog(request.Id, userId, request.RequesterId, Events.QuotationSent);
                      await _notifier.InsertNotification(message, userId, request.Requester.Id, request.Id, Events.QuotationAdded);
                       _mailerWorkFlow.WorkFlowMail(request.Requester.MailAddress, message, request.Id, "showRequest", mailToken, "New Hotel Quotation");
                     }

                     
                     return Ok(quotations[0]);

              }else {
                List<HotelQuotation> hotelQuotations = new List<HotelQuotation>();
                         var guid = Guid.NewGuid();
                      foreach(var id in requestIds){

                       var newHotelQuotation  = new HotelQuotation();
                       
                       newHotelQuotation.QuotationText = quotation;
                       newHotelQuotation.QuoteGiver = quoteGiver;
                       newHotelQuotation.RequestId = id;
                       newHotelQuotation.Custom = true;
                       newHotelQuotation.Linker = guid;
                       newHotelQuotation.RequestIds = requestIds;
                       newHotelQuotation.TotalCosts = travelerCosts;
                       hotelQuotations.Add(newHotelQuotation);
                  
                }


                   await _tripService.AddQuotations<HotelQuotation>(hotelQuotations);

                   var newRequestIds = await _requestService.GetRequestsFromRequestIds(requestIds);
                   


                   var message = $"A new hotel quotation has been added for your trip numbered {tripId}";
                     
                     foreach(var request in newRequestIds){

                           await _logService.InsertLog(request.Id, userId, request.RequesterId, Events.HotelQuotationSent);
                       var mailToken = _jwtTokenConverter.GenerateToken(request.Requester);
                      await _notifier.InsertNotification(message, userId, request.Requester.Id, request.Id, Events.HotelQuotationSent);
                       _mailerWorkFlow.WorkFlowMail(request.Requester.MailAddress, message, request.Id, "showRequest", mailToken, "New Hotel Quotation");

                     }

                   

                        return Ok(hotelQuotations[0]);

              }

            

    }




}