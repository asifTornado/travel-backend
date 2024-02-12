
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

namespace backEnd.Controllers.RequestControllers;




[Route("/")]
[ApiController]

public class ApprovalController : ControllerBase
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
    private BudgetsService _budgetService;
    private ILogService _logService;
    private TripService _tripService;
    private IJwtTokenConverter _jwtTokenConverter;

    private IIDCheckService _idCheckService;




   


    public ApprovalController(
    IIDCheckService iDCheckService,
    TripService tripService,   BudgetsService budgetsService, 
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
    [Route("/permanentlyReject")]
    public async Task<IActionResult> PermanentlyReject(IFormCollection data){
        var token = data["token"];
        var request = JsonSerializer.Deserialize<Request>(data["request"]);
        
        var allowed = _idCheckService.CheckSupervisor(request, token);
        if(allowed != true){
            return Ok(false);
        }

        request.PermanentlyRejected = true;
        request.Status = "Request Permanently Rejected By Supervisor";
        await _requestService.UpdateRequestForApproval(request);
        var message = $"Your trip numbered {request.BudgetId} has been permanently rejected by your supervisor";
        _notifier.InsertNotification(message, request.Requester?.SuperVisorId, request.RequesterId, request.Id, Events.PermanentlyRejected);
        token = _jwtTokenConverter.GenerateToken(request.Requester!);
        _mailer.PermanentlyRejected(request, token);
        _logService.InsertLog(request.Id, request.Requester.SuperVisorId, request.RequesterId, Events.PermanentlyRejected);
        return Ok(request);
    }

    [HttpPost]
    [Route("/reject")]
    public async Task<IActionResult> Reject(IFormCollection data){
        var request = JsonSerializer.Deserialize<Request>(data["request"]);
        var token = data["token"];
        var allowed = _idCheckService.CheckSupervisor(request, token);
        if(allowed != true){
            return Ok(false);
        }
        request.Status = "Seeking Trip Details Rectification";
        request.CurrentHandlerId = request.RequesterId;
        await _requestService.UpdateRequestForApproval(request);
        var message = $"Your trip numbered {request.BudgetId} has been rejected";
        _notifier.InsertNotification(message, request.Requester.SuperVisorId, request.RequesterId, request.Id, Events.TripRejected);
        token = _jwtTokenConverter.GenerateToken(request.Requester);
        _mailer.SeekRectification(request, token);
        _logService.InsertLog(request.Id, request.Requester.SuperVisorId, request.RequesterId, Events.TripRejected);
        return Ok(request);
    }

    
    [HttpPost]
    [Route("/approve")]
    public async Task<IActionResult> Approve(IFormCollection data){
        var request = JsonSerializer.Deserialize<Request>(data["request"]);
        var token = data["token"];
        var allowed = _idCheckService.CheckSupervisor(request, token);
        if(allowed != true){
            return Ok(false);
        }
        if(request.Custom == true){
        request.Status = "Seeking Approval From Department Head";
        request.CurrentHandlerId = request.Requester.ZonalHeadId;
        }else{
            request.Status = "Seeking Ticket Quotations";
            request.CurrentHandler = null;
        }
        request.SupervisorApproved = true;
      
        await _requestService.UpdateRequestForApproval(request);
       
        var message2 = $"The trip numbered {request.BudgetId} for the traveler {request.Requester.EmpName} has been approved by his/her supervisor";
        token = _jwtTokenConverter.GenerateToken(request.Requester);

        if(request.Custom == false){
        var message = $"Your trip numbered {request.BudgetId} has been approved by your supervisor";
        _notifier.InsertNotification(message, request.Requester.SuperVisorId, request.RequesterId, request.Id, Events.SupervisorApprovalTrip);
        _mailer.RequestApproved(request, token);
        _logService.InsertLog(request.Id, request.Requester.SuperVisorId, request.RequesterId, Events.SupervisorApprovalTrip);
        }else{
            
        }
        return Ok(request);
    }

    

    [HttpPost]
    [Route("/giveInfo")]
    public async Task<IActionResult> GiveInfo(IFormCollection data){
        var request = JsonSerializer.Deserialize<Request>(data["request"]);
        var token = data["token"];
        var allowed = _idCheckService.CheckTraveler(request, token);
        if(allowed != true){
            return Ok(false);
        }
        request.Status = "Seeking Supervisor Approval For Trip";
        var message = $"{request.Requester.EmpName} is seeking approval for a new trip";
        _notifier.InsertNotification(message, request.Requester.Id, request.Requester.SuperVisorId, request.Id, Events.SupervisorApprovalTrip);
        token = _jwtTokenConverter.GenerateToken(request.Requester.SuperVisor);
        _mailer.SeekSupervisorApprovalTrip(request, token);
        request.CurrentHandlerId = request.Requester.SuperVisorId;
        await _requestService.UpdateRequestForApproval(request);
        _logService.InsertLog(request.Id, request.RequesterId, request.Requester.SuperVisorId, Events.GiveInfo);
        return Ok(request);
    }



    [HttpPost]
    [Route("/departmentHeadPermanentlyRejectTrip")]
    public async Task<IActionResult> DepartmentHeadPermanentlyRejectTrip(IFormCollection data){
        var request = JsonSerializer.Deserialize<Request>(data["request"]);
        var token = data["token"];
        var allowed = _idCheckService.CheckDepartmentHead(request, token);
        if(allowed != true){
            return Ok(false);
        }
        request.PermanentlyRejected = true;
        request.Status = "Request Permanently Rejected By Department Head";
        await _requestService.UpdateRequestForApproval(request);
        var message = $"Your trip numbered {request.BudgetId} has been permanently rejected by your department head";
        _notifier.InsertNotification(message, request.Requester?.ZonalHeadId, request.RequesterId, request.Id, Events.DepartmentHeadPermanentlyReject);
         token = _jwtTokenConverter.GenerateToken(request.Requester!);
        _mailer.DepartmentHeadPermanentlyRejected(request, token);
        _logService.InsertLog(request.Id, request.Requester.ZonalHeadId, request.RequesterId, Events.DepartmentHeadPermanentlyReject);
        return Ok(request);
    }

    [HttpPost]
    [Route("/departmentHeadRejectTrip")]
    public async Task<IActionResult> DepartmentHeadRejectTrip(IFormCollection data){
        var request = JsonSerializer.Deserialize<Request>(data["request"]);
          var token = data["token"];
        var allowed = _idCheckService.CheckDepartmentHead(request, token);
        if(allowed != true){
            return Ok(false);
        }
        request.Status = "Seeking Trip Details Rectification";
        request.CurrentHandlerId = request.RequesterId;
        request.SupervisorApproved = false;
        await _requestService.UpdateRequestForApproval(request);
        var message = $"The trip numbered {request.BudgetId} that you approved has been rejected by your department head";
        _notifier.InsertNotification(message, request.Requester.ZonalHeadId, request.Requester.SuperVisorId, request.Id, Events.DepartmentHeadReject);
        token = _jwtTokenConverter.GenerateToken(request.Requester);
        _mailer.DepartmentHeadRejected(request, token);
        _logService.InsertLog(request.Id, request.Requester.ZonalHeadId, request.Requester.SuperVisorId, Events.DepartmentHeadReject);
        return Ok(request);
    }



        
    [HttpPost]
    [Route("/departmentHeadApproveTrip")]
    public async Task<IActionResult> DepartmentHeadApproveTrip(IFormCollection data){
        var request = JsonSerializer.Deserialize<Request>(data["request"]);
          var token = data["token"];
        var allowed = _idCheckService.CheckDepartmentHead(request, token);
        if(allowed != true){
            return Ok(false);
        }
        request.Status = "Seeking Quotations";
        request.CurrentHandlerId = null;
        request.SupervisorApproved = true;
        request.DepartmentHeadApproved = true;
      
        await _requestService.UpdateRequestForApproval(request);
        var message = $"Your trip numbered {request.BudgetId} has been approved by your department head";
        _notifier.InsertNotification(message, request.Requester.ZonalHeadId, request.RequesterId, request.Id, Events.DepartmentHeadApprove);
        var message2 = $"The trip numbered {request.BudgetId} for the traveler {request.Requester.EmpName} has been approved by his/her department head";
         token = _jwtTokenConverter.GenerateToken(request.Requester);
        _mailer.DepartmentHeadApproved(request, token);
        _logService.InsertLog(request.Id, request.Requester.ZonalHeadId, request.RequesterId, Events.DepartmentHeadPermanentlyReject);
        return Ok(request);
    }

}