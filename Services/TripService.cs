using backEnd.Models;
using backEnd.Services;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Org.BouncyCastle.Tls;
using backEnd.Services.IServices;
using backEnd.Factories.IFactories;
using System.Runtime.CompilerServices;
using Microsoft.Data.SqlClient;
using backEnd.Models.DTOs;
using backEnd.Models.IModels;
using backEnd.Mappings;
using Dapper;
using System.Data;
using AutoMapper;
using System.Text.Json;
using ZstdSharp.Unsafe;
using Newtonsoft.Json.Converters;


namespace backEnd.Services;

public class TripService:ITripService
{
      
    private TravelContext _travelContext;

    private IConnection _connection;


    private IMapper _imapper;
 

    public TripService(TravelContext travelContext, IConnection connection, IMapper mapper)
    {
       _travelContext = travelContext;
         _connection = connection;
            _imapper = mapper;
     
     
    }


    public async Task AddQuotations<T> (List<T> quotations ){
        foreach(var quotation in quotations){
            _travelContext.Entry(quotation).State = EntityState.Added;
        }

        await _travelContext.SaveChangesAsync();
    }


    public async Task AddOrUpdateTicketInvoice(TicketInvoice invoice){

        if(invoice.Id == 0 || invoice.Id == null){
            _travelContext.Entry(invoice).State = EntityState.Added;
    }else{
            _travelContext.Entry(invoice).State = EntityState.Modified;
    
    }

    foreach(var quotation in invoice.Quotations){
        _travelContext.Entry(quotation).State = EntityState.Modified;
    }

    await _travelContext.SaveChangesAsync();

    }


    public async Task AddOrUpdateHotelInvoice(HotelInvoice invoice){
        if(invoice.Id == 0 || invoice.Id == null){
            _travelContext.Entry(invoice).State = EntityState.Added;

        }else{
            _travelContext.Entry(invoice).State = EntityState.Modified;
        }

        foreach(var quotation in invoice.Quotations){
            _travelContext.Entry(quotation).State = EntityState.Modified;
        }

        await _travelContext.SaveChangesAsync();

    }



    public async Task UpdateTicketQuotationsAndRequests (List<Quotation> quotations, List<Request> requests){
           
           foreach(var quotation in quotations){
            if(quotation.Id == 0 || quotation.Id == null){
            _travelContext.Entry(quotation).State = EntityState.Added;
             }else{
            _travelContext.Entry(quotation).State = EntityState.Modified;
             }
           }

             foreach(var request in requests){
                 _travelContext.Entry(request).State = EntityState.Modified;
             }


             await _travelContext.SaveChangesAsync();
   





}



 public async Task UpdateHotelQuotationsAndRequests (List<HotelQuotation> quotations, List<Request> requests){
           
           foreach(var quotation in quotations){
            if(quotation.Id == 0 || quotation.Id == null){
            _travelContext.Entry(quotation).State = EntityState.Added;
             }else{
            _travelContext.Entry(quotation).State = EntityState.Modified;
             }
           }

             foreach(var request in requests){
                 _travelContext.Entry(request).State = EntityState.Modified;
             }


             await _travelContext.SaveChangesAsync();
   





}


public async Task<List<Request>> GetRequestsForReversal(List<int> requestIds, string quotationText){
    var result = await _travelContext.Requests
                 .AsNoTracking()
                 .Where(x => requestIds.Contains(x.Id))
                 .ToListAsync();

    return result;    
}


public async Task<List<Quotation>> GetQuotationsForReversal(List<int> requestIds, string quotationText){
    var result = await _travelContext.Quotations
                 .AsNoTracking()
                 .Where(x => x.QuotationText == quotationText)
                 .ToListAsync();

    return result;    

}



public async Task<List<Quotation>> GetRelatedTicketQuotations(Quotation quotation){
    var result = await _travelContext.Quotations.AsNoTracking()
                 .Where(x => x.Linker == quotation.Linker)
                 .ToListAsync();

                 return result;
}




public async Task<List<HotelQuotation>> GetRelatedHotelQuotations(HotelQuotation quotation){
    var result = await _travelContext.HotelQuotations.AsNoTracking()
                 .Where(x => x.Linker == quotation.Linker)
                 .ToListAsync();

                 return result;
}


public async Task<List<Request>> GetRelatedRequests(Request request){

    var quotation = request.Quotations.Where(x => x.Booked == true).FirstOrDefault();

    var result = await _travelContext.Requests
                 .AsNoTracking()
                 .Include(x => x.Quotations)
                 .Include(x => x.Requester)
                 .Include(x => x.Requester.SuperVisor)
                 .Include(x => x.Requester.ZonalHead)
                 .Where(x => x.Quotations.Any(x => x.Linker == quotation.Linker))
                 .ToListAsync();

    return result;    
}


public async Task<List<Request>> GetRelatedHotelRequests(Request request){

    var quotation = request.HotelQuotations.Where(x => x.Booked == true).FirstOrDefault();

    var result = await _travelContext.Requests
                 .AsNoTracking()
                 .Include(x => x.HotelQuotations)
                 .Include(x => x.Requester)
                 .Include(x => x.Requester.SuperVisor)
                 .Include(x => x.Requester.ZonalHead)
                 .Where(x => x.HotelQuotations.Any(x => x.Linker == quotation.Linker))
                 .ToListAsync();

    return result;    
}



public async Task ApproveRelatedQuotes(Quotation quotation){
   var linkId = quotation.Linker;

   await _travelContext.Database.ExecuteSqlRawAsync($"UPDATE dbo.Quotations SET Approved = 1 WHERE Linker = {linkId}");

}


public async Task ApproveRelatedHotelQuotes(HotelQuotation quotation){
    var linkId = quotation.Linker;

    await _travelContext.Database.ExecuteSqlRawAsync($"UPDATE dbo.HotelQuotations SET Approved = 1 WHERE Linker = {linkId}");
}

public async Task<List<Request>> GetRelatedRequestsFromQuotation(Quotation quotation){
    var result = await _travelContext.Requests
                    .AsNoTracking()
                    .AsSplitQuery()
                    .Include(x => x.Quotations)
                    .ThenInclude(x => x.Invoices)
                    .Include(x => x.Quotations)
                    .ThenInclude(x => x.TicketApprovals)
                    .Include(x => x.Requester.SuperVisor)
                    .Include(x => x.Requester.ZonalHead)
                    .Where(x => x.Quotations.Any(x => x.Linker == quotation.Linker))
                    .ToListAsync();
    return result;
}




public async Task<List<Request>> GetRelatedRequestsFromHotelQuotation(HotelQuotation quotation){
    var result = await _travelContext.Requests
                    .AsNoTracking()
                    .Include(x => x.Quotations)
                    .Include(x => x.HotelQuotations)
                    .ThenInclude(x => x.Invoices)
                    .Include(x => x.HotelQuotations)
                    .ThenInclude(x => x.HotelApprovals)
                    .Include(x => x.Requester.SuperVisor)
                    .Include(x => x.Requester.ZonalHead)
                    .Where(x => x.HotelQuotations.Any(x => x.Linker == quotation.Linker))
                    .ToListAsync();
    return result;
}



public async Task UpdateRequests(List<Request> requests){
    
        foreach(var request in requests){
            _travelContext.Entry(request).State = EntityState.Modified;
        }

        await _travelContext.SaveChangesAsync();
    }





}