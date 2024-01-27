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

namespace backEnd.services;

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
}




