

using backEnd.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace backEnd.Services.IServices
{
    public interface INotificationService
    {
        Task<List<Notification>> GetNotifications();
        Task<Notification> GetNotification(int id);
        Task RemoveNotification(int id);
        Task InsertNotification(Notification notification);

        Task <IEnumerable<Notification>> GetNotificationsByUser(int id);

        Task DeleteNotification(int? TicketId, int? To, string Event);
    }
}
