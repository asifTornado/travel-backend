
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

public class TripInvoicController : ControllerBase
{
    private IBudgetsService _budgetsService;
    private TripService _tripService;
    private IMapper _imapper;
    private IFileHandler _fileHandler;
    private IUsersService _usersService;
    private IMailer _mailer;
    private ILogService _logService;
    private IIDCheckService _idCheckService;
    private readonly IJwtTokenConverter _jwtTokenConverter;

    
    public TripInvoicController(IJwtTokenConverter jwtTokenConverter, IIDCheckService idCheckService, IMailer mailer, ILogService logService, IUsersService usersService, IBudgetsService budgetsService, IMapper mapper, TripService tripService, IFileHandler fileHandler)
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



      

}