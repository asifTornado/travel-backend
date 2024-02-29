using backEnd.Models;
using backEnd.Models.DTOs;
using backEnd.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Org.BouncyCastle.Tls;
using backEnd.Services.IServices;
using backEnd.Factories.IFactories;
using System.Runtime.CompilerServices;
using Microsoft.Data.SqlClient;
using backEnd.Models.DTOs;
using backEnd.Mappings;
using Dapper;
using System.Data;
using AutoMapper;
using System.Text.Json;
using ZstdSharp.Unsafe;
using Newtonsoft.Json.Converters;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Identity.Client;

namespace backEnd.Services;

public class RoleService
{
      
    private TravelContext _travelContext;

    private IConnection _connection;


    private IMapper _imapper;
 

    public RoleService(TravelContext travelContext, IConnection connection, IMapper mapper)
    {
       _travelContext = travelContext;
         _connection = connection;
            _imapper = mapper;
     
     
    }


    public async Task<User> GetAccountsReceiverForMoneyReceipt(){
        var result = await _travelContext.Users.Where(x => x.Roles.Any(y => y.Value == "Accounts Money Receipt")).FirstOrDefaultAsync();
        return result;
    }


    public async Task<User> GetAccountsReceiverForExpenseReport(){
        var result = await _travelContext.Users.Where(x => x.Roles.Any(y => y.Value == "Accounts Expense Report")).FirstOrDefaultAsync();
        return result;
    }
    


    public async Task<User> GetTravelManager(){
        var result = await _travelContext.Users.Where(x => x.Roles.Any(y => y.Value == "Travel Manager")).FirstOrDefaultAsync();
        return result;
    }

    public async Task<User> GetAuditor(){
        var result = await _travelContext.Users.Where(x => x.Roles.Any(y => y.Value == "Auditor")).FirstOrDefaultAsync();
        return result;
    }

    public async Task InsertRole(Role role){
          _travelContext.Entry(role).State = EntityState.Added;
          await _travelContext.SaveChangesAsync();
    }


     public async Task RemoveRole(Role role){
          _travelContext.Entry(role).State = EntityState.Deleted;
          await _travelContext.SaveChangesAsync();
    }


   
     public async Task UpdateRole(Role role){
          _travelContext.Entry(role).State = EntityState.Modified;
          await _travelContext.SaveChangesAsync();
    }


    public async Task<List<Role>> GetRoles(){
        var result = await _travelContext.Roles.ToListAsync();
        return result;
    }





}




