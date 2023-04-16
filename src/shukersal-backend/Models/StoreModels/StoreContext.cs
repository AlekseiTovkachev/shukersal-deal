using Microsoft.EntityFrameworkCore;

namespace shukersal_backend.Models
{
    public class StoreContext : DbContext
    {
        public StoreContext()
        {

        }
        public StoreContext(DbContextOptions<StoreContext> options)
            : base(options)
        {

        }

        public virtual DbSet<DiscountRule> DiscountRules { get; set; } = null!;
        public virtual DbSet<DiscountType> DiscountTypes { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<Store> Stores { get; set; } = null!;
    }
}
