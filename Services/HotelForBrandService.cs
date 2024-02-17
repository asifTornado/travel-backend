using backEnd.Models;
using backEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Data.SqlClient;

using MimeKit.Encodings;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Org.BouncyCastle.Asn1.Mozilla;
using Org.BouncyCastle.Tls;
using System.Linq;
using backEnd.Factories.IFactories;
using backEnd.Services.IServices;
using Dapper;
using models.DTOs;

namespace backEnd.Services;

public class HotelForBrandService: IHotelForBrandService
{


     private TravelContext _travelContext;
     private IConnection _connection;

    public HotelForBrandService(TravelContext travelContext, IConnection connection )
    {
        
        _travelContext = travelContext;
        _connection = connection;
          
    }


 

    public async Task InsertHotelsForBrand(HotelForBrands hotelForBrands){


 

      
         _travelContext.HotelForBrands.Add(hotelForBrands);

         await _travelContext.SaveChangesAsync();
    }


     public async Task<HotelForBrands> GetHotelsForBrand(int id){

   
        var result = await _travelContext.HotelForBrands
        .Include(x => x.Locations).ThenInclude(y => y.Hotels)
        .AsNoTracking()
        .Where(h => h.Id == id).FirstOrDefaultAsync();
        return result;
    }



    public async Task<List<HotelForBrands>> GetHotelsForBrandsAll(){
       
        var result = await _travelContext.HotelForBrands.AsNoTracking().ToListAsync();
        return result;
    }


    public async Task EditHotelsForBrand(HotelForBrands hotelForBrands){

        var original = await _travelContext.HotelForBrands
        .Include(x => x.Locations)
        .ThenInclude(y => y.Hotels)
        .AsNoTracking().FirstOrDefaultAsync(h => h.Id == hotelForBrands.Id);


          _travelContext.HotelForBrands.Update(hotelForBrands);
        
        foreach(var location in original.Locations){

            var locationToRemove = hotelForBrands.Locations.FirstOrDefault(l => l.Id == location.Id);

            if(locationToRemove == null){
                _travelContext.Entry(location).State = EntityState.Deleted;
            }

           if(locationToRemove != null){

            foreach(var hotel in location.Hotels){
                  var locationFront = hotelForBrands.Locations.FirstOrDefault(l => l.Id == location.Id);
                  var hotelToRemove = locationFront.Hotels.FirstOrDefault(h => h.Id == hotel.Id);

                  if(hotelToRemove == null){
                      _travelContext.Entry(hotel).State = EntityState.Deleted;
                  }
            }


           }
  }

      
       

       
      

        await _travelContext.SaveChangesAsync();
    }


    public async Task DeleteHotelLocations(HotelLocations hotelLocations){
        _travelContext.HotelLocations.Remove(hotelLocations);
        await _travelContext.SaveChangesAsync();
    }


    public async Task DeleteHotel(Hotels hotel){
        _travelContext.Hotels.Remove(hotel);
        await _travelContext.SaveChangesAsync();
    }


    public async Task DeleteHotelsForBrand(int id){



        var result = await _travelContext.HotelForBrands
                            .Include(h => h.Locations).ThenInclude(h => h.Hotels)
                            .AsNoTracking()
                            .Where(h => h.Id == id).FirstOrDefaultAsync();


           if(result != null){
           _travelContext.HotelForBrands.Remove(result);
           }

           await _travelContext.SaveChangesAsync();



    }


    public async Task<JsonResult> SearchHotesForBrands(string brand, string location){




        // var results = await _travelContext.HotelForBrands.AsNoTracking()
        // .Include(x => x.Locations)
        // .ThenInclude(x => x.Hotels)
        // .ToListAsync();
        // var searchBrand = "%" + brand + "%";
        // var searchLocation = "%" + location + "%";
   
        IQueryable<HotelForBrands> query = _travelContext.HotelForBrands;
         
         var searchBrand = brand.ToLower().Trim();
         var searchLocation = location.ToLower().Trim();
         
         if(brand != ""){
            query = query.Where(x => x.Brand.ToLower().Trim().Contains(searchBrand));
         }

         if(location != ""){
            query = query.Where(x => x.Locations.Any((y)=>y.LocationName.ToLower().Trim().Contains(searchLocation) ));
         }


         var intermediateResults = await query.SelectMany(x=> x.Locations.SelectMany(y => y.Hotels.Select( 
            z => new{
            Location = z.HotelAddress,
            Hotel = z.HotelName, 
            Brand = x.Brand,
            Rooms = z.Rooms
         }
         ))).ToListAsync();
        

        var results = new List<SearchVM>();
        
        foreach(var result in intermediateResults){
            if(result.Rooms != null){
                foreach(var room in result.Rooms){
                    var newSearchItem = new SearchVM(){
                        Location=result.Location,
                        Hotel=result.Hotel,
                        Brand = result.Brand,
                        Type = room.Type,
                        Average_rate = room.Average_rate
                    };

                    results.Add(newSearchItem);
                }
            }else{
                var newSearchItem = new SearchVM(){
                    Location = result.Location,
                    Hotel = result.Hotel,
                    Brand = result.Brand,
                    Type = string.Empty,
                    Average_rate = string.Empty,
                };
                results.Add(newSearchItem);
            }
        }


        return new JsonResult(results);

        // await using SqlConnection connection = _connection.GetConnection();

        // var sql =  @"SELECT B.Brand, L.LocationName AS Location, 
        //              H.HotelName AS Hotel, 
        //              H.Rooms as Rooms, 
        //              H.Average_rate,
        //              H.Actual_rate 
        //              FROM dbo.HotelForBrands B 
        //              INNER JOIN dbo.HotelLocations L ON B.Id = L.HotelForBrandsId
        //              INNER JOIN dbo.Hotels H ON L.Id = H.HotelLocationsId
        //              WHERE b.Brand LIKE @Brand OR L.LocationName LIKE @Location";
          

          
        //  var results = await connection.QueryAsync<SearchDTO>(sql, new {Brand = searchBrand.ToLower().Trim(), Location = searchLocation.ToLower().Trim()});
         
         
        // return new JsonResult(results);

    }
 


  








}



