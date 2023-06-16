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

            // Not In Use
            //// We manually manage the deleting of the comments TODO: Manually delete comments
            //modelBuilder.Entity<Comment>()
            //    .HasOne(x => x.ParentComment)
            //    .WithMany(x => x.ChildComments)
            //    .HasForeignKey(x => x.ParentCommentId)
            //    .OnDelete(DeleteBehavior.NoAction);

            //// We manually manage the deleting of the managers TODO: Manually delete managers
            //modelBuilder.Entity<StoreManager>()
            //    .HasOne(x => x.ParentManager)
            //    .WithMany(x => x.ChildManagers)
            //    .HasForeignKey(x => x.ParentManagerId)
            //    .OnDelete(DeleteBehavior.NoAction);



            //foreach (var foreignKey in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            //{
            //    foreignKey.DeleteBehavior = DeleteBehavior.NoAction;
            //}
            //foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            //{
            //    var primaryKey = entityType.FindPrimaryKey();
            //    if (primaryKey != null)
            //    {
            //        foreach (var property in primaryKey.Properties)
            //        {
            //            // Set ValueGeneratedOnAdd for the primary key property of each entity
            //            property.ValueGenerated = ValueGenerated.OnAdd;
            //        }
            //    }
            //}

            //modelBuilder.Entity<Product>()
            //    .HasOne(p => p.Store)
            //    .WithMany()
            //    .HasForeignKey(p => p.StoreId)
            //    .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<StoreManager>()
            //    .HasOne(s => s.Store)
            //    .WithMany()
            //    .HasForeignKey(p => p.StoreId)
            //    .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<Store>()
            //    .HasMany(s => s.Products)
            //    .WithOne(p => p.Store)
            //    .OnDelete(DeleteBehavior.NoAction);

            //modelBuilder.Entity<Store>()
            //    .HasOne(s => s.RootManager)
            //    .WithOne(p => p.Store)
            //    .OnDelete(DeleteBehavior.NoAction);

            //modelBuilder.Entity<Member>()
            //    .HasOne(m => m.ShoppingCart)
            //    .WithOne(s => s.Member)
            //    .OnDelete(DeleteBehavior.NoAction);

            //modelBuilder.Entity<ShoppingCart>()
            //    .HasOne(m => m.Member)
            //    .WithOne(s => s.ShoppingCart)
            //    .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<PurchaseRule>()
            //    .HasOne<Store>()
            //    .WithMany()
            //    .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<DiscountRule>()
            //    .HasOne<Store>()
            //    .WithMany()
            //    .OnDelete(DeleteBehavior.Cascade);
            //modelBuilder.Entity<Store>()
            //    .HasMany(s => s.DiscountRules)
            //    .WithOne()
            //    .HasForeignKey(dr => dr.StoreId)
            //    .OnDelete(DeleteBehavior.Cascade);

            //modelBuilder.Entity<Store>()
            //    .HasMany(s => s.PurchaseRules)
            //    .WithOne()
            //    .HasForeignKey(pr => pr.StoreId)
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
