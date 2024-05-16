using backEnd.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backEnd.Services.IServices
{
    public interface IHotelForBrandService
    {
        Task InsertHotelsForBrand(HotelForBrands hotelForBrands);
        Task<HotelForBrands> GetHotelsForBrand(int id);
        Task<List<HotelForBrands>> GetHotelsForBrandsAll();
        Task EditHotelsForBrand(HotelForBrands hotelForBrands);
        Task DeleteHotelsForBrand(int id);
        Task<JsonResult> SearchHotesForBrands(string brand, string location);

        Task DeleteHotel(Hotels hotel);

        Task DeleteHotelLocations(HotelLocations hotelLocations);

        Task UpdateBrand(HotelForBrands hotelForBrands);

        Task DeleteBrand(HotelForBrands hotelForBrands);

        Task<List<HotelLocations>> GetHotelLocations(int id);

        Task DeleteHotelLocation(int id);

        Task<HotelLocations> GetHotelLocation(int id);

        Task UpdateHotelLocation(HotelLocations hotelLocation);

        Task CreateHotelLocation(HotelLocations hotelLocation);

        Task <List<Hotels>> GetHotels(int id);

        Task <Hotels> GetHotel(int id);

        Task DeleteHotel(int id);

        Task UpdateHotel(Hotels hotels);

        Task CreateHotel(Hotels hotels);
    }
}
