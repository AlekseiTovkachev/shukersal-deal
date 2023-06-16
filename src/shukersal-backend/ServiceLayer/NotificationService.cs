using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using shukersal_backend.Models;
using Microsoft.AspNetCore.SignalR;
using shukersal_backend.DomainLayer.notifications;
using shukersal_backend.DomainLayer.Controllers;

namespace shukersal_backend.ServiceLayer
{
    [Route("api/[controller]")]
    public class NotificationService : ControllerBase
    {

        private readonly NotificationController _notificationController;
        private readonly ILogger<ControllerBase> logger;
        public NotificationService(NotificationController notificationController, ILogger<ControllerBase> logger)
        {
            _notificationController = notificationController;
            this.logger = logger;
        }



        [HttpPost]
        [Route("Notifications/member/{memberId}")]
        public async Task<IActionResult> SendNotification(long memberId, NotificationType notificationType, string message)
        {
            if (ModelState.IsValid)
            {

                var response = await _notificationController.SendNotificationToUser(memberId, notificationType, message);
                if (!response.IsSuccess || response.Result == null)
                {
                    return BadRequest(response.ErrorMessage);
                }
                return Ok(response.Result);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }


        [HttpGet("Notifications/member/{memberId}")]
        public async Task<ActionResult<IEnumerable<Notification>>> GetNotificationsById(long memberId)
        {
            var response = await _notificationController.GetNotificationsById(memberId);
            if (!response.IsSuccess)
            {
                return NotFound();
            }
            return Ok(response.Result);
        }


        [HttpDelete("Noifications/{notificationId}")]
        public async Task<IActionResult> DeleteNotificationsById(long notificationId)
        {
            var response = await _notificationController.DeleteNotificationById(notificationId);
            if (!response.IsSuccess)
            {
                return NotFound(response.ErrorMessage);
            }
            return Ok(response.Result);
        }

        [HttpDelete("Notifications/member/{id}")]
        public async Task<IActionResult> DeleteAllNotificationsUserId(long id)
        {
            var response = await _notificationController.DeleteAllNotificationsUserId(id);
            if (!response.IsSuccess)
            {
                return NotFound(response.ErrorMessage);
            }
            return Ok(response.Result);
        }

    }
}
