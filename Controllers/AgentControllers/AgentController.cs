using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Mozilla;
using backEnd.Helpers;
using backEnd.Services;
using backEnd.Models;
using System.Text.Json;
using backEnd.services;
using backEnd.Services.IServices;
using backEnd.Helpers.IHelpers;
using AutoMapper;

namespace backEnd.Controllers.AgnetControllers
{
    [ApiController]
    [Route("/")]
    public class AgentController : Controller
    {

        private readonly IUserApi _userApi;
        private readonly  IUsersService _usersService;
        private readonly  IJwtTokenConverter _jwtTokenConverter;

        private readonly IAgentsService _agentsService;

        private IMapper _mapper;

        public AgentController(IMapper mapper, IAgentsService agentsService, IUserApi userApi, IUsersService usersService, IJwtTokenConverter jwtTokenConverter)
        {
            _userApi = userApi;
            _usersService = usersService;
            _jwtTokenConverter = jwtTokenConverter;
            _agentsService = agentsService;
            _mapper = mapper;
        }



      

        [HttpPost]
        [Route("/createAgent")]
        public async Task<IActionResult> CreateAgent(IFormCollection data)
        { 
            var newAgent = JsonSerializer.Deserialize<Agent>(data["agent"]);
            await _agentsService.CreateAgent(newAgent);
            return Ok(newAgent);

        }



        [HttpPost]
        [Route("/editAgent")]
        public async Task<IActionResult> EditAgent(IFormCollection data)
        { 
            var newAgent = JsonSerializer.Deserialize<Agent>(data["agent"]);
            await _agentsService.UpdateAsync(newAgent.Id, newAgent);
            return Ok(newAgent);

        }



        
        [HttpPost]
        [Route("/deleteAgent")]
        public async Task<IActionResult> DeleteAgent(IFormCollection data)
        { 
            var id = data["id"];
            await _agentsService.RemoveAsync(int.Parse(id));
            return Ok(true);

        }


     


    


       

            
        }



    




    }

