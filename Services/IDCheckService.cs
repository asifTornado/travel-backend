using backEnd.Models;
using backEnd.Models.DTOs;
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

namespace backEnd.Services;

public class IDCheckService : IIDCheckService
{

    
    private TravelContext _travelContext;
    private IJwtTokenConverter _jwtTokenConverter;
    private IUsersService _usersService;
    private RoleService _roleService;

    public IDCheckService(RoleService roleService, TravelContext travelContext, IJwtTokenConverter jwtTokenConverter, IUsersService usersService)
    {
        _travelContext = travelContext;
        _jwtTokenConverter = jwtTokenConverter;
        _usersService = usersService;
        _roleService = roleService;

    }

    public async Task<bool> CheckAdmin(string token)
    {
        int? tokenId = _jwtTokenConverter.ParseToken(token);
        var user = await _usersService.GetOneUser(tokenId);
        if(user.UserType == "admin"){
            return true;
        }else{
            return false;
        }

    }

    public async Task<bool> CheckAdminOrManager(string token){
        int? tokenId = _jwtTokenConverter.ParseToken(token);
        var user = await _usersService.GetOneUser(tokenId);

        if(user.UserType == "admin" || user.UserType == "manager"){
            return true;
        }else{
            return false;
        }
        
    }

    public async Task<Return>  CheckAdminOrManagerAndReturn(string token){
        int? tokenId = _jwtTokenConverter.ParseToken(token);
        var user = await _usersService.GetOneUser(tokenId);

        if(user.UserType == "admin" || user.UserType == "manager"){
            return new Return{
                Valid=true,
                UserId=user.Id
            };
        }else{
            return new Return{
                Valid=false
            };
        }
    }

   

    public bool CheckDepartmentHead(Request request, string token)
    {
        int? tokenId = _jwtTokenConverter.ParseToken(token);
        var userType = _jwtTokenConverter.GetUserType(token);

        if(tokenId == request.Requester.ZonalHeadId || userType == "userType: admin"){
            return true;
        }else{
            return false;
        }
    }

  
    public bool CheckSupervisor(Request request, string token)
    {
        int? tokenId = _jwtTokenConverter.ParseToken(token);
        var userType = _jwtTokenConverter.GetUserType(token);

        if(tokenId == request.Requester.SuperVisorId || userType == "userType: admin"){
            return true;
        }else{
            return false;
        }
    }

     public Return CheckSupervisorAndReturn(Request request, string token)
    {
        int? tokenId = _jwtTokenConverter.ParseToken(token);
        var userType = _jwtTokenConverter.GetUserType(token);

        if(tokenId == request.Requester.SuperVisorId || userType == "userType: admin"){
            return new Return{
                Valid=true,
                UserId=tokenId
            };
        }else{
            return new Return{
                Valid=false,
                UserId=0
            };
        }
    }



    public bool CheckTraveler(Request request, string token)
    {
         int? tokenId = _jwtTokenConverter.ParseToken(token);

           var userType = _jwtTokenConverter.GetUserType(token);

        if(tokenId == request.RequesterId || userType == "userType: admin"){
            return true;
        }else{
            return false;
        }
    }


    public async Task<bool> CheckManager(Request request, string token){
        int? tokenId = _jwtTokenConverter.ParseToken(token);
        var manager = await _usersService.GetOneUser(tokenId);

          var userType = _jwtTokenConverter.GetUserType(token);


        if(manager.UserType == "manager" ||  userType == "userType: admin"){
            return true;
        }else{
            return false;
        }

    
        
    }


    public bool CheckCurrent(int? currentHandlerId, string token){

          var userType = _jwtTokenConverter.GetUserType(token);
         int? tokenId = _jwtTokenConverter.ParseToken(token);
         if(currentHandlerId == tokenId || userType == "userType: admin"){
            return true;
         }else{
            return false;
         }
    }


}




