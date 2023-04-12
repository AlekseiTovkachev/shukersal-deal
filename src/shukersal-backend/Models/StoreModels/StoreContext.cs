using Microsoft.EntityFrameworkCore;

namespace shukersal_backend.Models
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options)
            : base(options)
        {

        }

        public DbSet<DiscountRule> DiscountRules { get; set; } = null!;
        public DbSet<DiscountType> DiscountTypes { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Store> Stores { get; set; } = null!;
    }
}
