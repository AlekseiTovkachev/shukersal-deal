using Microsoft.EntityFrameworkCore;

namespace shukersal_backend.Models
{
    public class NotificationContext : DbContext
    {
        public NotificationContext(DbContextOptions<NotificationContext> options)
            : base(options)
        {

        }

        public DbSet<Notification> Notifications { get; set; } = null!;
        public DbSet<NotificationType> NotificationTypes { get; set; } = null!;
    }
}
