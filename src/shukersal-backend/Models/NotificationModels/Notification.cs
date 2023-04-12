namespace shukersal_backend.Models
{
    public class Notification
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public bool IsRead { get; set; }
        public long MemberId { get; set; }
        public long NotificationTypeId { get; set; }
        public Member Member { get; set; }
        public NotificationType NotificationType { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? ReadAt { get; set; }
    }
}
