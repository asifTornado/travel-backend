
using Microsoft.AspNetCore.Mvc;
using backEnd.Models;
using backEnd.Models.DTOs;
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

namespace backEnd.Controllers;




[Route("/")]
[ApiController]

public class LogsListController : ControllerBase
{

    private ILogService _logService;

    public LogsListController( ILogService logService)
    {
        _logService = logService;
    }
    
        
    


    [HttpPost]
    [Route("/getLogs")]
    public async Task<IActionResult> GetLogs(IFormCollection data){

        var id = data["id"];
        var result = await _logService.GetLogs(int.Parse(id));
        return Ok(result);

    }



    [HttpPost]
    [Route("/getLogsForTrip")]
    public async Task<IActionResult> GetLogsForTrip(IFormCollection data){

        var requestIds = JsonSerializer.Deserialize<List<int>>(data["requestIds"]);
        var result = await _logService.GetLogsForTrip(requestIds);
        return Ok(result);

    }




}