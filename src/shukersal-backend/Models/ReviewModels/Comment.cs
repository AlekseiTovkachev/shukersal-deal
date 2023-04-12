namespace shukersal_backend.Models
{
    public class Comment
    {
        public long Id { get; set; }
        public long ReviewId { get; set; }
        public Review Review { get; set; }
        public long? ParentCommentId { get; set; }
        public Comment ParentComment { get; set; }
        public long MemberId { get; set; }
        public Member Member { get; set; }
        public string Body { get; set; }
        public DateTime Date { get; set; }
        public ICollection<Comment> ChildComments { get; set; }
    }
}
