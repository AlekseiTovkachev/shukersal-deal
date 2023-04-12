namespace shukersal_backend.Models
{
    public class Auction
    {
        public long Id { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public decimal StartingPrice { get; set; }
        public decimal CurrentPrice { get; set; }
        public bool IsSold { get; set; }

        public long ProductId { get; set; }
        public virtual Product Product { get; set; }

        public virtual ICollection<AuctionBid> Bids { get; set; }
    }
}
