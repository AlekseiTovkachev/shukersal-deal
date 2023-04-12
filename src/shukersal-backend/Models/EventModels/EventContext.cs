using Microsoft.EntityFrameworkCore;

namespace shukersal_backend.Models
{
    public class EventContext : DbContext
    {
        public EventContext(DbContextOptions<EventContext> options)
            : base(options)
        {

        }

        public DbSet<Auction> Auctions { get; set; } = null!;
        public DbSet<AuctionBid> AuctionBids { get; set; } = null!;
        public DbSet<Raffle> Raffles { get; set; } = null!;
        public DbSet<RaffleBid> RaffleBids { get; set; } = null!;
    }
}
