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
using Dapper;
using System.Data;
using System.Linq;
using System;


namespace backEnd.Services;



public class LogService: ILogService
{

     

     private IConnection _connection;

     private TravelContext _context;

   public LogService(IConnection connection, TravelContext context)
   {
        _connection = connection;
        _context = context;
   }


    public async Task InsertLog(int? requestId, int? from, int? to, string Event){


        var date = DateTime.Now;

        var log = new Log{
            Date = date.ToString(),
            FromId = from,
            ToId = to,
            Event = Event,
            RequestId = requestId
        };

       
       _context.Entry(log).State = EntityState.Added;
      
       await _context.SaveChangesAsync();

    
    }



       public async Task InsertLogs(List<int> requestIds, int? from, int? to, string Event){

                                
        var date = DateTime.Now;


        foreach(var id in requestIds){

        var log = new Log{
            Date = date.ToString(),
            FromId = from,
            ToId = to,
            Event = Event,
            RequestId = id
        };
          
          _context.Entry(log).State = EntityState.Added;
        }
       
     
      
       await _context.SaveChangesAsync();

    
    }







   public async Task<List<LogDTO>> GetLogs(int id)
   {   
       

       var query = from logEntry in _context.Logs.AsNoTracking()
                   join fromEntry in _context.Users.AsNoTracking() on logEntry.FromId equals fromEntry.Id
                   join toEntry in _context.Users.AsNoTracking() on logEntry.ToId equals toEntry.Id
                   where logEntry.RequestId == id
                   select new LogDTO{
                       Date = logEntry.Date,
                       Event = logEntry.Event,
                       From = fromEntry.EmpName,
                       To = toEntry.EmpName
                   };
                 
            return await query.ToListAsync();
   }

    public async Task<List<LogDTO>> GetLogsForTrip(List<int> requestIds)
    {
        var query = from logEntry in _context.Logs.AsNoTracking()
                   join fromEntry in _context.Users.AsNoTracking() on logEntry.FromId equals fromEntry.Id
                   join toEntry in _context.Users.AsNoTracking() on logEntry.ToId equals toEntry.Id
                   where requestIds.Contains(logEntry.RequestId.Value)
                   select new LogDTO{
                       Date = logEntry.Date,
                       Event = logEntry.Event,
                       From = fromEntry.EmpName,
                       To = toEntry.EmpName
                   };
                 
            return await query.ToListAsync();
    }
}