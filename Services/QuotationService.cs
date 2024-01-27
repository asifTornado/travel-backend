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

namespace backEnd.services
{
    public class QuotationService : IQuotationService
    {
        private TravelContext _travelContext;

        private IConnection _connection;

        public QuotationService(TravelContext travelContext, IConnection connection)
        {
            _travelContext = travelContext;
            _connection = connection;
        }


        public async Task UpdateTicketQuotations(List<Quotation> quotations){


            foreach(var quotation in quotations){
                _travelContext.Entry(quotation).State = EntityState.Modified;

                foreach(var ticketApproval in quotation.TicketApprovals){
                    _travelContext.Entry(ticketApproval).State = EntityState.Modified;
                }
            }
           
            await _travelContext.SaveChangesAsync();

        }

        public async Task UpdateHotelQuotations(List<HotelQuotation> quotations){

                 foreach(var quotation in quotations){
                _travelContext.Entry(quotation).State = EntityState.Modified;

                foreach(var hotelApproval in quotation.HotelApprovals){
                    _travelContext.Entry(hotelApproval).State = EntityState.Modified;

                }
              }

          

            await _travelContext.SaveChangesAsync();

        }

        // Read all quotations
        public async Task<List<Quotation>> GetAllQuotations()
        {

            var results = await _travelContext.Quotations.AsNoTracking().ToListAsync();
            return results;
            // await using var connection = _connection.GetConnection();
            // await connection.OpenAsync();

            // var sql = "SELECT * FROM dbo.Quotations";

            // var result = await connection.QueryAsync<Quotation>(sql);

            // return result.ToList();
        }

        // Read a single quotation by id
        public async Task<Quotation> GetQuotationById(int id)
        {

            var result = await _travelContext.Quotations.AsNoTracking()
            .Include(x => x.Request)
            .FirstOrDefaultAsync(q => q.Id == id);
            return result;
            // await using var connection = _connection.GetConnection();
            // await connection.OpenAsync();

            // var sql = "SELECT * FROM dbo.Quotations WHERE Id = @Id";

            // var result = await connection.QueryFirstOrDefaultAsync<Quotation>(sql, new { Id = id });

            // return result;

            
        }

        public async Task<HotelQuotation> GetHotelQuotationById(int id){
                
                var result = await _travelContext.HotelQuotations.AsNoTracking()
                .Include(x => x.Request)
                .FirstOrDefaultAsync(q => q.Id == id);
                return result;
        }

        // Create a new quotation
        public async Task CreateQuotation(Quotation quotation)
        {

            _travelContext.Entry(quotation).State = EntityState.Added;

            await _travelContext.SaveChangesAsync();

            // await using var connection = _connection.GetConnection();
            // await connection.OpenAsync();

            // var sql = @"INSERT INTO dbo.Quotations 
            // (AgentId, QuotationText, Selected, Booked, Confirmed, Hovered, 
            // Custom, RequestId) 
            // VALUES (@AgentId, @QuotationText, @Selected, @Booked, @Confirmed, @Hovered, @Custom, @RequestId)";



            // await connection.ExecuteAsync(sql, quotation);
        }

        // Update an existing quotation
        public async Task UpdateQuotation(Quotation quotation)
        {

            _travelContext.Entry(quotation).State = EntityState.Modified;

            await _travelContext.SaveChangesAsync();

            // await using var connection = _connection.GetConnection();
            // await connection.OpenAsync();

            // var sql = @"INSERT INTO dbo.Quotations 
            // SET AgentId = @AgentId, QuotationText = @QuotationText, Selected = @Selected, Booked = @Booked, 
            // Confirmed = @Confirmed, Hovered = @Hovered, 
            // Custom = @Custom, RequestId = @RequestId";
            
            // await connection.ExecuteAsync(sql, quotation);
            
        }

        // Delete a quotation
        public async Task DeleteQuotation(Quotation quotation)
        {


            _travelContext.Entry(quotation).State = EntityState.Deleted;

            await _travelContext.SaveChangesAsync();
            // await using var connection = _connection.GetConnection();
            // await connection.OpenAsync();

            // var sql = "DELETE FROM dbo.Quotations WHERE Id = @Id";

            // await connection.ExecuteAsync(sql, new { Id = quotation.Id });


        }
    }
}
      





