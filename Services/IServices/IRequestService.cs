using backEnd.Models;
using backEnd.Models.DTOs;




namespace backEnd.Services.IServices
{
    public interface IRequestService
    {
        Task<List<RequestDTO>> GetRequestssRaisedByUser(User user);
        Task<List<RequestDTO>> GetRequestsForMe(User user);
        Task<List<RequestDTO>> GetRequestsProcessedByMe(User user);
        Task<Request?> GetAsync(int? id);
        Task<int> CreateAsync(Request newRequest);
        Task<List<RequestDTO>> GetAllRequests();

        Task GiveInvoiceProfessional(Request request, TicketInvoice invoice);
        Task UpdateAsync(Request? updatedRequest);
        Task RemoveAsync(int id);

      

        Task<(string, string)> UpdateInvoice(int id, string filepath, string what);


        Task UpdateAsyncDapper(RequestDTO? request, QuotationDTO quotation, string what);


        Task UpdateStatus(Request request);


        Task UpdateHotelQuotation(List<HotelQuotation> quotations);

        Task<Request> GetCustomRequest(int id);

        Task<List<RequestDTO>> GetCustomRequests();
        Task<Request> GetRequestForApproval(int id);

        Task UpdateRequestForApproval(Request request);
        
         Task<List<Request>> GetUnapprovedRequests(int id);
    


        // Task UpdateQuotation(Quotation quotation);

        // Task UpdateHotelQuotation(HotelQuotation quotation);

        
    }
}