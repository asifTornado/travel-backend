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
        [Route("/updateBrand")]
        public async Task<IActionResult> UpdateBrand(IFormCollection data)
        { 
           var brand = JsonSerializer.Deserialize<HotelForBrands>(data["brand"]);


           await _hotelForBrandService.UpdateBrand(brand);

           return Ok();

        }


        // [HttpPost]
        // [Route("/deleteBrand")]
        // public async Task<IActionResult> DeleteBrand(IFormCollection data)
        // { 
        //    var brand = JsonSerializer.Deserialize<HotelForBrands>(data["brand"]);


        //    await _hotelForBrandService.DeleteBrand(brand);

        //    return Ok();

        // }





        
        [HttpPost]
        [Route("/deleteHotelsForBrand")]
        public async Task<IActionResult> DeleteHotelsForBrand(IFormCollection data)
        { 
          
              await _hotelForBrandService.DeleteHotelsForBrand(int.Parse(data["id"]));
              return Ok();

        }





          
        [HttpPost]
        [Route("/getHotelLocation")]
        public async Task<IActionResult> GetHotelLocation(IFormCollection data)
        { 
          
              var result = await _hotelForBrandService.GetHotelLocation(int.Parse(data["id"]));
              return Ok(result);

        }


        [HttpPost]
        [Route("/updateHotelLocation")]
        public async Task<IActionResult> UpdateHotelLocation(IFormCollection data)
        { 
              var location = JsonSerializer.Deserialize<HotelLocations>(data["location"]);
              await _hotelForBrandService.UpdateHotelLocation(location);
              return Ok();
        }


        [HttpPost]
        [Route("/createHotelLocation")]
        public async Task<IActionResult> CreateHotelLocation(IFormCollection data)
        { 
              var location = JsonSerializer.Deserialize<HotelLocations>(data["location"]);
              location.HotelForBrandsId = int.Parse(data["id"]);
              await _hotelForBrandService.CreateHotelLocation(location);
              return Ok();
        }


          [HttpPost]
        [Route("/deleteHotelLocation")]
        public async Task<IActionResult> DeleteHotelLocation(IFormCollection data)
        { 
              
              await _hotelForBrandService.DeleteHotelLocation(int.Parse(data["id"]));
              return Ok();
        }



        [HttpPost]
        [Route("/createHotel")]
        public async Task<IActionResult> CreateHotel(IFormCollection data)
        { 
              var hotel = JsonSerializer.Deserialize<Hotels>(data["hotel"]);
             
              await _hotelForBrandService.CreateHotel(hotel, int.Parse(data["id"]));
              return Ok();
        }


        [HttpPost]
        [Route("/getHotel")]
        public async Task<IActionResult> GetHotel(IFormCollection data)
        { 
              var result = await _hotelForBrandService.GetHotel(int.Parse(data["id"]));
              return Ok(result);
        }
        


        [HttpPost]
        [Route("/deleteHotel")]
        public async Task<IActionResult> DeleteHotel(IFormCollection data)
        { 
              await _hotelForBrandService.DeleteHotel(int.Parse(data["id"]));
              return Ok();
        }


        [HttpPost]
        [Route("/updateHotel")]
        public async Task<IActionResult> UpdateHotel(IFormCollection data)
        {     var hotel = JsonSerializer.Deserialize<Hotels>(data["hotel"]);
              await _hotelForBrandService.UpdateHotel(hotel);
              return Ok();
        }










    


    




    }

}

