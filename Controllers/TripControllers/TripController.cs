
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

public class TripController : ControllerBase
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
    private readonly RoleService _rolesService;

    
    public TripController(IJwtTokenConverter jwtTokenConverter, RoleService rolesService, IIDCheckService idCheckService, IMailer mailer, ILogService logService, IUsersService usersService, IBudgetsService budgetsService, IMapper mapper, ITripService tripService, IFileHandler fileHandler)
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
        _rolesService = rolesService;
        


    }




    [HttpPost("getTrip")]
    public async Task<IActionResult> GetTrip(IFormCollection data)
    {
        var result = await _budgetsService.GetAsync(int.Parse(data["id"]));

            var tripDTO = _imapper.Map<TripDTO>(result);

          var quotationTracker = new List<Guid?>();
          var hotelQuotationTracker = new List<Guid?>();
         

        foreach(var request in result.Requests)
        {
            

            foreach(var quotation in request.Quotations)
            {
                if(quotationTracker.Any(x => x == quotation.Linker))
                {
                    continue;
                }else{
                  quotationTracker.Add(quotation.Linker);
                  tripDTO.Quotations.Add(quotation);
                

                }
            }

            foreach(var hotelQuotation in request.HotelQuotations)
            {    
                if(hotelQuotationTracker.Any(x => x == hotelQuotation.Linker))
                {
                    continue;
                }else{
                hotelQuotationTracker.Add(hotelQuotation.Linker);
                tripDTO.HotelQuotations.Add(hotelQuotation);

                }
            }

            foreach(var message in request.Messages)
            {
                tripDTO.Messages.Add(message);
            }

           
        }

        Console.WriteLine("Sending Trip");
        
        return Ok(tripDTO);
    }


  



  


        [HttpPost]
        [Route("TEmailRequestsAccounts")]
        public async Task<IActionResult> TEmailRequestsAccounts(IFormCollection data){
               var token = data["token"];
              var allowed = await _idCheckService.CheckAdminOrManager(token);
              if(allowed != true){
                return Ok(false);
              };
            var user = JsonSerializer.Deserialize<User>(data["user"]);
            var trip = JsonSerializer.Deserialize<TripDTO>(data["trip"]);
            var userId = int.Parse(data["userId"]);
            var recipient = data["recipient"];
            var requestIds = trip.Requests.Select( x => x.Id).ToList();
        
            var auditor = await _usersService.GetAuditor();
        

            

            List<Request> requests = new List<Request>();   

            foreach(var request in trip.Requests){
                request.Status = "Being Processed";
                request.BeingProcessed = true;
                requests.Add(request);
            }

            _mailer.TEmailRequestsAccounts(requests, recipient, auditor, user);


            await _tripService.UpdateRequests(requests);
            await _logService.InsertLogs(requestIds, userId,  userId, Events.MailedAccountsAndAudit);

        

        

            return Ok(true);


        }


         [HttpPost]
         [Route("TEmailRequestsCustom")]
         public async Task<IActionResult> TEmailRequestsCustom(IFormCollection data){
               var token = data["token"];
              var allowed = await _idCheckService.CheckAdminOrManager(token);
              if(allowed != true){
                return Ok(false);
              };
            var user = JsonSerializer.Deserialize<User>(data["user"]);
            var trip = JsonSerializer.Deserialize<TripDTO>(data["trip"]);
            var recipient = data["recipient"];
               var userId = int.Parse(data["userId"]);
        
            

            

            List<Request> requests = new List<Request>();   

            foreach(var request in trip.Requests){
           
                requests.Add(request);
            }

            _mailer.TEmailRequestsCustom(requests, recipient, user);


            return Ok(true);
         }


         [HttpPost]
         [Route("TComplete")]
            public async Task<IActionResult> TEmailRequests(IFormCollection data){
                   var token = data["token"];
              var allowed = await _idCheckService.CheckAdminOrManager(token);
              if(allowed != true){
                return Ok(false);
              };
                
                var trip = JsonSerializer.Deserialize<TripDTO>(data["trip"]);
                var userId = int.Parse(data["userId"]);
              
                List<Request> requests = new List<Request>();   
    
                foreach(var request in trip.Requests){
                    request.Processed = true;
                    request.Status = "Processing Complete";
                    requests.Add(request);
                }
               
               var requestIds = requests.Select(x => x.Id).ToList();

               await _logService.InsertLogs(requestIds, userId, userId, Events.Processed);
               await _tripService.UpdateRequests(requests);


               Console.WriteLine("Completed the function");

               return Ok(true);

}


}