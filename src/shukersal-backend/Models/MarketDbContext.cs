﻿using Microsoft.EntityFrameworkCore;

namespace shukersal_backend.Models
{
    public class MarketDbContext : DbContext
    {
        public MarketDbContext() { }

        public MarketDbContext(DbContextOptions<MarketDbContext> options)
            : base(options)
        {

        }


        #region Store Models
        // *------------------------------------------------- Store Models --------------------------------------------------*
        public virtual DbSet<DiscountRule> DiscountRules { get; set; } = null!;
        public virtual DbSet<DiscountRuleBoolean> DiscountRuleBooleans { get; set; } = null!;
        public virtual DbSet<PurchaseRule> PurchaseRules { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<Store> Stores { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        #endregion

        #region Manager Models
        // *------------------------------------------------- Manager Models --------------------------------------------------*
        public virtual DbSet<StoreManager> StoreManagers { get; set; } = null!;
        public virtual DbSet<StorePermission> StorePermissions { get; set; } = null!;
        #endregion

        #region Event Models
        // *------------------------------------------------- Event Models --------------------------------------------------*
        public DbSet<Auction> Auctions { get; set; } = null!;
        public DbSet<AuctionBid> AuctionBids { get; set; } = null!;
        public DbSet<Raffle> Raffles { get; set; } = null!;
        public DbSet<RaffleBid> RaffleBids { get; set; } = null!;
        #endregion

        #region Member Models
        // *------------------------------------------------- Member Models --------------------------------------------------*
        public virtual DbSet<Member> Members { get; set; } = null!;
        #endregion

        #region Notification Models
        // *------------------------------------------------- Notification Models --------------------------------------------------*
        virtual public DbSet<Notification> Notifications { get; set; } = null!;
        #endregion

        #region Transaction Models
        // *------------------------------------------------- Transaction Models --------------------------------------------------*
        virtual public DbSet<Transaction> Transactions { get; set; } = null!;
        virtual public DbSet<TransactionItem> TransactionItems { get; set; } = null!;
        #endregion

        #region Review Models
        // *------------------------------------------------- Review Models --------------------------------------------------*
        public DbSet<Review> Reviews { get; set; } = null!;
        public DbSet<Comment> Comments { get; set; } = null!;
        #endregion

        #region Shopping Cart
        // *------------------------------------------------- Shopping Cart Models --------------------------------------------------*
        public virtual DbSet<ShoppingCart> ShoppingCarts { get; set; } = null!;
        public virtual DbSet<ShoppingBasket> ShoppingBaskets { get; set; } = null!;
        public virtual DbSet<ShoppingItem> ShoppingItems { get; set; } = null!;
        #endregion


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Store>()
                .HasMany(s => s.DiscountRules)
                .WithOne()
                .HasForeignKey(dr => dr.StoreId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Store>()
                .HasMany(s => s.DiscountRules)
                .WithOne()
                .HasForeignKey(dr => dr.StoreId)
                .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<DiscountRuleBoolean>()
            //    .HasIndex(drb => drb.RootDiscountId)
            //    .IsUnique(false);

            modelBuilder.Entity<DiscountRule>()
                .HasOne(dr => dr.discountRuleBoolean)
                .WithOne()
                .HasForeignKey<DiscountRuleBoolean>(drb => drb.RootDiscountId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Store>()
                .HasMany(s => s.PurchaseRules)
                .WithOne()
                .HasForeignKey(dr => dr.StoreId)
                .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<DiscountRuleBoolean>()
            //    .HasMany(dr => dr.Components)
            //    .WithOne()
            //    .OnDelete(DeleteBehavior.Cascade);





            base.OnModelCreating(modelBuilder);

            var adminCart = new ShoppingCart { Id = 1, MemberId = 1, ShoppingBaskets = new List<ShoppingBasket>() };

            var admin = new Member
            {
                Username = "Admin",
                PasswordHash = Utility.HashingUtilities.HashPassword("password"),
                Role = "Administrator",
                Id = 1,
                //ShoppingCart = adminCart
            };
            //adminCart.Member = admin;
            modelBuilder.Entity<Member>().HasData(admin);
            modelBuilder.Entity<ShoppingCart>().HasData(adminCart);

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

        public virtual void MarkAsModified(Store store)
        {
            Entry(store).State = EntityState.Modified;
        }

        public virtual void MarkAsModified(Product product)
        {
            Entry(product).State = EntityState.Modified;
        }
    }
}
