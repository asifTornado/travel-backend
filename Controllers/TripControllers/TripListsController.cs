
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

public class TripListsController : ControllerBase
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

    
    public TripListsController(IJwtTokenConverter jwtTokenConverter, IIDCheckService idCheckService, IMailer mailer, ILogService logService, IUsersService usersService, IBudgetsService budgetsService, IMapper mapper, TripService tripService, IFileHandler fileHandler)
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


    [HttpPost("getAllTrips")]
    public async Task<IActionResult> GetAllTrips()
    {
        var results = await _budgetsService.GetAllInitiatedTrips();
        return Ok(results);
    }


    







   
}