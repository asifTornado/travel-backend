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
    }
}
