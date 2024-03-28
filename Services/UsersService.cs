using System.Formats.Asn1;
using backEnd.Models;
using backEnd.Models.DTOs;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Org.BouncyCastle.Crypto.Operators;
using backEnd.Services.IServices;
using backEnd.Factories.IFactories;
using Dapper;
using Microsoft.Identity.Client;
using DnsClient.Protocol;

namespace backEnd.Services;

public class UsersService:IUsersService
{

 
    private TravelContext _travelContext;
    private IConnection _connection;
    public UsersService(TravelContext travelContext, IConnection connection)
    {
       _travelContext = travelContext;
         _connection = connection;
    }
    

    


    public async Task<List<User>> GetAsync(){


            
           var results = await _travelContext.Users.AsNoTracking()
           .Include(x => x.Roles)
           .ToListAsync();
           
           return results;
               
            }

    public async Task<User> GetAdmin(){
        var result = await _travelContext.Users.AsNoTracking()
                           .Where(x => x.UserType == "admin" )
                           .FirstOrDefaultAsync();

                    return result;
    }

    public async Task<List<User>> GetUsersForSupervisor(int id){
        var results = await _travelContext.Users.AsNoTracking()
                      .Where(x => x.SuperVisorId == id)
                      .ToListAsync();

        return results;
        
    }

    
    public async Task<User> GetAuditor(){

            var result = await _travelContext.Users.AsNoTracking().Where(u => u.UserType == "auditor").FirstOrDefaultAsync();
      
        return result;
       
    }

    public async Task<User> GetUserByMail(string mail)
    {   

          var result = await _travelContext.Users.AsNoTracking().Where(u => u.MailAddress == mail).FirstOrDefaultAsync();
      
        return result;
    
        
    }

        public async Task<User> GetUserByName(string name)
    {
            var result = await _travelContext.Users.AsNoTracking().Where(user => user.EmpName.Trim() == name.Trim())
           
            
            .FirstOrDefaultAsync();
      
        return result;
      
    }



       public async Task<List<User>> GetUsersByMail(List<string> mails)
    {

        var result = await _travelContext.Users.AsNoTracking().Where(user => mails.Contains(user.MailAddress)).ToListAsync();
      
        return result;
      
    }


    public async Task<User?> GetUserByMailAndPassword(string mail, string password)
    { 

        var result = await _travelContext.Users
               .AsNoTracking()
        .Include(User => User.SuperVisor)
       
        .Include(User => User.ZonalHead)
        .Include(User => User.FlyerNos)
 
        .Where(user => user.MailAddress == mail && user.Password == password)
        .FirstOrDefaultAsync();
      
        return result;

    }

    public async Task<List<User>> GetAllNormalUsers()
    {  

       var result = await _travelContext.Users.AsNoTracking().Where(user => user.UserType != "Admin" && user.UserType != "Master Admin").ToListAsync();
      
        return result;
      
    }


    public async Task<List<User>> GetUsersIncludingAdmin()
    {   
        var result = await _travelContext.Users.AsNoTracking().Where(user => user.UserType != "Master Admin" ).ToListAsync();
      
        return result;

        
    }

    public async Task<User?> GetOneUser(int? id){
     
        var result = await _travelContext.Users
           .AsNoTracking()
        .Include(user => user.SuperVisor)
       
        .Include(user => user.ZonalHead)
        .Include(user => user.FlyerNos)
        .Include(user => user.Roles)
     
        .Where(user => user.Id == id)
       
        .FirstOrDefaultAsync();
       

      
       

        return result;
    }



    public async Task CreateAsync(User newUser){


   

      
            

        _travelContext.Entry(newUser).State = EntityState.Added;
        await _travelContext.SaveChangesAsync();
     

      
     

    }

