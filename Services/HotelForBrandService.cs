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

namespace backEnd.services;

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




        var results = await _travelContext.HotelForBrands.AsNoTracking()
        .Include(x => x.Locations)
        .ThenInclude(x => x.Hotels)
        .ToListAsync();
        // var searchBrand = "%" + brand + "%";
        // var searchLocation = "%" + location + "%";
   
        // IQueryable<HotelForBrands> query = _travelContext.HotelForBrands;
         
      
         
        //  if(brand != ""){
        //     query = query.Where(x => x.Brand.ToLower().Trim().Contains(brand.ToLower().Trim()));
        //  }

        //  if(location != ""){
        //     query = query.Where(x => x.Locations.Any((y)=>y.LocationName.ToLower().Trim().Contains(location.ToLower().Trim()) ));
        //  }


        //  var intermediateResults = await query.SelectMany(x=> x.Locations.Select(y => new {
        //     Location = y.LocationName,
        //     Hotels = y.Hotels, 
        //     Brand = x.Brand
        //  })).ToListAsync();


        //  var results = intermediateResults.SelectMany( x => x.Hotels.Select( y => y.Rooms.Select(z => new{
        //     Brand = x.Brand,
        //     Location = x.Location,
        //     Hotel = y.HotelName,
        //     Type = z.Type,
        //     Average_rate = z.Average_rate,
        //     Actual_rate = z.Actual_rate
        //  }) )).ToList();


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



