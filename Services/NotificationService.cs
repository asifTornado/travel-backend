using backEnd.Models;
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


namespace backEnd.Services;



public class NotificationService:INotificationService {

     private TravelContext _travelContext;

     private IConnection _connection;


    public NotificationService(TravelContext travelContext, IConnection connection)
    {
         _travelContext = travelContext;
         _connection = connection;
    }

    public async Task<List<Notification>> GetNotifications(){


        var results = await _travelContext.Notifications.AsNoTracking().ToListAsync();

        return results;

        // await using var connection = _connection.GetConnection();
        // await connection.OpenAsync();

        // var sql = "SELECT * FROM dbo.Notifications";

        // var result = await connection.QueryAsync<Notification>(sql);

        // return result.ToList();
     

    }

    public async Task<Notification> GetNotification(int id)
    {   

        var result = await _travelContext.Notifications.AsNoTracking().FirstOrDefaultAsync(n => n.Id == id);
        
        return result;
        // await using var connection = _connection.GetConnection();
        // await connection.OpenAsync();

        // var sql = "SELECT * FROM dbo.Notifications WHERE Id = @Id";

        // var result = await connection.QueryFirstOrDefaultAsync<Notification>(sql, new {Id = id});

        // return result;
    }

    public async Task RemoveNotification(int id){

        var newNotification = new Notification{Id = id};

        _travelContext.Entry(newNotification).State = EntityState.Deleted;

        await _travelContext.SaveChangesAsync();
      
    //   await using var connection = _connection.GetConnection();

    //   await connection.OpenAsync();

    //   var sql = "DELETE FROM dbo.Notifications WHERE Id = @Id";

    //   await connection.ExecuteAsync(sql, new {Id = id});

    }


    public async Task<IEnumerable<Notification>> GetNotificationsByUser(int id){

        var result = await _travelContext.Notifications.AsNoTracking().Where(n => n.To == id).ToListAsync();

        return result;

        // await using var connection = _connection.GetConnection();
        // await connection.OpenAsync();

        // var sql = "SELECT * FROM dbo.Notifications WHERE [To] = @Id";

        // var result = await connection.QueryAsync<Notification>(sql, new {Id = id});

        // return result;



    }

    public async Task InsertNotification(Notification notification){


        _travelContext.Entry(notification).State = EntityState.Added;

        await _travelContext.SaveChangesAsync();

      
        // await using var connection = _connection.GetConnection();
        
        // var sql = @"INSERT INTO dbo.Notifications (Time, Message, TicketId, [From], [To], Type, [Event])
        // Values(@Time, @Message, @TicketId, @From, @To, @Type, @Event)
        
        // ";

        // await connection.OpenAsync();

        // await connection.ExecuteAsync(sql, notification);


        

        // await _travelContext.SaveChangesAsync();


    }


    public async Task DeleteNotification(int? TicketId, int? To, string Event){


         await _travelContext.Database
    .ExecuteSqlRawAsync("DELETE FROM dbo.Notifications WHERE TicketId = {0} AND [To] = {1} AND [Event] = {2}", TicketId, To, Event);


    }


  
    

    
}



