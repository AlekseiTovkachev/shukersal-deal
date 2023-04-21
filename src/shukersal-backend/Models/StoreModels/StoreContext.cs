using Microsoft.EntityFrameworkCore;

namespace shukersal_backend.Models
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options)
            : base(options)
        {

        }

        public StoreContext()
        {

        }

        public virtual DbSet<DiscountRule> DiscountRules { get; set; } = null!;
        public virtual DbSet<DiscountType> DiscountTypes { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<Store> Stores { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Add categories to Categories table
            modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Electronics" },
            new Category { Id = 2, Name = "Clothing" },
            new Category { Id = 3, Name = "Home & Kitchen" },
            new Category { Id = 4, Name = "Sports & Outdoors" },
            new Category { Id = 5, Name = "Beauty & Personal Care" },
            new Category { Id = 6, Name = "Books" },
            new Category { Id = 7, Name = "Health & Wellness" },
            new Category { Id = 8, Name = "Automotive" },
            new Category { Id = 9, Name = "Toys & Games" },
            new Category { Id = 10, Name = "Furniture" },
            new Category { Id = 11, Name = "Food & Grocery" },
            new Category { Id = 12, Name = "Jewelry & Watches" },
            new Category { Id = 13, Name = "Baby & Nursery" },
            new Category { Id = 14, Name = "Tools & Home Improvement" },
            new Category { Id = 15, Name = "Pet Supplies" },
            new Category { Id = 16, Name = "Office & School Supplies" },
            new Category { Id = 17, Name = "Music & Instruments" },
            new Category { Id = 18, Name = "Movies & TV Shows" },
            new Category { Id = 19, Name = "Arts & Crafts" },
            new Category { Id = 20, Name = "Travel & Luggage" }
            );
        }

    }
}
