using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Mozilla;
using backEnd.Helpers;
using backEnd.Services;
using backEnd.Models;
using backEnd.Models.DTOs;
using System.Text.Json;
using backEnd.Services.IServices;
using backEnd.Helpers.IHelpers;
using AutoMapper;

namespace backEnd.Controllers.HotelForBrandControllers
{
    [ApiController]
    [Route("/")]
    public class HotelLocationController : Controller
    {

        private readonly IUserApi _userApi;
        private readonly IUsersService _usersService;
        private readonly  IJwtTokenConverter _jwtTokenConverter;

        private readonly IHotelForBrandService _hotelForBrandService;


        private IMapper _mapper;

        public HotelLocationController(IMapper mapper, IHotelForBrandService hotelService, IUserApi userApi, IUsersService usersService, IJwtTokenConverter jwtTokenConverter)
        {
            _userApi = userApi;
            _usersService = usersService;
            _jwtTokenConverter = jwtTokenConverter;
            _hotelForBrandService = hotelService;
            _mapper = mapper;

        }





        [HttpPost]
        [Route("/deleteLocation")]
        public async Task<IActionResult> DeleteHotelLocations(IFormCollection data){
            var hotelLocations = JsonSerializer.Deserialize<HotelLocations>(data["location"]);
            await _hotelForBrandService.DeleteHotelLocations(hotelLocations);
            return Ok();
        }



    




    }

}

