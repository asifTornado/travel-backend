
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

public class RequestAccountsController : ControllerBase
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

    private IIDCheckService _idCheckService;




   


    public RequestAccountsController(
    IIDCheckService iDCheckService,
    TripService tripService,   IBudgetsService budgetsService, 
    ILogService logService, IQuotationService quotationService, 
    TravelContext travelContext, IHelperClass helperClass, 
    IFileHandler fileHandler, IUsersService usersService, 
    IAgentsService agentsService, IMapper mapper, 
    IRequestService requestService, IMailer mailer, 
    INotifier notifier, IJwtTokenConverter jwtTokenConverter)
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
        _idCheckService = iDCheckService;
    }
    
    
    [HttpPost]
    [Route("requestForward")]
    public async Task<IActionResult> Forward(IFormCollection data){
        var request = JsonSerializer.Deserialize<Request>(data["request"]);
        var forwardedTo = JsonSerializer.Deserialize<User>(data["forwardedTo"]);

        var allowed = _idCheckService.CheckCurrent(request.Id, data["token"]);

        if(allowed == false){
            return Ok(false);
        }

        request.CurrentHandlerId = forwardedTo.Id;

        await _requestService.UpdateAsync(request);

        return Ok(true);
    }
   

}