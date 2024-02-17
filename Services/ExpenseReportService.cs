using backEnd.Models;
using Microsoft.Data.SqlClient;
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
using ZstdSharp.Unsafe;

namespace backEnd.Services;

public class ExpenseReportService : IExpenseReportService
{

    
    private TravelContext _travelContext;



    public ExpenseReportService(TravelContext travelContext)
    {
        _travelContext = travelContext;

    }

    public async Task<ExpenseReport> GetExpenseReportFromRequest(int id)
    {
        var result = await _travelContext.ExpenseReports.AsNoTracking()
                            .Include(x => x.Expenses)
                            .Where( x => x.RequestId == id)
                            .FirstOrDefaultAsync();
        return result;
    }



    public async Task InsertExpenseReport(ExpenseReport expenseReport)
    {
        _travelContext.Entry(expenseReport).State = EntityState.Added;

        foreach(var expense in expenseReport.Expenses){
            _travelContext.Entry(expense).State = EntityState.Added;
        }

        await _travelContext.SaveChangesAsync();
    }



      public async Task UpdateExpenseReport(ExpenseReport expenseReport){
        var original = await _travelContext.ExpenseReports.AsNoTracking()
            .Include(x => x.Expenses)
            .Where(x => x.Id == expenseReport.Id).FirstOrDefaultAsync();
        
    _travelContext.Entry(expenseReport).State = EntityState.Modified;

    foreach(var expense in expenseReport.Expenses){
            var exist = original.Expenses.Any(x => x.Id == expense.Id);
            if (exist == true)
            {
                _travelContext.Entry(expense).State = EntityState.Modified
            }
            else
            {
                _travelContext.Entry(expense).State = EntityState.Added
                    .
            }
    }

    foreach(var expense in original.Expenses)
        {
            var exist = expenseReport.Expenses.Any(x => x.Id == expense.Id);
            
        }
    
    await _travelContext.SaveChangesAsync();
  }


  
  public async Task<List<ExpenseReport>> GetExpenseReportsForMe(User user){
    var result = await _travelContext.ExpenseReports.AsNoTracking()
      .Include(x => x.CurrentHandler)
                 .Where(x => x.CurrentHandlerId == user.Id)
                 .ToListAsync();
    return result;
  }


    public async Task<List<ExpenseReport>> GetMyExpenseReports(User user){
    var result = await _travelContext.ExpenseReports.AsNoTracking()
                 .Include(x => x.Request)
                 .ThenInclude(x => x.Requester)
                 .Include(x => x.CurrentHandler)
                 .Where(x => x.Request.RequesterId == user.Id)
                 .ToListAsync();
    return result;
  }

    public async Task<List<ExpenseReport>> GetExpenseReportsApprovedByMe(User user){
    var result = await _travelContext.ExpenseReports.AsNoTracking()
    .Include(x => x.CurrentHandler)
    .ToListAsync();
    var finalResult = result.Where(x => x.Approvals.Any(y => y.Id == user.Id)).ToList();
    return finalResult;
  }

    public async Task<List<ExpenseReport>> GetAllExpenseReports(){
    var result = await _travelContext.ExpenseReports.AsNoTracking()
                 .Include(x => x.CurrentHandler)
                 .ToListAsync();
    return result;
  }

  public async Task<ExpenseReport> GetExpenseReport(int id){
    var result = await _travelContext.ExpenseReports.AsNoTracking()
                 .Include(x => x.Expenses)
                 .Where(x => x.Id == id)
                 .FirstOrDefaultAsync();
    return result;
  }
}




