using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Mozilla;
using backEnd.Helpers;
using backEnd.Helpers.IHelpers;
using backEnd.Services;
using backEnd.Models;
using System.Text.Json;
using backEnd.Services.IServices;
using backEnd.Models.DTOs;
using AutoMapper;

namespace backEnd.Controllers.AuthenticationControllers
{
    [ApiController]
    [Route("/")]
    public class AuthenticationController : Controller
    {

        private readonly IUserApi _userApi;
        private readonly IUsersService _usersService;
        private readonly IJwtTokenConverter _jwtTokenConverter;
        private IMapper _mapper;
        

        public AuthenticationController(IMapper mapper, IUserApi userApi, IUsersService usersService, IJwtTokenConverter jwtTokenConverter)
        {
            _userApi = userApi;
            _usersService = usersService;
            _jwtTokenConverter = jwtTokenConverter;
            _mapper = mapper;
        }



        [HttpGet]
        [Route("/api/getUsers")]
        public async Task<IActionResult> ApiUsers()
        {
            var results = await _userApi.GetUsers();

            return Ok(results);

        }

        [HttpPost]
        [Route("/authenticateToken")]
        public async Task<IActionResult> AuthenticateToken(IFormCollection data){
            var token = data["token"];
            var id =  _jwtTokenConverter.ParseToken(token);
            if(id != null){
                var user = await _usersService.GetOneUser(id);
                if(user != null){
                    var newData = new {
                          token = token,
                          success = true,
                          result = user
                    };

                return Ok(newData);
            }
            else
            {
                var newData = new { success = false, message = "This user is not authorized" };
                return Ok(newData);
            }
                }else{
                    var newData = new {success = false, message = "This token is not authorized"};
                    return Ok(newData);
                }
            }
            
        

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(IFormCollection data)
        {
            var result = await _usersService.GetUserByMail(data["email"]);

                if(result != null)
            {
                var newData = new
                {
                    registered = false,
                    message = "This email is already registered"
                };

                      return new JsonResult(newData);
            }
            else
            {


                var user = JsonSerializer.Deserialize<User>(data["user"]) ;
               
                user.UserType = "normal";
                user.Password = data["Password"];
              
                await _usersService.CreateAsync(user);
                return Ok(true);

            };

            
        }



        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(IFormCollection data)
        {
            var email = data["email"];
            var password = data["password"];

            Console.WriteLine("this is the email");
            Console.WriteLine(email);

            var result =await  _usersService.GetUserByMailAndPassword(email, password);

            if(result != null)
            {
                var token = _jwtTokenConverter.GenerateToken(result);
                

                var newData = new
                {
                   
                    token = token,
                    success = true,
                    result = result

                };

                return Ok(newData);
            }
            else
            {
                var newData = new { success = false, message = "This user is not authorized" };
                return Ok(newData);
            }
        }




    }
}
