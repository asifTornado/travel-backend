
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

public class RequestListsController : ControllerBase
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


    private ITripService _tripService;
    private IJwtTokenConverter _jwtTokenConverter;
    private IIDCheckService _idCheckService;




   


    public RequestListsController(IJwtTokenConverter jwtTokenConverter, ITripService tripService, 
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
[Route("getMyRequests")]
public async Task<IActionResult> GetMyRequests(IFormCollection data){
     

     var id = data["id"];
     var user = await _usersService.GetOneUser(int.Parse(id));

     Console.WriteLine("these are the user");
     Console.WriteLine(user.EmpName);
     var result =await _requestService.GetRequestssRaisedByUser(user);

   

     return Ok(result);



}



[HttpPost]
[Route("getAllRequests")]
public async Task<IActionResult> GetAllRequests(IFormCollection data){

    var allowed = await _idCheckService.CheckAdminOrManager(data["token"]);

    if(allowed == false){
        return Ok(false);
    }
     
     var id = data["id"];
     
     var result =await _requestService.GetAllRequests();


 

     return Ok(result);

 



}



[HttpPost]
[Route("getRequestsForMe")]
public async Task<IActionResult> GetRequestsForMe(IFormCollection data){
     
     var user = JsonSerializer.Deserialize<User>(data["user"]);
     var result = await _requestService.GetRequestsForMe(user);
   
     var resultDTO = _imapper.Map<List<RequestDTO>>(result);

     return Ok(resultDTO);



}



[HttpPost]
[Route("getRequestsProcessedByMe")]
public async Task<IActionResult> GetRequestsProcessedByMe(IFormCollection data){
     
     var user = JsonSerializer.Deserialize<User>(data["user"]);
     var result = await _requestService.GetRequestsProcessedByMe(user);

     var resultDTO = _imapper.Map<List<RequestDTO>>(result);

     return Ok(resultDTO);



}



    [HttpPost("getCustomRequests")]
    public async Task<IActionResult> GetCustomRequests(IFormCollection data){
        var result = await _requestService.GetCustomRequests();
        return Ok(result);
    }





    [HttpPost]
    [Route("/getUnapprovedRequests")]
    public async Task<IActionResult> GetUnapprovedRequests(IFormCollection data){
        var user  = JsonSerializer.Deserialize<User>(data["user"]);
        var result = await _requestService.GetUnapprovedRequests(user);
        return Ok(result);
    }











     

        


}




 

