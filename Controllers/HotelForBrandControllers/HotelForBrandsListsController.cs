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

namespace backEnd.Controllers
{
    [ApiController]
    [Route("/")]
    public class HotelForBrandsListController : Controller
    {

        private readonly IUserApi _userApi;
        private readonly IUsersService _usersService;
        private readonly  IJwtTokenConverter _jwtTokenConverter;

        private readonly IHotelForBrandService _hotelForBrandService;


        private IMapper _mapper;

        public HotelForBrandsListController(IMapper mapper, IHotelForBrandService hotelService, IUserApi userApi, IUsersService usersService, IJwtTokenConverter jwtTokenConverter)
        {
            _userApi = userApi;
            _usersService = usersService;
            _jwtTokenConverter = jwtTokenConverter;
            _hotelForBrandService = hotelService;
            _mapper = mapper;

        }



        
        [HttpPost]
        [Route("/getHotelsForBrandsAll")]
        public async Task<IActionResult> GetHotelsForBrandsAll(IFormCollection data)
        {
            
            var result = await _hotelForBrandService.GetHotelsForBrandsAll();
            return Ok(result);

        }



        [HttpPost]
        [Route("/searchHotelsForBrands")]
        public async Task<IActionResult> SearchHotelsForBrands(IFormCollection data){
            var results = await _hotelForBrandService.SearchHotesForBrands(data["brand"], data["location"]);
            return Ok(results);
        }


      




    }

}

