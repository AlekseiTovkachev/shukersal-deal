namespace shukersal_backend.Models
{
    public class AuctionBid
    {
        public long Id { get; set; }
        public decimal BidAmount { get; set; }
        public DateTime BidDateTime { get; set; }

        public long AuctionId { get; set; }
        public virtual Auction Auction { get; set; }

        public long MemberId { get; set; }
        public virtual Member Member { get; set; }
    }
}
