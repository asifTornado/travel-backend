

using backEnd.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backEnd.Services.IServices
{
    public interface IQuotationService
    {
        Task<List<Quotation>> GetAllQuotations();
        Task<Quotation> GetQuotationById(int id);
        Task<HotelQuotation> GetHotelQuotationById(int id);
        Task CreateQuotation(Quotation quotation);
        Task UpdateQuotation(Quotation quotation);
        Task DeleteQuotation(Quotation quotation);

        Task UpdateTicketQuotations(List<Quotation> quotations);

        Task UpdateHotelQuotations(List<HotelQuotation> quotations);

      
        
    }
}

