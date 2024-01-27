using backEnd.Models;
using backEnd.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using backEnd.Services.IServices;
using backEnd.Helpers.IHelpers;
using backEnd.Mappings;

namespace backend.Controllers
{

    [ApiController]
    [Route("/")]
    public class NotificationController : Controller
    {
        INotificationService _notificationService;

        
     
        public NotificationController(INotificationService notificationService) {

            _notificationService = notificationService;
           
        }


        [HttpPost]
        [Route("/getNotifications")]
        public async Task<IActionResult> GetNotifications(IFormCollection data)
        {

            var user = JsonSerializer.Deserialize<User>(data["user"]);
            var result = await _notificationService.GetNotificationsByUser(user.Id);

            return Ok(result);
                
        }


   


      


     




        [HttpPost]
        [Route("/deleteNotification")]
        public async Task<IActionResult> DeleteNotification(IFormCollection data)
        {
           
            var id = data["id"];


            

            await _notificationService.RemoveNotification(int.Parse(id));


            return Ok(true);
        }
    }
}
