
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

namespace backEnd.Controllers.RoleControllers;




[Route("/")]
[ApiController]

public class RoleController : ControllerBase
{


   private RoleService _roleService;
   private IIDCheckService _idCheckService;




   


    public RoleController(RoleService roleService, IIDCheckService idCheckService)
    {
        _roleService = roleService;
        _idCheckService = idCheckService;
      
    }

    
    [HttpPost]
    [Route("insertRole")]
    public async Task<IActionResult> InsertRole(IFormCollection data){

           var allowed = await _idCheckService.CheckAdmin(data["token"]);
           if(allowed == false){
            return Ok(false);
           }
           var role = JsonSerializer.Deserialize<Role>(data["role"]);
           await _roleService.InsertRole(role);
           return Ok(role);
    }
     

     
    [HttpPost]
    [Route("updateRole")]
     public async Task<IActionResult> UpdateRole(IFormCollection data){
          var allowed = await _idCheckService.CheckAdmin(data["token"]);
           if(allowed == false){
            return Ok(false);
           }
           var role = JsonSerializer.Deserialize<Role>(data["role"]);
           await _roleService.UpdateRole(role);
           return Ok(role);
    }
     

     
[HttpPost]
[Route("removeRole")]
     public async Task<IActionResult> RemoveRole(IFormCollection data){
          var allowed = await _idCheckService.CheckAdmin(data["token"]);
           if(allowed == false){
            return Ok(false);
           }
           var role = JsonSerializer.Deserialize<Role>(data["role"]);
           await _roleService.RemoveRole(role);
           return Ok(role);
    }




[HttpPost]
[Route("getRoles")]
      public async Task<IActionResult> GetRoles(IFormCollection data){
           
           var result = await _roleService.GetRoles();
           return Ok(result);
    }



}




 

