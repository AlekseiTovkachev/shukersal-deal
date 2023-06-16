namespace shukersal_backend.Models
{


    public enum NotificationType
    {
        ProductPurchased = 1,
        RemovedFromStore = 2,
        AddedToStore = 3
    }

    public class Notification
    {
        public long Id { get; set; }
        public string Message { get; set; }
        public long MemberId { get; set; }
        public NotificationType NotificationType { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
