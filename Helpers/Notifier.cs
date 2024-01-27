using backEnd.Helpers;
using backEnd.Models;
using backEnd.Services;
using backEnd.Helpers.IHelpers;
using System.Text.Json;
using backEnd.Services.IServices;
using backEnd.Helpers.IHelpers;



namespace backEnd.Helpers
{

    public class Notifier:INotifier
    {

        INotificationService _notificationService;
        private IHelperClass _helper;

        

        public Notifier(INotificationService notificationService, IHelperClass helper) {
        
            _notificationService = notificationService;
            _helper = helper;
    
          
        
        }

       


         public async Task InsertNotification(string message, int? from, int? to, int? ticketId, string Event, string type = "message")
        {
            
            var time = _helper.GetCurrentTime();

            var newNotification = new Notification
            {
                Time = time,
                Message = message,
                From = from,
                To = to,
                TicketId = ticketId,
                Type = type,
                Event = Event
                
          
         
            };


            await _notificationService.InsertNotification(newNotification);



        }


        public async Task DeleteNotification(int? TicketId, int? To, string Event)
        {   
            await _notificationService.DeleteNotification(TicketId, To, Event);
        }


       











       
    }
}


