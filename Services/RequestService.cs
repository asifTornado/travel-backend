using backEnd.Models;
using backEnd.Models.DTOs;
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
using backEnd.Mappings;
using Dapper;
using System.Data;
using AutoMapper;
using System.Text.Json;
using ZstdSharp.Unsafe;
using Newtonsoft.Json.Converters;
using AutoMapper.QueryableExtensions;

namespace backEnd.Services;

public class RequestService: IRequestService
{
      
    private TravelContext _travelContext;

    private IConnection _connection;


    private IMapper _imapper;
 

    public RequestService(TravelContext travelContext, IConnection connection, IMapper mapper)
    {
       _travelContext = travelContext;
         _connection = connection;
            _imapper = mapper;
     
     
    }


 


 









    public async Task<List<RequestDTO>> GetRequestssRaisedByUser(User user)
    {   
       
        var results =  await _travelContext.Requests.Include(r => r.Requester)
        .AsNoTracking()
        .Where( r => r.Requester.EmpName==  user.EmpName || user.UserType == "admin")
        .ProjectTo<RequestDTO>(_imapper.ConfigurationProvider)
        .ToListAsync();
        return results;
    }



        public async Task<List<RequestDTO>> GetRequestsForMe(User user)
   {    
      
    
        var results = await _travelContext.Requests.Include(r => r.CurrentHandler)
        .AsNoTracking()
        .Include(r => r.Requester.SuperVisor)
        .Where(r => r.CurrentHandler.MailAddress == user.MailAddress)
        .ProjectTo<RequestDTO>(_imapper.ConfigurationProvider)
        .ToListAsync();
        return results;
    }



        public async Task<List<RequestDTO>> GetRequestsProcessedByMe(User user)
   {     

 
        var results = await _travelContext.Requests.Include(r => r.Requester)
                         .AsNoTracking()
                        .Where(r => r.Requester.EmpName == user.EmpName)
                        .ProjectTo<RequestDTO>(_imapper.ConfigurationProvider)
                        .ToListAsync(); 
        return results;
    }




    

    public async Task<Request?> GetAsync(int? id){
      
       


       var results = await _travelContext.Requests.AsNoTracking()
    .Include(x => x.Messages)
    // .Include(x => x.HotelApprovals)
    // .Include(x => x.TicketApprovals)
    .Include(x => x.Requester)
        .Include(x => x.Requester.SuperVisor)
        .Include(x => x.Requester.TravelHandler)
        .Include(x => x.Requester.ZonalHead)
    .Include(x => x.HotelQuotations)
    .ThenInclude(x => x.Invoices)
       
   
    .Include(x => x.Quotations)
    .ThenInclude(x => x.Invoices)
    
    // .Include(x => x.Invoices)
    .Include(x => x.CurrentHandler)
    .Include(x => x.Quotations)
    .ThenInclude(x => x.TicketApprovals)
    .Include(x => x.HotelQuotations)
    .ThenInclude(x => x.HotelApprovals)

.Where(r => r.Id == id)
.FirstOrDefaultAsync();

    
        return results;


        
    }

    public async Task<int> CreateAsync(Request newRequest) {

           

           _travelContext.Entry(newRequest).State = EntityState.Added;

    

          
           await _travelContext.SaveChangesAsync();

           return newRequest.Id;

       
    }


    public async Task<List<RequestDTO>> GetAllRequests(){
        

   var result = await _travelContext.Requests.Include( r => r.CurrentHandler)
   .AsNoTracking()
   .Where(x => x.Custom == false)
   .ProjectTo<RequestDTO>(_imapper.ConfigurationProvider)
   .ToListAsync();
         return result;
    }

    

    public async Task UpdateAsync(Request? updatedRequest){

           _travelContext.ChangeTracker.Clear();

           _travelContext.Entry(updatedRequest).State = EntityState.Modified;

           foreach(var quotation in updatedRequest.Quotations){
            if(quotation.Id == 0 || quotation.Id == null){
                _travelContext.Entry(quotation).State = EntityState.Added;
              
            }else{
                _travelContext.Entry(quotation).State = EntityState.Modified;
            }
           }

            foreach(var hotelquotation in updatedRequest.HotelQuotations){
            if(hotelquotation.Id == 0 || hotelquotation.Id == null){
                _travelContext.Entry(hotelquotation).State = EntityState.Added;
              
            }else{
                _travelContext.Entry(hotelquotation).State = EntityState.Modified;
            }
            }


            foreach(var message in updatedRequest.Messages){
                if(message?.Id == null || message?.Id == 0){
                    _travelContext.Entry(message).State = EntityState.Added;
                }else{
                    _travelContext.Entry(message).State = EntityState.Modified;
                }
            }


        await _travelContext.SaveChangesAsync();



       



    }

    


    public async Task UpdateHotelQuotation(List<HotelQuotation> quotations ){

       foreach(var quotation in quotations){
        _travelContext.Entry(quotation).State = EntityState.Modified;
       }

        await _travelContext.SaveChangesAsync();

    }


    public async Task UpdateStatus(Request request){


      

        _travelContext.Entry(request).State = EntityState.Modified;

        await _travelContext.SaveChangesAsync();
    



    }


