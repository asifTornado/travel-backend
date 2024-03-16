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
using AutoMapper.QueryableExtensions;
using backEnd.Models.DTOs;
using backEnd.Mappings;
using AutoMapper;
using AutoMapper.QueryableExtensions;



namespace backEnd.Services;

public class BudgetsService : IBudgetsService
{

    
    private TravelContext _travelContext;

    private IConnection _connection;


    private IMapper _mapper;

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


    //    var result = await _travelContext.Budgets
    // .AsNoTracking()
    // .Where(b => b.Id == id)
    // .Select(b => new Budget
    // {
    //     Id = b.Id,
    //     // Add other properties you need from the Budget entity
    //     Destination = b.Destination,
    //     Subject = b.Subject,
    //     ArrivalDate = b.ArrivalDate,
    //     DepartureDate = b.DepartureDate,
    //     NumberOfTravelers = b.NumberOfTravelers,
    //     AirTicketBudget = b.AirTicketBudget,
    //     HotelBudget = b.HotelBudget,
    //     TotalTripBudget = b.TotalBookingCost,
    //     TransportExpense = b.TransportExpense,
    //     IncidentalExpense = b.IncidentalExpense,
    //     TotalBookingCost = b.TotalBookingCost,
        
        
        
    //     Requests = b.Requests.Select(r => new Request
    //     {

    //         Id = r.Id,
    //         Confirmed = r.Confirmed,
    //         HotelConfirmed = r.HotelConfirmed,
    //         Status = r.Status,
    //         // Add other properties you need from the Request entity
    //         Requester = new User
    //         {
    //             // Add properties you need from the Requester entity
    //             // For example:
    //             MailAddress = r.Requester.MailAddress,
    //             Designation = r.Requester.Designation,
    //             Department = r.Requester.Department,
    //             EmpName = r.Requester.EmpName,
    //             SuperVisor = new User{
    //                 Id = r.Requester.SuperVisor.Id,
    //                 EmpName = r.Requester.SuperVisor.EmpName,
    //                 MailAddress = r.Requester.SuperVisor.MailAddress
    //             },
    //             ZonalHead = new User{
    //                 Id = r.Requester.ZonalHead.Id,
    //                 EmpName = r.Requester.ZonalHead.EmpName,
    //                 MailAddress = r.Requester.ZonalHead.MailAddress
    //             },
    //             // Add more properties as needed
    //         },
          
    //         Quotations = r.Quotations.Select(q => new Quotation
    //         {
    //             // Add properties you need from the Quotation entity
    //             // For example:
    //             Id = q.Id,
    //             QuotationText = q.QuotationText,
    //             QuoteGiver = q.QuoteGiver,
    //             Confirmed = q.Confirmed,
    //             Booked = q.Booked,
    //             Approved = q.Approved,
    //             Invoices = q.Invoices.Select(i => new TicketInvoice{
    //                 Filename = i.Filename,
    //                 FilePath = i.FilePath
    //             }).ToList(),
    //             TicketApprovals = q.TicketApprovals.Select(t => new User{
    //                 EmpName = t.EmpName
    //             }).ToList(),
    //             // Add more properties as needed
    //         }).ToList(),
    //         HotelQuotations = r.HotelQuotations.Select(hq => new HotelQuotation
    //         {
    //             // Add properties you need from the HotelQuotation entity
    //             // For example:
    //             Id = hq.Id,
    //             HotelApprovals = hq.HotelApprovals.Select(ht => new User {
    //                 EmpName = ht.EmpName
    //             }).ToList(),
    //             Invoices = hq.Invoices.Select(hi => new HotelInvoice{
    //                 Filename = hi.Filename,
    //                 FilePath = hi.FilePath
    //             }).ToList(),
    //             Confirmed = hq.Confirmed,
    //             Approved = hq.Approved,
    //             Booked = hq.Booked,
    //             QuoteGiver = hq.QuoteGiver,
    //             QuotationText = hq.QuotationText,
            
