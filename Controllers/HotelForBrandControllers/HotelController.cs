using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Mozilla;
using backEnd.Helpers;
using backEnd.Services;
using backEnd.Models;
using backEnd.Models.DTOs;
using System.Text.Json;
using backEnd.services;
using backEnd.Services.IServices;
using backEnd.Helpers.IHelpers;
using AutoMapper;

namespace backEnd.Controllers.HotelForBrandControllers
{
    [ApiController]
    [Route("/")]
    public class HotelController : Controller
    {

        private readonly IUserApi _userApi;
        private readonly IUsersService _usersService;
        private readonly  IJwtTokenConverter _jwtTokenConverter;

        private readonly IHotelForBrandService _hotelForBrandService;


        private IMapper _mapper;

        public HotelController(IMapper mapper, IHotelForBrandService hotelService, IUserApi userApi, IUsersService usersService, IJwtTokenConverter jwtTokenConverter)
        {
            _userApi = userApi;
            _usersService = usersService;
            _jwtTokenConverter = jwtTokenConverter;
            _hotelForBrandService = hotelService;
            _mapper = mapper;

        }


    

        [HttpPost]
        [Route("/deleteHotel")]
        public async Task<IActionResult> DeleteHotel(IFormCollection data){
            var hotel = JsonSerializer.Deserialize<Hotels>(data["hotel"]);
            await _hotelForBrandService.DeleteHotel(hotel);
            return Ok();


       

            
        }



    


    




    }

}

