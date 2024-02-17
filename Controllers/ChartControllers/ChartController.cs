
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

namespace backEnd.Controllers.ChartControllers;




[Route("/")]
[ApiController]

public class ChartController : ControllerBase
{


  


   private IChartService _chartService;

   


    public ChartController(IChartService chartService)
    {
        _chartService = chartService;
    }
    
    
    [HttpPost]
    [Route("getRequestsByTime")]
    public async Task<IActionResult> GetRequestsByTime(IFormCollection data){

        var timeSpan = int.Parse(data["timespan"]);

        var result = await _chartService.GetTimeSeriesData(timeSpan);

        return Ok(result);






}


}