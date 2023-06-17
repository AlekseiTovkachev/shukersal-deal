using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using shukersal_backend.Utility;
using shukersal_backend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace shukersal_backend.DomainLayer.notifications
{
    public class NotificationController
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly static ConcurrentDictionary<long, string> _connectedUsers = new ConcurrentDictionary<long, string>();
        private readonly MarketDbContext _marketContext;
        private readonly ILogger<ControllerBase> logger;

        public NotificationController(IHubContext<NotificationHub> hubContext, MarketDbContext marketContext,ILogger<ControllerBase> logger)
        {
            _hubContext = hubContext;
            //_connectedUsers = new ConcurrentDictionary<long, string>();
            _marketContext = marketContext;
        }

        public async Task<Response<string>> SendNotificationToUser(long recipientId, NotificationType notificationType, string message)
        {
            var notificationPost = new Notification
            {
                Message = message,
                MemberId = recipientId,
                NotificationType = notificationType,
                CreatedAt = DateTime.Now
            };
            _marketContext.Notifications.Add(notificationPost);
            await _marketContext.SaveChangesAsync();

            if (_connectedUsers.TryGetValue(recipientId, out string connectionId))
            {
                var json = JsonSerializer.Serialize(notificationPost);  // Serialize the notificationPost object to JSON
                await _hubContext.Clients.Client(connectionId).SendAsync("ReceiveNotification", json);
                return Response<string>.Success(HttpStatusCode.OK, "notification sent");
            }

            return Response<string>.Error(HttpStatusCode.NotFound, "User is not connected. They will see the notification when they log in.");
        }

        public void MapUserToConnection(long id, string connectionId)
        {
            Console.WriteLine("got map connection");
            _connectedUsers.AddOrUpdate(id, connectionId, (key, value) => connectionId);
        }


        public void RemoveUserConnection(string connectionId)
        {
            var userId = GetUserId(connectionId);
            if (userId != 0)
            {
                _connectedUsers.TryRemove(userId, out _);
            }
        }


        public async Task<Response<IEnumerable<Notification>>> GetNotificationsById(long memberId)
        {
            var notifications = await _marketContext.Notifications
                .Where(n => n.MemberId == memberId)
                .ToListAsync();
            return Response<IEnumerable<Notification>>.Success(HttpStatusCode.OK, notifications);
        }


        private long GetUserId(string connectionId)
        {
            foreach (var userConnection in _connectedUsers)
            {
                if (userConnection.Value == connectionId)
                {
                    return userConnection.Key;
                }
            }

            return 0; // Or any other default value indicating user ID not found
        }

        public async Task<Response<string>> DeleteNotificationById(long id)
        {
            if (_marketContext.Notifications == null)
            {
                return Response<string>.Error(HttpStatusCode.NotFound, "Entity set 'nitifications' is null.");
            }

            var notification  = await _marketContext.Notifications.FindAsync(id);
            if (notification == null)
            {
                return Response<string>.Error(HttpStatusCode.NotFound, "notification is not found");
            }

            _marketContext.Notifications.Remove(notification);

            await _marketContext.SaveChangesAsync();

            return Response<string>.Success(HttpStatusCode.NoContent, "delete was successful");
        }


        public async Task<Response<string>> DeleteAllNotificationsUserId(long memberId)
        {
            var notifications = await _marketContext.Notifications
                .Where(n => n.MemberId == memberId)
                .ToListAsync();

            if (notifications.Count == 0)
            {
                return Response<string>.Error(HttpStatusCode.NotFound, "No notifications found for the specified memberId.");
            }

            _marketContext.Notifications.RemoveRange(notifications);

            await _marketContext.SaveChangesAsync();

            return Response<string>.Success(HttpStatusCode.NoContent, "Deletion of notifications was successful.");
        }

        public async Task<Response<string>> SendBulkNotifications(List<Tuple<long, string>> notificationList, NotificationType notificationType)
        {
            foreach (var notification in notificationList)
            {
                await SendNotificationToUser(notification.Item1, notificationType, notification.Item2);
            }

            return Response<string>.Success(HttpStatusCode.OK, "Bulk notifications sent successfully.");
        }


    }
}
