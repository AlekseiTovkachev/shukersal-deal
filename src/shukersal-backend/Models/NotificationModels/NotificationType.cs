namespace shukersal_backend.Models
{
    public class NotificationType
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public ICollection<Notification> Notifications { get; set; }
    }
}
