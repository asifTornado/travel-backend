using backEnd.Models;
using System.Threading.Tasks;

namespace backEnd.Helpers.IHelpers;



    public interface INotifier
    {
        Task InsertNotification(string message, int? from, int? to, int ticketId, string Event, string type = "message");
        Task DeleteNotification(int? TicketId, int? To, string Event);
    }


