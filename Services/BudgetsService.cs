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

public class BudgetsService : IBudgetsService
{

    
    private TravelContext _travelContext;

    private IConnection _connection;

    public BudgetsService(TravelContext travelContext, IConnection connection)
    {
        _travelContext = travelContext;
        _connection = connection;
 
    }




    public async Task<Budget?> GetAsync(int id){


        // var result  = await _travelContext.Budgets
        // .AsNoTracking()
        // .Include( x => x.Requests)
        // .ThenInclude(x => x.Requester)
        // .ThenInclude(x => x.SuperVisor)
        // .Include( x => x.Requests)
        // .ThenInclude(x => x.Requester)
        // .ThenInclude(x => x.ZonalHead)
        // .Include(x => x.Travelers)
        // .ThenInclude(x => x.TravelHandler)
        // .Include(x => x.Requests)
        // .ThenInclude(x =>  x.Quotations)
        // .ThenInclude(x => x.Invoices)
        // .Include(x => x.Requests)
        // .ThenInclude( x => x.Quotations)
        // .ThenInclude(x => x.TicketApprovals)
        // .Include(x => x.Requests)
        // .ThenInclude( x => x.HotelQuotations)
        // .ThenInclude(x => x.HotelApprovals)
        // .Include(x => x.Requests)
        // .ThenInclude(x => x.CurrentHandler)
        // .Include(x => x.Requests)
        // .ThenInclude(x => x.HotelQuotations)
        // .ThenInclude(x => x.Invoices)

        // .FirstOrDefaultAsync(b => b.Id == id);


        var budget = await _travelContext.Budgets.AsNoTracking()
        .Include(x => x.Travelers)
        .FirstOrDefaultAsync(b => b.Id == id);

        var requests = await _travelContext.Requests.AsNoTracking()
        .Include(x => x.Requester)
        .Include(x => x.Requester.ZonalHead)
        .Include(x => x.Requester.SuperVisor)
        .Include(x => x.Requester.TravelHandler)
        .Include(x => x.CurrentHandler)
        .Include("HotelQuotations.Invoices")
        .Include("HotelQuotations.HotelApprovals")
        .Include("Quotations.Invoices")
        .Include("Quotations.TicketApprovals")
        .Where(x => x.BudgetId == id).ToListAsync();
        

        budget.Requests = requests;
        






     

        Console.WriteLine("Sending trip from service");
        return budget;
        
        
        
    }

    public async Task CreateBudget(Budget budget) {


        _travelContext.Entry(budget).State = EntityState.Added;

        budget.Travelers.ForEach((x)=>{
            _travelContext.Entry(x).State = EntityState.Modified;            
        });

        await _travelContext.SaveChangesAsync();
        
    }

    public async Task CreateCustomRequestBudget(Budget budget){
        
        _travelContext.Entry(budget).State = EntityState.Added;

        budget.Requests.ForEach((x)=>{
            _travelContext.Entry(x).State = EntityState.Added;
        });

        await _travelContext.SaveChangesAsync();


    }


    public async Task<List<Budget>> GetAllBudgets(){

      var results = await _travelContext.Budgets.AsNoTracking().ToListAsync();
      return results;

    }



public async Task UpdateAsync(int id, Budget budget)
{
    // Load the existing budget with its travelers from the database
    var existingBudget = await _travelContext.Budgets
        .Include(b => b.Travelers)
        .SingleOrDefaultAsync(b => b.Id == budget.Id);

    if (existingBudget == null)
    {
        // Handle the case where the budget doesn't exist
        throw new KeyNotFoundException("Budget not found");
    }

    // Update the scalar properties of the budget
    _travelContext.Entry(existingBudget).CurrentValues.SetValues(budget);

    // Remove the travelers that are no longer associated with the budget
    foreach (var existingTraveler in existingBudget.Travelers.ToList())
    {
        if (!budget.Travelers.Any(t => t.Id == existingTraveler.Id))
            existingBudget.Travelers.Remove(existingTraveler);
    }

    // Add the new travelers that are now associated with the budget
    foreach (var newTraveler in budget.Travelers)
    {
        if (!existingBudget.Travelers.Any(t => t.Id == newTraveler.Id))
            existingBudget.Travelers.Add(newTraveler);
    }


    foreach(var request in budget.Requests){
        _travelContext.Entry(request).State = EntityState.Modified;
    }

    await _travelContext.SaveChangesAsync();
    _travelContext.ChangeTracker.Clear();
}
       

    public async Task RemoveAsync(int id){

        _travelContext.Entry(new Budget{Id = id}).State = EntityState.Deleted;

        await _travelContext.SaveChangesAsync();

  
    }


    public async Task<JsonResult> SearchBudget(Dictionary<string, string> searchObject){
        
        var twoMonthsFromNow = DateTime.Now.AddMonths(2);
 
       IQueryable<Budget> query = _travelContext.Budgets.AsNoTracking().Include( b => b.Travelers);

if (!string.IsNullOrEmpty(searchObject["name"]))
{
    query = query.Where(b => b.Travelers.Any(x => x.EmpName == searchObject["name"]) );
}

if (!string.IsNullOrEmpty(searchObject["destination"]))
{
    query = query.Where(b => b.Destination == searchObject["destination"]);
}

var results = await query.ToListAsync();

var filteredResults  = results.Where(b => DateTime.ParseExact(b.DepartureDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) < twoMonthsFromNow).ToList();

return new JsonResult(filteredResults);


     

    }

    public async Task<List<Budget>> GetAllInitiatedTrips()
    {   _travelContext.ChangeTracker.Clear();
        var result = await _travelContext.Budgets.AsNoTracking()
        .Where(b => b.Initiated == "Yes").ToListAsync();
        return result;
    }


    public async Task<Budget> GetTrip(int id)
    {
        var result = await _travelContext.Budgets.AsNoTracking()
        .Include(b => b.Requests)
        .ThenInclude( r => r.Requester)
        .Include(b => b.Requests)
        .ThenInclude( r => r.CurrentHandler)
        .FirstOrDefaultAsync(b => b.Initiated == "Yes" && b.Id == id);
        return result;
    }
}




