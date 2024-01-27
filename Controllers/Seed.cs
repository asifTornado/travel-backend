
using Microsoft.AspNetCore.Mvc;
using backEnd.Models;

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


using Microsoft.AspNetCore.Authorization;








using MailKit;
using AutoMapper;
using backEnd.services;
using backEnd.Helpers;
using System.Security.AccessControl;
using backEnd.Services;
using backEnd.Services.IServices;
using backEnd.Helpers;
using Org.BouncyCastle.Asn1.X509;
using MimeKit.Encodings;
using Microsoft.VisualBasic;

namespace backEnd.Controllers;




[Route("/")]
[ApiController]

public class SeedController: ControllerBase{
    
    private ICounterService _counterService;
    public SeedController(ICounterService counterService){
        _counterService = counterService;
    }
    

    [HttpGet]
    [Route("seedUsers")]
    public async Task<IActionResult> SeedUsers(){
       await _counterService.SeedUsers();
       return Ok("Users Seeded");
    }

}