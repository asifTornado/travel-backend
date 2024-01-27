using backEnd.Models;
using backEnd.Models.DTOs;
using AutoMapper;

namespace backEnd.Mappings;
public class ApplicationProfile : Profile
{
    public ApplicationProfile()
    {
        CreateMap<User, UserDTO>().ReverseMap();
        CreateMap<Quotation, QuotationDTO>().ReverseMap();
        CreateMap<RequestDTO, Request>().ReverseMap()
        .ForMember(dest => dest.CurrentHandlerName, opt => opt.MapFrom(src => src.CurrentHandler.EmpName));
        CreateMap<HotelQuotation, QuotationDTO>().ReverseMap();
        CreateMap<Hotels, HotelsDTO>().ReverseMap();
        CreateMap<HotelLocations, HotelLocationsDTO>().ReverseMap();
        CreateMap<HotelForBrands, HotelForBrandsDTO>().ReverseMap();
        CreateMap<Budget, TripDTO>().ReverseMap();
       
        // Add more mappings as needed
    }
}