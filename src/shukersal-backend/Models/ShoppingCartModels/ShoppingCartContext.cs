using Microsoft.EntityFrameworkCore;

namespace shukersal_backend.Models.ShoppingCartModels
{
    public class ShoppingCartContext : DbContext
    {
        public ShoppingCartContext(DbContextOptions<ShoppingCartContext> options)
            : base(options)
        {

        }

        public DbSet<ShoppingCart> ShoppingCarts { get; set; } = null!;
        public DbSet<ShoppingBasket> ShoppingBaskets { get; set; } = null!;
        public DbSet<ShoppingItem> ShoppingItems { get; set; } = null!;

        // TODO: Test the following code
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    modelBuilder.Entity<ShoppingCart>()
        //        .HasMany(c => c.ShoppingBaskets)
        //        .WithOne(b => b.ShoppingCart)
        //        .OnDelete(DeleteBehavior.Cascade);

        //    modelBuilder.Entity<ShoppingBasket>()
        //        .HasMany(b => b.ShoppingItems)
        //        .WithOne(i => i.ShoppingBasket)
        //        .OnDelete(DeleteBehavior.Cascade);
        //}
    }
}
