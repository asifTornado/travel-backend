using System.Text.Json;

using Microsoft.AspNetCore.Mvc;
using backEnd.Models;

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
    
}

