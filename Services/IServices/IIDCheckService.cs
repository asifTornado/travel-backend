using System.Text.Json;

using Microsoft.AspNetCore.Mvc;
using backEnd.Models;

namespace backEnd.Services.IServices;




public interface IIDCheckService
{
    Boolean CheckSupervisor(Request request, string token);
    Boolean CheckTraveler(Request request, string token);
    Boolean CheckDepartmentHead(Request request, string token);
    Task<Boolean> CheckAdmin(string token);
    
}

