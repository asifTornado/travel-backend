using backEnd.Models;
using backEnd.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Org.BouncyCastle.Tls;
using System.IO;
using backEnd.Services.IServices;
using backEnd.Factories.IFactories;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using System.Data;
using System.Linq;
using System;


namespace backEnd.Services;



public class ChartService: IChartService
{

     

     private IConnection _connection;

     private TravelContext _context;

   public ChartService(IConnection connection, TravelContext context)
   {
        _connection = connection;
        _context = context;
   }

   public async Task<JsonResult> GetTimeSeriesData(int timespan){
   var now = DateTime.Now;

    var result = await _context.Requests.AsNoTracking()

    .Select(r => new {
        Date = DateTime.Parse(r.Date),
        
    })
    .ToListAsync();
       
    var groupedData = result.GroupBy(r => r.Date).Select(g => new {
        Date = g.Key.ToString("dddd, MMMM dd, yyyy h:mm:ss tt"),
        Count = g.Count()
    }).ToList();


    return new JsonResult(groupedData);
               
   }


  






}