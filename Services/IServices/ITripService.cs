using backEnd.Models;
using backEnd.Models.DTOs;




namespace backEnd.Services.IServices
{
    public interface ITripService
    {
       

      Task AddQuotations<T> (List<T> quotations );
      Task AddOrUpdateTicketInvoice(TicketInvoice invoice);
      Task AddOrUpdateHotelInvoice(HotelInvoice invoice);
      Task UpdateTicketQuotationsAndRequests (List<Quotation> quotations, List<Request> requests);
      Task UpdateHotelQuotationsAndRequests (List<HotelQuotation> quotations, List<Request> requests);
      Task<List<Request>> GetRequestsForReversal(List<int> requestIds, string quotationText);
      Task<List<Quotation>> GetQuotationsForReversal(List<int> requestIds, string quotationText);
      Task<List<Quotation>> GetRelatedTicketQuotations(Quotation quotation);
      Task<List<HotelQuotation>> GetRelatedHotelQuotations(HotelQuotation quotation);
      Task<List<Request>> GetRelatedRequests(Request request);
      Task<List<Request>> GetRelatedRequestsFromQuotation(Quotation quotation);
      Task<List<Request>> GetRelatedRequestsFromHotelQuotation(HotelQuotation quotation);
      Task UpdateRequests(List<Request> requests);

      Task ApproveRelatedQuotes(Quotation quotation);

      Task<List<Request>> GetRelatedHotelRequests(Request request);
      Task ApproveRelatedHotelQuotes(HotelQuotation quotation);



        
    }
}