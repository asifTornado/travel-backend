
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
using Microsoft.EntityFrameworkCore.Update;
using Org.BouncyCastle.Asn1.IsisMtt.Ocsp;
using System.Diagnostics.CodeAnalysis;
using Org.BouncyCastle.Bcpg;

namespace backEnd.Controllers;



[Route("/")]
[ApiController]

public class TripController : ControllerBase
{
    private IBudgetsService _budgetsService;
    private TripService _tripService;
    private IMapper _imapper;
    private IFileHandler _fileHandler;
    private IUsersService _usersService;
    private IMailer _mailer;
    private ILogService _logService;
    private IIDCheckService _idCheckService;

    
    public TripController(IIDCheckService idCheckService, IMailer mailer, ILogService logService, IUsersService usersService, IBudgetsService budgetsService, IMapper mapper, TripService tripService, IFileHandler fileHandler)
    {    
        _idCheckService = idCheckService;
        _budgetsService = budgetsService;
        _imapper = mapper;
        _tripService = tripService;
        _fileHandler = fileHandler;
        _usersService = usersService;
        _mailer = mailer;
        _logService = logService;
        

    }


    [HttpPost("getAllTrips")]
    public async Task<IActionResult> GetAllTrips()
    {
        var results = await _budgetsService.GetAllInitiatedTrips();
        return Ok(results);
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


  



    [HttpPost("TAddCustomQuote")]
    public async Task<IActionResult> TAddCustomQuote(IFormCollection data)
    {         var token = data["token"];
              var allowed = await _idCheckService.CheckAdmin(token);
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
                     await _logService.InsertLogs(requestIds, userId, userId, Events.QuotationSent);
                     await _tripService.AddQuotations<Quotation>(quotations);
                     
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
                     await _logService.InsertLogs(requestIds, userId, userId, Events.HotelQuotationSent);
                   await _tripService.AddQuotations<HotelQuotation>(hotelQuotations);

                        return Ok(hotelQuotations[0]);

              }

            

    }



    [HttpPost("TAddHotelQuote")]
    public async Task<IActionResult> TAddHotelQuote(IFormCollection data){
           var token = data["token"];
              var allowed = await _idCheckService.CheckAdmin(token);
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
                request.CurrentHandlerId  = request.Requester?.TravelHandler.Id;

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

    
    [HttpPost("THotelBook")]
    public async Task<IActionResult> THotelBook(IFormCollection data){

          var token = data["token"];
              var allowed = await _idCheckService.CheckAdmin(token);
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
                request.CurrentHandlerId = request.Requester?.TravelHandler.Id;

                  foreach(var quotation2 in quotations ){
                          quotation2.Booked = true;   
                          quotation2.Approved = true;
         
                         }
              }else{

                  foreach(var quotation2 in quotations ){
                          quotation2.Booked = true;   
                          quotation2.Approved = false;
         
                         }

                request.Status = "Seeking Supervisor's Approval For Hotel";
                request.CurrentHandlerId = request.Requester?.SuperVisor.Id;
              }

             
       }

     

    await _logService.InsertLogs(requestIds, userId, userId, Events.HotelQuotationBooked);
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
              var allowed = await _idCheckService.CheckAdmin(token);
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
   
          }


          foreach(var quotation2 in quotations){
                quotation2.Booked = false;
                quotation2.Approved = false;
          }

          await _logService.InsertLogs(requestIds, userId, userId, Events.HotelQuotationUnbooked);
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
              var allowed = await _idCheckService.CheckAdmin(token);
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
     
        }

        foreach(var quotation2 in quotations){
            quotation2.Confirmed = true;
        }
        
        await _logService.InsertLogs(requestIds, userId, userId, Events.HotelQuotationConfirmed);
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
              var allowed = await _idCheckService.CheckAdmin(token);
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



    [HttpPost("TUploadTicketFile")]
    public async Task<IActionResult> TUploadTicketFiles(IFormCollection data){
           var token = data["token"];
              var allowed = await _idCheckService.CheckAdmin(token);
              if(allowed != true){
                return Ok(false);
              };
            
            var quotation = JsonSerializer.Deserialize<Quotation>(data["quotation"]);
               var userId = int.Parse(data["userId"]);

            var quotations = await _tripService.GetRelatedTicketQuotations(quotation);
            var requestIds = (await _tripService.GetRelatedRequestsFromQuotation(quotation)).Select(x => x.Id).ToList();
                                     

            long maxFileSize = 2 * 1024 * 1024;

            if(data.Files != null  && data.Files.Count > 0){
               if(data.Files[0].Length > maxFileSize){
                return Ok("size");
               }else{
                       
                   var fileName = _fileHandler.GetUniqueFileName(data.Files[0].FileName);

                   var  filePath =   await _fileHandler.SaveFile(fileName, data.Files[0]);

                   var ticketInvoice = new TicketInvoice(){ 
                    FilePath = filePath,
                    Filename = fileName,
                   };


                   ticketInvoice.Quotations.AddRange(quotations);
                 



                     await _tripService.AddOrUpdateTicketInvoice(ticketInvoice);
                     await _logService.InsertLogs(requestIds, userId, userId, Events.AirTicketInvoiceSent);

                    return Ok(ticketInvoice);

               
                


                   };
                    
                    }else{

                        return Ok("No Files");
                    }


                      }



    [HttpPost("TUploadHotelFile")]
    public async Task<IActionResult> TUploadHotelFiles(IFormCollection data){
           var token = data["token"];
              var allowed = await _idCheckService.CheckAdmin(token);
              if(allowed != true){
                return Ok(false);
              };
            
             var quotation = JsonSerializer.Deserialize<HotelQuotation>(data["quotation"]);
                var userId = int.Parse(data["userId"]);
                var requestIds = (await _tripService.GetRelatedRequestsFromHotelQuotation(quotation)).Select(x => x.Id).ToList();

            var quotations = await _tripService.GetRelatedHotelQuotations(quotation);


           

                         


            long maxFileSize = 2 * 1024 * 1024;

            if(data.Files != null  && data.Files.Count > 0){
               if(data.Files[0].Length > maxFileSize){
                return Ok("size");
               }else{
                       
                   var fileName = _fileHandler.GetUniqueFileName(data.Files[0].FileName);

                   var  filePath =   await _fileHandler.SaveFile(fileName, data.Files[0]);

                   var hotelInvoice = new HotelInvoice(){
                    FilePath = filePath,
                    Filename = fileName,
                   };


                   hotelInvoice.Quotations.AddRange(quotations);
                



                     await _tripService.AddOrUpdateHotelInvoice(hotelInvoice);
                     await _logService.InsertLogs(requestIds, userId, userId, Events.HotelInvoiceSent);

                    return Ok(hotelInvoice);

               
                


                   };
                    
                    }else{

                        return Ok("No Files");
                    }



    }



        [HttpPost]
        [Route("TEmailRequestsAccounts")]
        public async Task<IActionResult> TEmailRequestsAccounts(IFormCollection data){
               var token = data["token"];
              var allowed = await _idCheckService.CheckAdmin(token);
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
              var allowed = await _idCheckService.CheckAdmin(token);
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
              var allowed = await _idCheckService.CheckAdmin(token);
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