    public async Task UpdateAsyncDapper(RequestDTO? request, QuotationDTO quotation, string what){

        await using SqlConnection connection = _connection.GetConnection();
        await connection.OpenAsync();


        

        var sqlRequest = @"
        UPDATE dbo.Requests SET 
        CurrentHandlerId = @CurrentHandlerId,
        Status = @Status,
        Confirmed = @Confirmed,
        SeekingInvoices = @SeekingInvoices,
        Processed = @Processed,
        SeekingHotelInvoices = @SeekingHotelInvoices,
        Best = @Best,
        Booked = @Booked,
        TicketInvoiceUploaded = @TicketInvoiceUploaded,
        HotelInvoiceUploaded = @HotelInvoiceUploaded,
        HotelBooked = @HotelBooked,
        HotelConfirmed = @HotelConfirmed,
        BestHotel = @BestHotel,
        InTrip = @InTrip
        
        WHERE Id = @Id;    
        
        ";


        var sqlQuotation = $@"
        UPDATE {what} SET
        Booked = @Booked,
        Confirmed = @Confirmed,
        Selected = @Selected
        
    
        WHERE Id = @Id;
        
        ";

       using(var transaction = await connection.BeginTransactionAsync()){

           try{

                var row2 = await connection.ExecuteAsync(sqlRequest, request, transaction);

                var row = await connection.ExecuteAsync(sqlQuotation, quotation, transaction);

                await transaction.CommitAsync();

           }catch(Exception ex){

               transaction.Rollback();

               throw ex;
           }
       }
        

    }


 public async Task GiveInvoiceProfessional(Request request, TicketInvoice invoice){
       
       _travelContext.Entry(request).State = EntityState.Modified;

       _travelContext.Entry(invoice).State = EntityState.Added;

        await _travelContext.SaveChangesAsync();
 }




    public async Task<(string, string)> UpdateInvoice(int id, string filePath, string what){


        var updatedRequest = new Request{ Id = id};

        if(what == "ticket"){
            updatedRequest.TicketInvoiceUploaded = true;
               _travelContext.Entry(updatedRequest).Property(r => r.TicketInvoiceUploaded).IsModified = true;
        }else{
            updatedRequest.HotelInvoiceUploaded = true;
              _travelContext.Entry(updatedRequest).Property(r => r.HotelInvoiceUploaded).IsModified = true;
        }

      


         await _travelContext.SaveChangesAsync();


         var request = await _travelContext.Requests
         .Include(r => r.Requester)
         .Include(r => r.Requester.TravelHandler)
         .AsNoTracking()
         .Where(r => r.Id == id)
         .Select(r => new {r.Requester.TravelHandler.MailAddress, r.Requester.TravelHandler.EmpName})
         
         .FirstOrDefaultAsync();


         return (request.MailAddress, request.EmpName);


        //   await using SqlConnection connection = _connection.GetConnection();
        //   await connection.OpenAsync();
          

        //   var sql = "dbo.UpdateInvoice";
          

        //   var result = await connection.QueryFirstOrDefaultAsync<(string, string)>(sql, new {Id = id, Filepath = filePath, What = what}, commandType: CommandType.StoredProcedure);


        //   return result;


      
    }




        

    public async Task RemoveAsync(int id){


        var requestToDelete = new Request{ Id = id};

        _travelContext.Entry(requestToDelete).State = EntityState.Deleted;

        await _travelContext.SaveChangesAsync();

        // var requestToDelete = new Request{ Id = id};

        // await using var connection = _connection.GetConnection();
        // await connection.OpenAsync();

        // var sql = "DELETE FROM dbo.Requests WHERE Id = @Id";

        // await connection.ExecuteAsync(sql, requestToDelete);

        


        
    }



    public async Task<Request> GetCustomRequest(int id){
    var result = await  _travelContext.Requests.AsNoTracking()
    .Include("Requester.TravelHandler")
    .Include("Requester.SuperVisor")
    .Include("Requester.ZonalHead")
    .Include("HotelQuotations.Invoices")
    .Include("HotelQuotations.HotelApprovals")
    .Include("Quotations.Invoices")
    .Include("Quotations.TicketApprovals")
    .FirstOrDefaultAsync(x => x.Custom == true);
    
    return result;
}


 

   public async Task<List<RequestDTO>> GetCustomRequests(){
    var result = await _travelContext.Requests.AsNoTracking().Where(x => x.Custom == true)
    .ProjectTo<RequestDTO>(_imapper.ConfigurationProvider)
    .ToListAsync();
    return result;
   }



   public async Task<Request> GetRequestForApproval(int id){
    var result = await _travelContext.Requests.AsNoTracking()
                     .Include(x => x.Requester.SuperVisor)
                     .Include(x => x.Requester.TravelHandler)
                     .Include(x => x.Requester.ZonalHead)
                     .Include(x => x.Requester)
                     .Where(x => x.Id == id && x.PermanentlyRejected != true).FirstOrDefaultAsync();
    return result;
   }

   public async Task UpdateRequestForApproval(Request request){
    
    _travelContext.Entry(request).State = EntityState.Modified;
    
    await _travelContext.SaveChangesAsync();

   }


   public async Task<List<Request>> GetUnapprovedRequests(int id){
    var result = await _travelContext.Requests.AsNoTracking()
                    .Include(x => x.Requester)
                    .Where(x => (x.SupervisorApproved == false || x.DepartmentHeadApproved  == false) && x.PermanentlyRejected != true
                    && (x.RequesterId == id || x.Requester!.SuperVisorId == id || x.Requester!.ZonalHeadId == id)
                    )
                    
                    .ToListAsync();
    return result;
   }





   









}




