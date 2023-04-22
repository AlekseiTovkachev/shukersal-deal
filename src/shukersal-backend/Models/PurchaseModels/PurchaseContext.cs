using Microsoft.EntityFrameworkCore;

namespace shukersal_backend.Models
{
    public class PurchaseContext : DbContext
    {
        public PurchaseContext(DbContextOptions<PurchaseContext> options)
            : base(options)
        {

        }
        public DbSet<Purchase> Purchases { get; set; } = null!;
        public DbSet<PurchaseItem> PurchaseItems { get; set; } = null!;

    }
}
