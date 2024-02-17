using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using backEnd.Models;

namespace backEnd.Services.IServices;




public interface IExpenseReportService
{
    Task InsertExpenseReport(ExpenseReport expenseReport);
    Task<ExpenseReport> GetExpenseReportFromRequest(int id);
    Task UpdateExpenseReport(ExpenseReport expenseReport);
    Task<List<ExpenseReport>> GetExpenseReportsForMe(User user);

    Task<List<ExpenseReport>> GetMyExpenseReports(User user);

    Task<List<ExpenseReport>> GetExpenseReportsApprovedByMe(User user);

    Task<List<ExpenseReport>> GetAllExpenseReports();

    Task<ExpenseReport> GetExpenseReport(int id);
    
}