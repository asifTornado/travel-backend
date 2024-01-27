using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using backEnd.Models;

namespace backEnd.Services.IServices;




public interface IExpenseReportService
{
    Task InsertExpenseReport(ExpenseReport expenseReport);
    Task<ExpenseReport> GetExpenseReportFromRequest(int id);
}