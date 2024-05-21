using backEnd.Models;
using backEnd.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Org.BouncyCastle.Tls;
using backEnd.Services.IServices;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backEnd.Factories.IFactories;
using System.Runtime.CompilerServices;
using Dapper;
using backEnd.Models.DTOs;

namespace backEnd.Services
{
    public class ReportService
    {
        private TravelContext _travelContext;

        private IConnection _connection;

        public ReportService(TravelContext travelContext, IConnection connection)
        {
            _travelContext = travelContext;
            _connection = connection;
        }



        public async Task<List<Budget>> GetReports(){
            var result = await _travelContext.Budgets.AsNoTracking().AsSplitQuery()
            .Include(x =>  x.Requests)
            .ThenInclude(x => x.ExpenseReport)
            .Where(x => x.Initiated == "Yes" && x.Requests.Any(x => x.ExpenseReportGiven != false))
            .Select(x => new Budget{
                  Id = x.Id,
                  Subject = x.Subject,
                  Destination = x.Destination,
                  ArrivalDate = x.ArrivalDate,
                  DepartureDate = x.DepartureDate,
                  TripId = x.TripId,
                 TotalTripBudget = x.TotalTripBudget,
                 Requests = x.Requests.Select(x => new Request {
                    Id = x.Id,
                    ExpenseReport = new ExpenseReport{
                        Expenses = x.ExpenseReport.Expenses
                    }
                 })
                 .ToList(),
            })
           
           
            
            .ToListAsync();

            return result;
        }


        public async Task<Budget> GetReport(int id){
            var result = await _travelContext.Budgets.AsNoTracking().AsSplitQuery()
            .Include(x => x.Requests)
            .ThenInclude(x => x.ExpenseReport)
            .ThenInclude(x => x.Expenses)
            .Include(x => x.Requests)
            .ThenInclude(x => x.Requester)
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync();

            return result;

        }



        public async Task<List<Budget>> GetReportsForDownload(){
            var result = await _travelContext.Budgets.AsNoTracking().AsSplitQuery()
            .Include(x => x.Requests.Where(x => x.ExpenseReportGiven == true))
            .ThenInclude(x => x.ExpenseReport)
            .ThenInclude(x => x.Expenses)
            .Include(x => x.Requests)
            .ThenInclude(x => x.Requester)
            .Include(x => x.Requests)
            .ThenInclude(x => x.MoneyReceipt)
            .ToListAsync();

            return result;
        }


      

        // Read a single quotation by id
      

       

        // Delete a quotation
      
    }
}
      