    public async Task UpdateAsync(int id, User user){

     _travelContext.ChangeTracker.Clear();

var original = await _travelContext.Users
    .AsNoTracking()
    .Include(user => user.FlyerNos)
    .Include(user => user.Roles)
    .FirstOrDefaultAsync(u => u.Id == id);

user.Roles?.ForEach(x => {

     var originalRoleNo = original.Roles.FirstOrDefault(y => y.Id == x.Id);
        if (originalRoleNo == null)
        {
            var newUserRole = new UserRoles(){UserId = user.Id, RoleId = x.Id};
            _travelContext.Entry(newUserRole).State = EntityState.Added;
        }

});


original.Roles?.ForEach(x => {
    var newRoleNo = user.Roles?.FirstOrDefault( y => y.Id == x.Id) ?? null;

    if(newRoleNo == null){
        var oldUserRole = new UserRoles(){UserId = user.Id, RoleId = x.Id};
        _travelContext.Entry(oldUserRole).State = EntityState.Deleted;
    }
});



user.FlyerNos.ForEach(x =>
{
    if (x.Id == 0 || x.Id == null)
    {
        _travelContext.Entry(x).State = EntityState.Added;
    }
    else
    {
        var originalFlyerNo = original.FlyerNos.FirstOrDefault(y => y.Id == x.Id);
        if (originalFlyerNo != null)
        {
            _travelContext.Entry(originalFlyerNo).CurrentValues.SetValues(x);
            _travelContext.Entry(originalFlyerNo).State = EntityState.Modified;
        }
        else
        {
            _travelContext.Entry(x).State = EntityState.Added;
        }
    }
});


_travelContext.Entry(user).State = EntityState.Modified;

await _travelContext.SaveChangesAsync();
           }
            

       


    //     await using SqlConnection connection = _connection.GetConnection();
    //     connection.Open();

    // string sql = @"
    //   UPDATE [dbo].[Users]
    //   SET
    //     EmpName = @EmpName,
    //     EmpCode = @EmpCode,
    //     Designation = @Designation,
    //     MailAddress = @MailAddress,
    //     Unit = @Unit,
    //     Section = @Section,
    //     Wing = @Wing,
    //     Team = @Team,
    //     Department = @Department,
    //     TeamType = @TeamType,
    //     Password = @Password,
    //     UserType = @UserType,
    //     Available = @Available,
    //     Rating  = @Rating,
   
    //     Extension = @Extension,
    //     MobileNo = @MobileNo,
    //     Location = @Location,
    //     Numbers = @Numbers,
    //     SuperVisorId = @SuperVisorId,
    //     ZonalHeadId = @ZonalHeadId,
    //     TravelHandlerId = @TravelHandlerId,
    //     PassportNo = @PassportNo,
    //     Preferences = @Preferences,
    //     HasFrequentFlyerNo = @HasFrequentFlyerNo
    //   WHERE Id = @Id";

    // await connection.ExecuteAsync(sql, user);
       
       

  public async Task UploadPreferenceImage(string filepath, int id){
    string sql = $"UPDATE dbo.Users SET PreferenceImage = {filepath} WHERE Id = {id}";

    await _travelContext.Database.ExecuteSqlRawAsync(sql);

  }


    

    public async Task UpdateUserNumber(User user)
    {


      

        user.Numbers = user.Numbers + 1;

        _travelContext.Entry(user).Property(u => u.Numbers).IsModified = true;

        await _travelContext.SaveChangesAsync();

        // await using var connection = _connection.GetConnection();
        // await connection.OpenAsync();
        // var sql = "UPDATE dbo.Users SET Numbers = @Numbers WHERE Id = @Id";
        // await connection.ExecuteAsync(sql, user);

  


      



         
      
    }

    public async Task RemoveAsync(int id){


        var user = new User{Id = id};

        _travelContext.Entry(user).State = EntityState.Deleted;

        await _travelContext.SaveChangesAsync();

         
        
        // await using var connection = _connection.GetConnection();
        // await connection.OpenAsync();
        // var sql = "DELETE FROM dbo.Users WHERE Id = @Id";
        // await connection.ExecuteAsync(sql, new {Id = id});
     


    }
        

    // public async Task<User?> GetOneUserByGroups(string group) =>
    //     await _user.Find(x => x.Groups.Contains(group)).FirstOrDefaultAsync();

    public async Task<List<string>> GetUserEmails(){


        var result = await _travelContext.Users.AsNoTracking().Select(x => x.MailAddress).ToListAsync();

        return result;

        // await using var connection = _connection.GetConnection();
        // await connection.OpenAsync();
        // var sql = "SELECT MailAddress FROM dbo.Users";
        // var result = await connection.QueryAsync<string>(sql);
        // return result.ToList()
   
    }



}





