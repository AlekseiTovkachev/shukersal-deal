﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using shukersal_backend.Models;

#nullable disable

namespace shukersal_backend.Migrations
{
    [DbContext(typeof(MarketDbContext))]
    [Migration("20230615165436_Init8")]
    partial class Init8
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("shukersal_backend.Models.Auction", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<decimal>("CurrentPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("EndDateTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsSold")
                        .HasColumnType("bit");

                    b.Property<long>("ProductId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("StartDateTime")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("StartingPrice")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.ToTable("Auctions");
                });

            modelBuilder.Entity("shukersal_backend.Models.AuctionBid", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long>("AuctionId")
                        .HasColumnType("bigint");

                    b.Property<decimal>("BidAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("BidDateTime")
                        .HasColumnType("datetime2");

                    b.Property<long>("MemberId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("AuctionId");

                    b.HasIndex("MemberId");

                    b.ToTable("AuctionBids");
                });

            modelBuilder.Entity("shukersal_backend.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Electronics"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Clothing"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Home & Kitchen"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Sports & Outdoors"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Beauty & Personal Care"
                        },
                        new
                        {
                            Id = 6,
                            Name = "Books"
                        },
                        new
                        {
                            Id = 7,
                            Name = "Health & Wellness"
                        },
                        new
                        {
                            Id = 8,
                            Name = "Automotive"
                        },
                        new
                        {
                            Id = 9,
                            Name = "Toys & Games"
                        },
                        new
                        {
                            Id = 10,
                            Name = "Furniture"
                        },
                        new
                        {
                            Id = 11,
                            Name = "Food & Grocery"
                        },
                        new
                        {
                            Id = 12,
                            Name = "Jewelry & Watches"
                        },
                        new
                        {
                            Id = 13,
                            Name = "Baby & Nursery"
                        },
                        new
                        {
                            Id = 14,
                            Name = "Tools & Home Improvement"
                        },
                        new
                        {
                            Id = 15,
                            Name = "Pet Supplies"
                        },
                        new
                        {
                            Id = 16,
                            Name = "Office & School Supplies"
                        },
                        new
                        {
                            Id = 17,
                            Name = "Music & Instruments"
                        },
                        new
                        {
                            Id = 18,
                            Name = "Movies & TV Shows"
                        },
                        new
                        {
                            Id = 19,
                            Name = "Arts & Crafts"
                        },
                        new
                        {
                            Id = 20,
                            Name = "Travel & Luggage"
                        });
                });

            modelBuilder.Entity("shukersal_backend.Models.Comment", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.HasKey("Id");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("shukersal_backend.Models.DiscountRule", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<double>("Discount")
                        .HasColumnType("float");

                    b.Property<long?>("DiscountRuleId")
                        .HasColumnType("bigint");

                    b.Property<long?>("StoreId")
                        .HasColumnType("bigint");

                    b.Property<int>("discountOn")
                        .HasColumnType("int");

                    b.Property<string>("discountOnString")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("discountRuleBooleanId")
                        .HasColumnType("bigint");

                    b.Property<int>("discountType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DiscountRuleId");

                    b.HasIndex("StoreId");

                    b.HasIndex("discountRuleBooleanId");

                    b.ToTable("DiscountRules");
                });

            modelBuilder.Entity("shukersal_backend.Models.DiscountRuleBoolean", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long?>("DiscountRuleBooleanId")
                        .HasColumnType("bigint");

                    b.Property<int>("conditionLimit")
                        .HasColumnType("int");

                    b.Property<string>("conditionString")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("discountRuleBooleanType")
                        .HasColumnType("int");

                    b.Property<int>("maxHour")
                        .HasColumnType("int");

                    b.Property<int>("minHour")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DiscountRuleBooleanId");

                    b.ToTable("DiscountRuleBooleans");
                });

            modelBuilder.Entity("shukersal_backend.Models.Member", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Members");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            PasswordHash = "AI4LqBpj4Ygy3UFOS/VWsK+62ljpWMr6AiAciS/beVzP53yDtyJg2+PkbxSAiQVFHQ==",
                            Role = "Administrator",
                            Username = "Admin"
                        });
                });

            modelBuilder.Entity("shukersal_backend.Models.Notification", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsRead")
                        .HasColumnType("bit");

                    b.Property<long>("MemberId")
                        .HasColumnType("bigint");

                    b.Property<long>("NotificationTypeId")
                        .HasColumnType("bigint");

                    b.Property<DateTime?>("ReadAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("MemberId");

                    b.HasIndex("NotificationTypeId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("shukersal_backend.Models.NotificationType", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("NotificationTypes");
                });

            modelBuilder.Entity("shukersal_backend.Models.Product", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<int?>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsListed")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<long>("StoreId")
                        .HasColumnType("bigint");

                    b.Property<int>("UnitsInStock")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("StoreId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("shukersal_backend.Models.PurchaseRule", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long?>("PurchaseRuleId")
                        .HasColumnType("bigint");

                    b.Property<long?>("StoreId")
                        .HasColumnType("bigint");

                    b.Property<int>("conditionLimit")
                        .HasColumnType("int");

                    b.Property<string>("conditionString")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("maxHour")
                        .HasColumnType("int");

                    b.Property<int>("minHour")
                        .HasColumnType("int");

                    b.Property<int>("purchaseRuleType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PurchaseRuleId");

                    b.HasIndex("StoreId");

                    b.ToTable("PurchaseRules");
                });

            modelBuilder.Entity("shukersal_backend.Models.Raffle", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.HasKey("Id");

                    b.ToTable("Raffles");
                });

            modelBuilder.Entity("shukersal_backend.Models.RaffleBid", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.HasKey("Id");

                    b.ToTable("RaffleBids");
                });

            modelBuilder.Entity("shukersal_backend.Models.Review", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.HasKey("Id");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("shukersal_backend.Models.ShoppingBasket", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long>("ShoppingCartId")
                        .HasColumnType("bigint");

                    b.Property<long>("StoreId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ShoppingCartId");

                    b.ToTable("ShoppingBaskets");
                });

            modelBuilder.Entity("shukersal_backend.Models.ShoppingCart", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long>("MemberId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("MemberId")
                        .IsUnique();

                    b.ToTable("ShoppingCarts");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            MemberId = 1L
                        });
                });

            modelBuilder.Entity("shukersal_backend.Models.ShoppingItem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long>("ProductId")
                        .HasColumnType("bigint");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<long>("ShoppingBasketId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("ShoppingBasketId");

                    b.ToTable("ShoppingItems");
                });

            modelBuilder.Entity("shukersal_backend.Models.Store", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long?>("AppliedDiscountRuleId")
                        .HasColumnType("bigint");

                    b.Property<long?>("AppliedPurchaseRuleId")
                        .HasColumnType("bigint");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<long?>("RootManagerId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("AppliedDiscountRuleId");

                    b.HasIndex("AppliedPurchaseRuleId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("RootManagerId");

                    b.ToTable("Stores");
                });

            modelBuilder.Entity("shukersal_backend.Models.StoreManager", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long>("MemberId")
                        .HasColumnType("bigint");

                    b.Property<long?>("ParentManagerId")
                        .HasColumnType("bigint");

                    b.Property<long>("StoreId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("MemberId");

                    b.HasIndex("ParentManagerId");

                    b.HasIndex("StoreId");

                    b.ToTable("StoreManagers");
                });

            modelBuilder.Entity("shukersal_backend.Models.StorePermission", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<int>("PermissionType")
                        .HasColumnType("int");

                    b.Property<long>("StoreManagerId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("StoreManagerId");

                    b.ToTable("StorePermissions");
                });

            modelBuilder.Entity("shukersal_backend.Models.Transaction", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long>("MemberId")
                        .HasColumnType("bigint");

                    b.Property<double>("TotalPrice")
                        .HasColumnType("float");

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("shukersal_backend.Models.TransactionItem", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<double>("FinalPrice")
                        .HasColumnType("float");

                    b.Property<double>("FullPrice")
                        .HasColumnType("float");

                    b.Property<string>("ProductDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("ProductId")
                        .HasColumnType("bigint");

                    b.Property<string>("ProductName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<long>("StoreId")
                        .HasColumnType("bigint");

                    b.Property<long>("TransactionId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("TransactionId");

                    b.ToTable("TransactionItems");
                });

            modelBuilder.Entity("shukersal_backend.Models.Auction", b =>
                {
                    b.HasOne("shukersal_backend.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");
                });

            modelBuilder.Entity("shukersal_backend.Models.AuctionBid", b =>
                {
                    b.HasOne("shukersal_backend.Models.Auction", "Auction")
                        .WithMany("Bids")
                        .HasForeignKey("AuctionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("shukersal_backend.Models.Member", "Member")
                        .WithMany()
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Auction");

                    b.Navigation("Member");
                });

            modelBuilder.Entity("shukersal_backend.Models.DiscountRule", b =>
                {
                    b.HasOne("shukersal_backend.Models.DiscountRule", null)
                        .WithMany("Components")
                        .HasForeignKey("DiscountRuleId");

                    b.HasOne("shukersal_backend.Models.Store", null)
                        .WithMany("DiscountRules")
                        .HasForeignKey("StoreId");

                    b.HasOne("shukersal_backend.Models.DiscountRuleBoolean", "discountRuleBoolean")
                        .WithMany()
                        .HasForeignKey("discountRuleBooleanId");

                    b.Navigation("discountRuleBoolean");
                });

            modelBuilder.Entity("shukersal_backend.Models.DiscountRuleBoolean", b =>
                {
                    b.HasOne("shukersal_backend.Models.DiscountRuleBoolean", null)
                        .WithMany("Components")
                        .HasForeignKey("DiscountRuleBooleanId");
                });

            modelBuilder.Entity("shukersal_backend.Models.Notification", b =>
                {
                    b.HasOne("shukersal_backend.Models.Member", "Member")
                        .WithMany()
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("shukersal_backend.Models.NotificationType", "NotificationType")
                        .WithMany("Notifications")
                        .HasForeignKey("NotificationTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Member");

                    b.Navigation("NotificationType");
                });

            modelBuilder.Entity("shukersal_backend.Models.Product", b =>
                {
                    b.HasOne("shukersal_backend.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId");

                    b.HasOne("shukersal_backend.Models.Store", "Store")
                        .WithMany("Products")
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Store");
                });

            modelBuilder.Entity("shukersal_backend.Models.PurchaseRule", b =>
                {
                    b.HasOne("shukersal_backend.Models.PurchaseRule", null)
                        .WithMany("Components")
                        .HasForeignKey("PurchaseRuleId");

                    b.HasOne("shukersal_backend.Models.Store", null)
                        .WithMany("PurchaseRules")
                        .HasForeignKey("StoreId");
                });

            modelBuilder.Entity("shukersal_backend.Models.ShoppingBasket", b =>
                {
                    b.HasOne("shukersal_backend.Models.ShoppingCart", null)
                        .WithMany("ShoppingBaskets")
                        .HasForeignKey("ShoppingCartId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("shukersal_backend.Models.ShoppingCart", b =>
                {
                    b.HasOne("shukersal_backend.Models.Member", "Member")
                        .WithOne("ShoppingCart")
                        .HasForeignKey("shukersal_backend.Models.ShoppingCart", "MemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Member");
                });

            modelBuilder.Entity("shukersal_backend.Models.ShoppingItem", b =>
                {
                    b.HasOne("shukersal_backend.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("shukersal_backend.Models.ShoppingBasket", "ShoppingBasket")
                        .WithMany("ShoppingItems")
                        .HasForeignKey("ShoppingBasketId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("ShoppingBasket");
                });

            modelBuilder.Entity("shukersal_backend.Models.Store", b =>
                {
                    b.HasOne("shukersal_backend.Models.DiscountRule", "AppliedDiscountRule")
                        .WithMany()
                        .HasForeignKey("AppliedDiscountRuleId");

                    b.HasOne("shukersal_backend.Models.PurchaseRule", "AppliedPurchaseRule")
                        .WithMany()
                        .HasForeignKey("AppliedPurchaseRuleId");

                    b.HasOne("shukersal_backend.Models.StoreManager", "RootManager")
                        .WithMany()
                        .HasForeignKey("RootManagerId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("AppliedDiscountRule");

                    b.Navigation("AppliedPurchaseRule");

                    b.Navigation("RootManager");
                });

            modelBuilder.Entity("shukersal_backend.Models.StoreManager", b =>
                {
                    b.HasOne("shukersal_backend.Models.Member", "Member")
                        .WithMany()
                        .HasForeignKey("MemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("shukersal_backend.Models.StoreManager", "ParentManager")
                        .WithMany("ChildManagers")
                        .HasForeignKey("ParentManagerId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("shukersal_backend.Models.Store", "Store")
                        .WithMany()
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Member");

                    b.Navigation("ParentManager");

                    b.Navigation("Store");
                });

            modelBuilder.Entity("shukersal_backend.Models.StorePermission", b =>
                {
                    b.HasOne("shukersal_backend.Models.StoreManager", "StoreManager")
                        .WithMany("StorePermissions")
                        .HasForeignKey("StoreManagerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("StoreManager");
                });

            modelBuilder.Entity("shukersal_backend.Models.TransactionItem", b =>
                {
                    b.HasOne("shukersal_backend.Models.Transaction", null)
                        .WithMany("TransactionItems")
                        .HasForeignKey("TransactionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("shukersal_backend.Models.Auction", b =>
                {
                    b.Navigation("Bids");
                });

            modelBuilder.Entity("shukersal_backend.Models.DiscountRule", b =>
                {
                    b.Navigation("Components");
                });

            modelBuilder.Entity("shukersal_backend.Models.DiscountRuleBoolean", b =>
                {
                    b.Navigation("Components");
                });

            modelBuilder.Entity("shukersal_backend.Models.Member", b =>
                {
                    b.Navigation("ShoppingCart")
                        .IsRequired();
                });

            modelBuilder.Entity("shukersal_backend.Models.NotificationType", b =>
                {
                    b.Navigation("Notifications");
                });

            modelBuilder.Entity("shukersal_backend.Models.PurchaseRule", b =>
                {
                    b.Navigation("Components");
                });

            modelBuilder.Entity("shukersal_backend.Models.ShoppingBasket", b =>
                {
                    b.Navigation("ShoppingItems");
                });

            modelBuilder.Entity("shukersal_backend.Models.ShoppingCart", b =>
                {
                    b.Navigation("ShoppingBaskets");
                });

            modelBuilder.Entity("shukersal_backend.Models.Store", b =>
                {
                    b.Navigation("DiscountRules");

                    b.Navigation("Products");

                    b.Navigation("PurchaseRules");
                });

            modelBuilder.Entity("shukersal_backend.Models.StoreManager", b =>
                {
                    b.Navigation("ChildManagers");

                    b.Navigation("StorePermissions");
                });

            modelBuilder.Entity("shukersal_backend.Models.Transaction", b =>
                {
                    b.Navigation("TransactionItems");
                });
#pragma warning restore 612, 618
        }
    }
}
