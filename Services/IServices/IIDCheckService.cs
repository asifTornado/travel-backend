using System.Text.Json;

using Microsoft.AspNetCore.Mvc;
using backEnd.Models;
using backEnd.Models.DTOs;

namespace backEnd.Services.IServices;




public interface IIDCheckService
{
    bool CheckSupervisor(Request request, string token);
    bool CheckTraveler(Request request, string token);
    bool CheckDepartmentHead(Request request, string token);
    Task<bool> CheckAdmin(string token);
    Task<bool> CheckManager(Request request, string token);
    Task<bool> CheckAdminOrManager(string token);
    bool CheckCurrent(int? currentHandlerId, string token);
    Task<Return>  CheckAdminOrManagerAndReturn(string token);
    Return CheckSupervisorAndReturn(Request request, string token);
  
    
}

