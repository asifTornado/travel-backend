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

namespace backEnd.services;

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
        var result = await _travelContext.Users.Where(x => x.Roles.Contains("Accounts Money Receipt")).FirstOrDefaultAsync();
        return result;
    }




}




