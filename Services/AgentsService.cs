using backEnd.Models;
using backEnd.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Org.BouncyCastle.Tls;
using backEnd.Services.IServices;
using backEnd.Factories.IFactories;
using Dapper;

namespace backEnd.Services;

public class AgentsService : IAgentsService
{

 
   private TravelContext _travelContext;
    private IConnection _connection;
    public AgentsService(TravelContext travelContext, IConnection connection)
    {
        _travelContext = travelContext;
        _connection = connection;
    }
  

 


 










    public async Task<Agent?> GetAsync(int id){

        var result  = await _travelContext.Agents.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
        return result;

        // await using var connection = _connection.GetConnection();

        // await connection.OpenAsync();

        // string sql = @"SELECT * FROM dbo.Agents WHERE Id = @Id";

        // var result = await connection.QueryFirstOrDefaultAsync<Agent>(sql, new {Id = id});

        // return result;
     
    
    }

    public async Task CreateAgent(Agent agent) {

        _travelContext.Entry(agent).State = EntityState.Added;

        await _travelContext.SaveChangesAsync();
        
        // await using var connection = _connection.GetConnection();
        // await connection.OpenAsync();
        // string sql =@"INSERT INTO dbo.Agents (Name, Email, Phone) VALUES (@Name, @Email, @Phone)";

        // await connection.ExecuteAsync(sql, agent);





   

   


        
    }


    public async Task<List<Agent>> GetAllAgents(){


        var results = _travelContext.Agents.AsNoTracking().ToList();

        return results;
    
        //  await using var connection = _connection.GetConnection();

        // await connection.OpenAsync();

        // string sql = @"SELECT * FROM dbo.Agents";

        // var result = await connection.QueryAsync<Agent>(sql);

        // return result.ToList();
    }


    public async Task<List<Agent>> GetAllProfessionalAgents(){
        
        var results = await _travelContext.Agents.AsNoTracking().Where(a => a.Professional == true).ToListAsync();
        return results;
    }



    public async Task UpdateAsync(int id, Agent agent){


        _travelContext.Entry(agent).State = EntityState.Modified;

        await _travelContext.SaveChangesAsync();
 
        // await using var connection = _connection.GetConnection();

        // connection.Open();

        // string sql = @"
        //  UPDATE dbo.Agents SET
        //  Name = @Name,
        //  Email = @Email,
        //  Phone = @Phone
        //  WHERE Id = @Id

        // ";

        // await connection.ExecuteAsync(sql, agent);

   


    }

    public async Task RemoveAsync(int id){

        var agent = new Agent{Id = id};

        _travelContext.Entry(agent).State = EntityState.Deleted;

        await _travelContext.SaveChangesAsync();
    
   

    }
    









}