    //             // Add more properties as nded
    //         }).ToList(),
    //         CurrentHandler = new User{
    //             EmpName = r.CurrentHandler.EmpName,
    //             Id = r.CurrentHandler.Id,
    //             MailAddress = r.CurrentHandler.MailAddress
    //         },
    //         // Add projections for other related entities as needed
    //     }).ToList(),
    //     Travelers = b.Travelers.Select(t => new User{
    //         EmpName = t.EmpName,
    //         Id = t.Id,
    //         MailAddress = t.MailAddress
    //     }).ToList(),
    //     // Add projections for other related entities as needed
    // })
    // .FirstOrDefaultAsync();



        var budget = await _travelContext.Budgets.AsNoTracking()
        .AsSplitQuery()
        .Include(x => x.Travelers)
        .Include(x => x.TicketApprovals)
        .FirstOrDefaultAsync(b => b.Id == id);

        var requests = await _travelContext.Requests.AsNoTracking()
        .AsSplitQuery()
        .Include(x => x.Requester)
        .Include(x => x.Requester.ZonalHead)
        .Include(x => x.Requester.SuperVisor)
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


    public async Task<int> CreateBudgetId(Budget budget){
          _travelContext.Entry(budget).State = EntityState.Added;

        budget.Travelers.ForEach((x)=>{
            _travelContext.Entry(x).State = EntityState.Modified;            
        });

        await _travelContext.SaveChangesAsync();

        return budget.Id;

        
        
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

      var results = await _travelContext.Budgets.AsNoTracking()
      .ToListAsync();
      return results;

    }



public async Task UpdateBudgetSolo(Budget budget){
    _travelContext.Entry(budget).State = EntityState.Modified;
    await _travelContext.SaveChangesAsync();
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
    // _travelContext.Entry(existingBudget).CurrentValues.SetValues(budget);

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

    // _travelContext.Entry(budget).State = EntityState.Modified;

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

var results = await query.Where(b => b.Custom == false).ToListAsync();

var filteredResults  = results.Where(b => DateTime.ParseExact(b.DepartureDate, "yyyy-MM-dd", CultureInfo.InvariantCulture) < twoMonthsFromNow).ToList();

return new JsonResult(filteredResults);


     

    }

    public async Task<List<Budget>> GetAllInitiatedTrips()
    {   
        var result = await _travelContext.Budgets.AsNoTracking()
        .Where(b => b.Initiated == "Yes")
        
        .ToListAsync();
        return result;
    }


    public async Task<Budget> GetTicketQuotation(int id)
    {
        var result = await _travelContext.Budgets.AsNoTracking()
        .Include(b => b.Requests)
        .ThenInclude( r => r.Requester)
        .Include(b => b.Requests)
        .ThenInclude( r => r.CurrentHandler)
        .FirstOrDefaultAsync(b => b.Initiated == "Yes" && b.Id == id);
        return result;
    }


    
    public async Task<List<Budget>> GetAllTicketQuotations(){
        var result = await _travelContext.Budgets.AsNoTracking()
                    .Where(x =>  x.SeekingAccountsApprovalForTickets == true)
                    .ToListAsync();
       return result;
    }

 

    public async Task<List<Budget>> GetTicketQuotationsForMe(User user){
        var result = await _travelContext.Budgets.AsNoTracking()
                    .Where(x => x.CurrentHandlerId == user.Id && x.SeekingAccountsApprovalForTickets == true)
                    .ToListAsync();
       return result;
    }


       public async Task<List<Budget>> GetTicketQuotationsApprovedByMe(User user){
        var result = await _travelContext.Budgets.AsNoTracking()
                     .Include(x => x.TicketApprovals)
                     .Where(x => x.TicketApprovals.Any(x => x.Id == user.Id) && x.SeekingAccountsApprovalForTickets == true)
                     .ToListAsync();

       return result;


      
    }


     public async Task InsertBudgetTicketApprover(BudgetTicketApprovals budgetTicketApprovals){
        _travelContext.Entry(budgetTicketApprovals).State = EntityState.Added;
        await _travelContext.SaveChangesAsync();
       }
}




