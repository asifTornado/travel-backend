
using backEnd.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using backEnd.Models.DTOs;

namespace backEnd.Services.IServices
{
    public interface IBudgetsService
    {
        Task<Budget?> GetAsync(int id);
        Task CreateBudget(Budget budget);
        Task<List<Budget>> GetAllBudgets();
        Task UpdateAsync(int id, Budget budget);
        Task RemoveAsync(int id);
        Task<JsonResult> SearchBudget(Dictionary<string, string> searchObject);
        Task<List<Budget>> GetAllInitiatedTrips();

        Task CreateCustomRequestBudget(Budget budget);

        Task<Budget> GetTrip(int id);

        Task<int> CreateBudgetId(Budget budget);
    }
}
