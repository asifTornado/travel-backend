
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

namespace backEnd.Controllers.TicketQuotationControllers;




[Route("/")]
[ApiController]

public class TicketQuotationController : ControllerBase
{


   private IBudgetsService _budgetService;
   private IMapper _imapper;
   private RoleService _roleService;




   


    public TicketQuotationController(IBudgetsService budgetsService, IMapper mapper, RoleService roleService)
    {
        _budgetService = budgetsService;
        _imapper = mapper;
        _roleService = roleService;
    }
    


    
    [HttpPost]
    [Route("/getTicketQuotations")]
    public async Task<IActionResult> GetRequestForApproval(IFormCollection data){
              var result = await _budgetService.GetAsync(int.Parse(data["id"]));

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

          

            foreach(var message in request.Messages)
            {
                tripDTO.Messages.Add(message);
            }

           
        }

        Console.WriteLine("Sending Trip");
        
        return Ok(tripDTO);
    }

    

    [HttpPost]
    [Route("sendToAccounts")]
    public async Task<IActionResult> SendToAccounts(IFormCollection data){
        var budget = JsonSerializer.Deserialize<Budget>(data["budget"]);
        var accounts = await _roleService.GetAccountsReceiverForExpenseReport();
        budget.SeekingAccountsApprovalForTickets = true;
        budget.CurrentHandlerId = accounts.Id;

        await _budgetService.UpdateAsync(budget.Id, budget);

        return Ok(true);
    }



  [HttpPost]
  [Route("ticketQuotationsMoneyDisburse")]
  public async Task<IActionResult> Disburse(IFormCollection data){
    var ticketQuotations = JsonSerializer.Deserialize<TripDTO>(data["ticketQuotations"]);
    var user = JsonSerializer.Deserialize<User>(data["user"]);
    
    ticketQuotations.TicketsMoneyDisbursed = true;
    var budget = _imapper.Map<Budget>(ticketQuotations);

    var newApproval = new BudgetTicketApprovals{
        BudgetId = budget.Id,
        UserId = user.Id
    };

    await _budgetService.InsertBudgetTicketApprover(newApproval);
   
    
    await _budgetService.UpdateAsync(ticketQuotations.Id, budget);
    

    return Ok(ticketQuotations);

  } 
    

    
   

}