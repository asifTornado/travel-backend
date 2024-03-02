
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

namespace backEnd.Controllers.TicketQuotationControllers;




[Route("/")]
[ApiController]

public class TicketQuotationListController : ControllerBase
{

private readonly IBudgetsService _budgetService;
private IIDCheckService _idCheckService;
   




   


    public TicketQuotationListController(
        IBudgetsService budgetsService,
        IIDCheckService idCheckService
  )
    {
        _budgetService = budgetsService;
        _idCheckService = idCheckService;
    }
    



    [HttpPost]
    [Route("/getAllTicketQuotations")]
    public async Task<IActionResult> GetRequestForApproval(IFormCollection data){

        var allowed = await _idCheckService.CheckAdminOrManager(data["token"]);

        if(allowed == false){
            return Ok(false);
        }
     
              var result = await _budgetService.GetAllTicketQuotations();
              return Ok(result);
    }


    
    [HttpPost]
    [Route("/getTicketQuotationsForMe")]
    public async Task<IActionResult> GetTicketQuotationsForMe(IFormCollection data){
              var user = JsonSerializer.Deserialize<User>(data["user"]);
              var result = await _budgetService.GetTicketQuotationsForMe(user);
              return Ok(result);
    }


     [HttpPost]
    [Route("/getTicketQuotationsApprovedByMe")]
    public async Task<IActionResult> GetTicketQuotationsApprovedByMe(IFormCollection data){
              var user = JsonSerializer.Deserialize<User>(data["user"]);
              var result = await _budgetService.GetTicketQuotationsApprovedByMe(user);
              return Ok(result);
    }


    

   
}