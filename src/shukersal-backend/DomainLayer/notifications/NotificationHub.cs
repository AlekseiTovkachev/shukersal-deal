using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.Language.Intermediate;
using Microsoft.AspNetCore.SignalR;
using shukersal_backend.DomainLayer.notifications;
using System.Drawing.Printing;
using System.Globalization;

namespace shukersal_backend.DomainLayer.notifications
{
    public class NotificationHub : Hub
    {
        private readonly NotificationController _notificationController;

        public NotificationHub(NotificationController notificationController)
        {
            _notificationController = notificationController;
        }

        public override async Task OnConnectedAsync()
        {
            //Console.WriteLine("got to the hub");
            //// Get the user ID and connection ID from the front end
            var connectionId = Context.ConnectionId;
            //if (long.TryParse(Context.GetHttpContext().Request.Query["userId"], out var userId))
            //{
            //    _notificationController.MapUserToConnection(userId, connectionId);
            //    await base.OnConnectedAsync();
            //}
            var userIdQueryParam = Context.GetHttpContext().Request.Query["userId"];
            Console.WriteLine($"userId query parameter: {userIdQueryParam}");

            if (long.TryParse(userIdQueryParam, out var userId))
            {
                _notificationController.MapUserToConnection(userId, connectionId);
                await base.OnConnectedAsync();
            }
            else
            {
                // Parsing failed, handle the error condition or additional formatting requirements
                Console.WriteLine("Failed to parse userId query parameter as long.");
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            // Get the connection ID
            var connectionId = Context.ConnectionId;
            // Remove the user's connection from the controller
            _notificationController.RemoveUserConnection(connectionId);

            await base.OnDisconnectedAsync(exception);
        }

    }
}
