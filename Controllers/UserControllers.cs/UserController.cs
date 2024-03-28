
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
using Amazon.Util.Internal;


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
    private IFileHandler _fileHandler;

   


    public UserController(IUserApi userapi, IFileHandler fileHandler, IAgentsService agentsService, IMapper mapper, IRequestService requestService, IMailer mailer, IUsersService usersService, IIDCheckService idCheckService)
    {
        _imapper = mapper;
        _requestService = requestService;
        _agentsService = agentsService;
        _mailer = mailer;
        _userService = usersService;
        _userApi = userapi;
        _idCheckService = idCheckService;
        _fileHandler = fileHandler;
 

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
    user.ZonalHeadId = user.ZonalHead.Id;
    user.SuperVisorId = user.SuperVisor.Id;
  
    await _userService.CreateAsync(user);
    return Ok(true);
}


[HttpPost]
[Route("preferenceImageUpload")]
public async  Task<IActionResult> ProfileUpload(IFormCollection  data){

      var fileSize = data.Files[0].Length;
      var id = int.Parse(data["id"]);

      if(fileSize > 100000000){

         return Ok(false);

      }else{



      var fileName = _fileHandler.GetUniqueFileName(data.Files[0].FileName);
      var  filePath =   await _fileHandler.SaveFile(fileName, data.Files[0]);
      
     

      return Ok(fileName);

      }

}







 







   





     

        


}




 

