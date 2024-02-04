using backEnd.Models;
using Microsoft.Data.SqlClient;
using backEnd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Org.BouncyCastle.Tls;
using System.Linq;
using backEnd.Services.IServices;
using backEnd.Factories.IFactories;
using Dapper;
using System.Globalization;
using backEnd.Helpers.IHelpers;

namespace backEnd.services;

public class IDCheckService : IIDCheckService
{

    
    private TravelContext _travelContext;
    private IJwtTokenConverter _jwtTokenConverter;
    private IUsersService _usersService;

    public IDCheckService(TravelContext travelContext, IJwtTokenConverter jwtTokenConverter, IUsersService usersService)
    {
        _travelContext = travelContext;
        _jwtTokenConverter = jwtTokenConverter;
        _usersService = usersService;

    }

    public async Task<Boolean> CheckAdmin(string token)
    {
        int? tokenId = _jwtTokenConverter.ParseToken(token);
        var user = await _usersService.GetOneUser(tokenId);
        if(user.UserType == "admin"){
            return true;
        }else{
            return false;
        }

    }

   

    public bool CheckDepartmentHead(Request request, string token)
    {
        int? tokenId = _jwtTokenConverter.ParseToken(token);

        if(tokenId == request.Requester.ZonalHeadId){
            return true;
        }else{
            return false;
        }
    }

  
    public bool CheckSupervisor(Request request, string token)
    {
        int? tokenId = _jwtTokenConverter.ParseToken(token);

        if(tokenId == request.Requester.SuperVisorId){
            return true;
        }else{
            return false;
        }
    }



    public bool CheckTraveler(Request request, string token)
    {
         int? tokenId = _jwtTokenConverter.ParseToken(token);

        if(tokenId == request.RequesterId){
            return true;
        }else{
            return false;
        }
    }


}




