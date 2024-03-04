
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

public class UserController : ControllerBase
{


    private IMapper _imapper;
    private IRequestService _requestService;

    private IAgentsService _agentsService;

    private IMailer _mailer;


    private IUsersService _userService;


    private IUserApi _userApi;

    private IIDCheckService _idCheckService;

   


    public UserController(IUserApi userapi, IAgentsService agentsService, IMapper mapper, IRequestService requestService, IMailer mailer, IUsersService usersService, IIDCheckService idCheckService)
    {
        _imapper = mapper;
        _requestService = requestService;
        _agentsService = agentsService;
        _mailer = mailer;
        _userService = usersService;
        _userApi = userapi;
        _idCheckService = idCheckService;
 

    }



    [HttpPost("getUser")]
   public async Task<IActionResult> GetUser(IFormCollection data){
         
         var user = await _userService.GetOneUser(int.Parse(data["id"]));
         return Ok(user);
             

   }









 


   
   [HttpPost("deleteUser")]
   public async Task<IActionResult> DeleteUser(IFormCollection data){

         var allowed = await _idCheckService.CheckAdminOrManager(data["token"]);
         if(allowed == false){
            return Ok(false);
         }
         Console.WriteLine("this is teh id");
         Console.WriteLine(data["id"]);
         Console.WriteLine("This is the id after parsing");
         var id = int.Parse(data["id"]);
         Console.WriteLine(id);
         await _userService.RemoveAsync(int.Parse(data["id"]));

         return Ok();
             

   }


[HttpPost]
[Route("updateUserNormal")]
public async Task<IActionResult> UpdateUserNormal(IFormCollection data)
{   
        var allowed = await _idCheckService.CheckAdminOrManager(data["token"]);
         if(allowed == false){
            return Ok(false);
         }
    var user = JsonSerializer.Deserialize<User>(data["user"]);
    user.SuperVisorId = user.SuperVisor?.Id;
    user.TravelHandlerId = user.TravelHandler?.Id;
    user.ZonalHeadId = user.ZonalHead?.Id;
    // user.SuperVisor.SuperVised = new List<User>();
    // user.TravelHandler.TravelHandled = new List<User>();
    // user.ZonalHead.Head = new List<User>();
    // var userDTO = _imapper.Map<UserDTO>(user);
    await _userService.UpdateAsync(user.Id, user);
    return Ok(true);
}



[HttpPost]
[Route("insertUser")]
public async Task<IActionResult> InsertUser(IFormCollection data)
{
        var allowed = await _idCheckService.CheckAdminOrManager(data["token"]);
         if(allowed == false){
            return Ok(false);
         }

    var user = JsonSerializer.Deserialize<User>(data["user"]);
    // var userDTO = _imapper.Map<UserDTO>(user);
    var userString = JsonSerializer.Serialize(user);
    await _userService.CreateAsync(user);
    return Ok(true);
}







 







   





     

        


}




 

