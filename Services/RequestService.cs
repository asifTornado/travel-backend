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
using backEnd.Mappings;
using Dapper;
using System.Data;
using AutoMapper;
using System.Text.Json;
using ZstdSharp.Unsafe;
using Newtonsoft.Json.Converters;

namespace backEnd.services;

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


 


 









    public async Task<List<Request>> GetRequestssRaisedByUser(User user)
    {   
       
        var results =  await _travelContext.Requests.Include(r => r.Requester)
        .AsNoTracking()
        .Where( r => r.Requester.EmpName==  user.EmpName || user.UserType == "admin").ToListAsync();
        return results;
    }



        public async Task<List<Request>> GetRequestsForMe(User user)
   {    
      
    
        var results = await _travelContext.Requests.Include(r => r.CurrentHandler)
        .AsNoTracking()
        .Include(r => r.Requester.SuperVisor)
        .Where(r => r.CurrentHandler.MailAddress == user.MailAddress).ToListAsync();
    
        return results;
    }



        public async Task<List<Request>> GetRequestsProcessedByMe(User user)
   {     

 
        var results = await _travelContext.Requests.Include(r => r.Requester)
                         .AsNoTracking()
                        .Where(r => r.Requester.EmpName == user.EmpName).ToListAsync(); 
        return results;
    }




    

    public async Task<Request?> GetAsync(int id){
      
       


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

       

        //    _travelContext.Entry(newRequest.Requester).State = EntityState.Modified;

        //    _travelContext.Entry(newRequest.CurrentHandler).State = EntityState.Modified;

          
           await _travelContext.SaveChangesAsync();

           return newRequest.Id;


 
    //     await using var connection = _connection.GetConnection();
    //     await connection.OpenAsync();

    //     var sql = "dbo.CreateRequest";

    //     // var newRequestDTO = _imapper.Map<RequestDTO>(newRequest);

    //     // var parameters = new DynamicParameters(newRequestDTO);
    

    //     // parameters.Add("@RequestId", dbType: DbType.Int32, direction: ParameterDirection.Output);
    //     // // parameters.Add("@CurrentHandlerId", newRequestDTO.CurrentHandlerId);


    //     var parameters = new {
    //         Id = newRequest.Id, //
    //         Destination = newRequest.Destination, //
    //         Purpose = newRequest.Purpose, //
    //         Mode = newRequest.Mode, //
    //         AccomodationRequired = newRequest.AccomodationRequired, // 
    //         NumberOfNights = newRequest.NumberOfNights, //
    //         TotalCost = newRequest.TotalCost, //
    //         RequesterId = newRequest.Requester.Id, //
    //         Number = newRequest.Number, //
    //         Status = newRequest.Status, //
    //         AgentNumbers = newRequest.AgentNumbers, //
    //         CurrentHandlerId = newRequest.CurrentHandler.Id, //
    //         Date = newRequest.Date, //
    //         StartDate  = newRequest.StartDate, //
    //         EndDate = newRequest.EndDate, //
    //         Selected = newRequest.Selected, //
    //         Confirmed = newRequest.Confirmed, //
    //         SeekingInvoices = newRequest.SeekingInvoices, //
    //         Processed = newRequest.Processed, //
    //         SeekingHotelInvoices = newRequest.SeekingHotelInvoices, //
    //         Best = newRequest.Best, //
    //         Booked = newRequest.Booked, //
    //         TicketInvoiceUploaded = newRequest.TicketInvoiceUploaded, //
    //         HotelInvoiceUploaded = newRequest.HotelInvoiceUploaded, //
    //         HotelBooked = newRequest.HotelBooked,
    //         HotelConfirmed = newRequest.HotelConfirmed,
    //         BestHotel = newRequest.BestHotel,
    //         InTrip = newRequest.InTrip,
      
                

    //     };


    //     var parameters2  = new DynamicParameters(parameters);
    //     parameters2.Add("@RequestId", dbType: DbType.Int32, direction: ParameterDirection.Output);


    //     await connection.ExecuteAsync(sql, parameters2, commandType: CommandType.StoredProcedure);

    //     int requestId = parameters2.Get<int>("@RequestId");

    // //    var agents = await _travelContext.Agents.ToListAsync();
      

    // //   if(agents != null){

    // //      newRequest.Agents = agents;

    // //   }
    

    // //    _travelContext.Requests.Add(newRequest);

    // //    await _travelContext.SaveChangesAsync();

    // //    return newRequest.Id;

    //     return requestId;


       
    }


    public async Task<List<Request>> GetAllRequests(){
        

   var result = await _travelContext.Requests.Include( r => r.CurrentHandler)
   .AsNoTracking()
   .Where(x => x.Custom == false)
   .ToListAsync();
         return result;
    }

    

    public async Task UpdateAsync(Request? updatedRequest){
        

          
        //    if(updatedRequest.Costs != null && updatedRequest.Costs.Count > 0){
             
        //          foreach(var cost in updatedRequest.Costs){
        //              _travelContext.Entry(cost).State = EntityState.Detached;
        //          }
        //    }else{
        //       _travelContext.Entry(updatedRequest.Costs).State = EntityState.Detached;
        //    }

        //    _travelContext.Entry(updatedRequest.Requester).State = EntityState.Detached;
        //    _travelContext.Entry(updatedRequest.CurrentHandler).State = EntityState.Detached;
          
           

        
          



        
        //    _travelContext.Requests.Update(updatedRequest);
        //     await _travelContext.SaveChangesAsync();

           

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


            // foreach(var invoice in updatedRequest.TicketInvoices){
            //     if(invoice?.Id == null || invoice?.Id == 0){
            //         _travelContext.Entry(invoice).State = EntityState.Added;
            //     }else{
            //         _travelContext.Entry(invoice).State = EntityState.Modified;
            //     }
            // }


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


 

   public async Task<List<Request>> GetCustomRequests(){
    var result = await _travelContext.Requests.AsNoTracking().Where(x => x.Custom == true).ToListAsync();
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


   public async Task<List<Request>> GetUnapprovedRequests(){
    var result = await _travelContext.Requests.AsNoTracking()
                    .Where(x => x.SupervisorApproved == false && x.Custom == false && x.PermanentlyRejected != true)
                    .ToListAsync();
    return result;
   }





   









}




