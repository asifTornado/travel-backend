
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

using backEnd.Models.DTOs;
using backEnd.Services.IServices;
using MailKit;
using AutoMapper;
using backEnd.Helpers;
using backEnd.Services;
using backEnd.Helpers.IHelpers;


namespace backEnd.Controllers.UserControllers;




[Route("/")]
[ApiController]

public class UserListsController : ControllerBase
{


    private IMapper _imapper;
    private IRequestService _requestService;

    private IAgentsService _agentsService;

    private IMailer _mailer;


    private IUsersService _userService;


    private IUserApi _userApi;
    private IIDCheckService _idCheckService;
   


    public UserListsController(IIDCheckService idCheckService, IUserApi userapi, IAgentsService agentsService, IMapper mapper, IRequestService requestService, IMailer mailer, IUsersService usersService)
    {
        _imapper = mapper;
        _requestService = requestService;
        _agentsService = agentsService;
        _mailer = mailer;
        _userService = usersService;
        _userApi = userapi;
        _idCheckService = idCheckService;
 

    }



 



   [HttpGet("getUsers")]
   public async Task<IActionResult> GetUsers(IFormCollection data){
      //    var allowed = await _idCheckService.CheckAdminOrManager(data["token"]);
      //    if(allowed == false) return Ok(false);
         var users = await _userService.GetAsync();
         Console.WriteLine("these are the users");
         Console.WriteLine(users);
         return Ok(users);
             

   }



   
   [HttpPost("getUsersForSupervisor")]
   public async Task<IActionResult> getUsersForSupervisor(IFormCollection data){
         var id = int.Parse(data["id"]);
         var users = await _userService.GetUsersForSupervisor(id);
         return Ok(users);
             

   }





   [HttpGet("getUserEmails")]
   public async Task<IActionResult> GetUserEmails(){
    var emails = await _userService.GetUserEmails();
    return Ok(emails);
   }





[HttpPost]
[Route("getApiUsers")]
public async Task<IActionResult> GetApiUsers(IFormCollection data){
      var results  = await _userApi.GetUsers();

      return Ok(results);
}




 







   





     

        


}




 

