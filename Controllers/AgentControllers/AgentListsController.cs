using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Mozilla;
using backEnd.Helpers;
using backEnd.Services;
using backEnd.Models;
using System.Text.Json;

using backEnd.Services.IServices;
using backEnd.Helpers.IHelpers;
using AutoMapper;

namespace backEnd.Controllers.AgnetControllers
{
    [ApiController]
    [Route("/")]
    public class AgentListsController : Controller
    {

        private readonly IUserApi _userApi;
        private readonly  IUsersService _usersService;
        private readonly  IJwtTokenConverter _jwtTokenConverter;

        private readonly IAgentsService _agentsService;

        private IMapper _mapper;

        public AgentListsController(IMapper mapper, IAgentsService agentsService, IUserApi userApi, IUsersService usersService, IJwtTokenConverter jwtTokenConverter)
        {
            _userApi = userApi;
            _usersService = usersService;
            _jwtTokenConverter = jwtTokenConverter;
            _agentsService = agentsService;
            _mapper = mapper;
        }



        [HttpGet]
        [Route("/getAgents")]
        public async Task<IActionResult> GetAgents()
        {
            var results = await _agentsService.GetAllAgents();

            return Ok(results);

        }


        [HttpGet]
        [Route("/getProfessionalAgents")]
        public async Task<IActionResult> GetProfessionalAgents()
        {
            var results = await _agentsService.GetAllProfessionalAgents();

            return Ok(results);

        }

      
     


    


       

            
        }



    




    }

