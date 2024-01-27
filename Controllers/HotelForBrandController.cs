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

namespace backEnd.Controllers
{
    [ApiController]
    [Route("/")]
    public class HotelForBrandController : Controller
    {

        private readonly IUserApi _userApi;
        private readonly IUsersService _usersService;
        private readonly  IJwtTokenConverter _jwtTokenConverter;

        private readonly IHotelForBrandService _hotelForBrandService;


        private IMapper _mapper;

        public HotelForBrandController(IMapper mapper, IHotelForBrandService hotelService, IUserApi userApi, IUsersService usersService, IJwtTokenConverter jwtTokenConverter)
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
        [Route("/getHotelsForBrand")]
        public async Task<IActionResult> GetHotelsForBrands(IFormCollection data)
        {
            var id = data["id"];
            var result = await _hotelForBrandService.GetHotelsForBrand(int.Parse(id));
            return Ok(result);

        }


        [HttpPost]
        [Route("/createHotelForBrand")]
        public async Task<IActionResult> CreateHotelsForBrand(IFormCollection data)
        { 
            var hotelForBrands = JsonSerializer.Deserialize<HotelForBrands>(data["hotelsForBrand"]);
            var hotelForBrandDTO  = _mapper.Map<HotelForBrandsDTO>(hotelForBrands);

            await _hotelForBrandService.InsertHotelsForBrand(hotelForBrands);

            return Ok(hotelForBrands);

        }



        [HttpPost]
        [Route("/editHotelForBrand")]
        public async Task<IActionResult> EditHotelsForBrand(IFormCollection data)
        { 
           var hotelForBrands = JsonSerializer.Deserialize<HotelForBrands>(data["hotelsForBrand"]);


           await _hotelForBrandService.EditHotelsForBrand(hotelForBrands);

           return Ok(hotelForBrands.Id);

        }



        
        [HttpPost]
        [Route("/deleteHotelsForBrand")]
        public async Task<IActionResult> DeleteHotelsForBrand(IFormCollection data)
        { 
          
              await _hotelForBrandService.DeleteHotelsForBrand(int.Parse(data["id"]));
              return Ok();

        }
        

        [HttpPost]
        [Route("/searchHotelsForBrands")]
        public async Task<IActionResult> SearchHotelsForBrands(IFormCollection data){
            var results = await _hotelForBrandService.SearchHotesForBrands(data["brand"], data["location"]);
            return Ok(results);
        }


        [HttpPost]
        [Route("/deleteHotel")]
        public async Task<IActionResult> DeleteHotel(IFormCollection data){
            var hotel = JsonSerializer.Deserialize<Hotels>(data["hotel"]);
            await _hotelForBrandService.DeleteHotel(hotel);
            return Ok();


       

            
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

