using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace shukersal_backend.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DiscountRuleBooleans",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    discountRuleBooleanType = table.Column<int>(type: "int", nullable: false),
                    conditionString = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    conditionLimit = table.Column<int>(type: "int", nullable: false),
                    minHour = table.Column<int>(type: "int", nullable: false),
                    maxHour = table.Column<int>(type: "int", nullable: false),
                    DiscountRuleBooleanId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountRuleBooleans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscountRuleBooleans_DiscountRuleBooleans_DiscountRuleBooleanId",
                        column: x => x.DiscountRuleBooleanId,
                        principalTable: "DiscountRuleBooleans",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NotificationTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RaffleBids",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RaffleBids", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Raffles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Raffles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberId = table.Column<long>(type: "bigint", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalPrice = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingCarts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCarts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoppingCarts_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    MemberId = table.Column<long>(type: "bigint", nullable: false),
                    NotificationTypeId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReadAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Notifications_NotificationTypes_NotificationTypeId",
                        column: x => x.NotificationTypeId,
                        principalTable: "NotificationTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransactionItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionId = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    StoreId = table.Column<long>(type: "bigint", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    FullPrice = table.Column<double>(type: "float", nullable: false),
                    FinalPrice = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionItems_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingBaskets",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShoppingCartId = table.Column<long>(type: "bigint", nullable: false),
                    StoreId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingBaskets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoppingBaskets_ShoppingCarts_ShoppingCartId",
                        column: x => x.ShoppingCartId,
                        principalTable: "ShoppingCarts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuctionBids",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    BidDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AuctionId = table.Column<long>(type: "bigint", nullable: false),
                    MemberId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuctionBids", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuctionBids_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Auctions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartingPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CurrentPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsSold = table.Column<bool>(type: "bit", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auctions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DiscountRules",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    discountType = table.Column<int>(type: "int", nullable: false),
                    Discount = table.Column<double>(type: "float", nullable: false),
                    discountRuleBooleanId = table.Column<long>(type: "bigint", nullable: true),
                    discountOn = table.Column<int>(type: "int", nullable: false),
                    discountOnString = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiscountRuleId = table.Column<long>(type: "bigint", nullable: true),
                    StoreId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscountRules_DiscountRuleBooleans_discountRuleBooleanId",
                        column: x => x.discountRuleBooleanId,
                        principalTable: "DiscountRuleBooleans",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DiscountRules_DiscountRules_DiscountRuleId",
                        column: x => x.DiscountRuleId,
                        principalTable: "DiscountRules",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsListed = table.Column<bool>(type: "bit", nullable: false),
                    UnitsInStock = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    StoreId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ShoppingItems",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShoppingBasketId = table.Column<long>(type: "bigint", nullable: false),
                    ProductId = table.Column<long>(type: "bigint", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoppingItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShoppingItems_ShoppingBaskets_ShoppingBasketId",
                        column: x => x.ShoppingBasketId,
                        principalTable: "ShoppingBaskets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseRules",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    purchaseRuleType = table.Column<int>(type: "int", nullable: false),
                    conditionString = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    conditionLimit = table.Column<int>(type: "int", nullable: false),
                    minHour = table.Column<int>(type: "int", nullable: false),
                    maxHour = table.Column<int>(type: "int", nullable: false),
                    PurchaseRuleId = table.Column<long>(type: "bigint", nullable: true),
                    StoreId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseRules_PurchaseRules_PurchaseRuleId",
                        column: x => x.PurchaseRuleId,
                        principalTable: "PurchaseRules",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StoreManagers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StoreId = table.Column<long>(type: "bigint", nullable: false),
                    MemberId = table.Column<long>(type: "bigint", nullable: false),
                    ParentManagerId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreManagers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreManagers_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StoreManagers_StoreManagers_ParentManagerId",
                        column: x => x.ParentManagerId,
                        principalTable: "StoreManagers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StorePermissions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PermissionType = table.Column<int>(type: "int", nullable: false),
                    StoreManagerId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StorePermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StorePermissions_StoreManagers_StoreManagerId",
                        column: x => x.StoreManagerId,
                        principalTable: "StoreManagers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Stores",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RootManagerId = table.Column<long>(type: "bigint", nullable: false),
                    AppliedDiscountRuleId = table.Column<long>(type: "bigint", nullable: true),
                    AppliedPurchaseRuleId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stores_DiscountRules_AppliedDiscountRuleId",
                        column: x => x.AppliedDiscountRuleId,
                        principalTable: "DiscountRules",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Stores_PurchaseRules_AppliedPurchaseRuleId",
                        column: x => x.AppliedPurchaseRuleId,
                        principalTable: "PurchaseRules",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Stores_StoreManagers_RootManagerId",
                        column: x => x.RootManagerId,
                        principalTable: "StoreManagers",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Electronics" },
                    { 2, "Clothing" },
                    { 3, "Home & Kitchen" },
                    { 4, "Sports & Outdoors" },
                    { 5, "Beauty & Personal Care" },
                    { 6, "Books" },
                    { 7, "Health & Wellness" },
                    { 8, "Automotive" },
                    { 9, "Toys & Games" },
                    { 10, "Furniture" },
                    { 11, "Food & Grocery" },
                    { 12, "Jewelry & Watches" },
                    { 13, "Baby & Nursery" },
                    { 14, "Tools & Home Improvement" },
                    { 15, "Pet Supplies" },
                    { 16, "Office & School Supplies" },
                    { 17, "Music & Instruments" },
                    { 18, "Movies & TV Shows" },
                    { 19, "Arts & Crafts" },
                    { 20, "Travel & Luggage" }
                });

            migrationBuilder.InsertData(
                table: "Members",
                columns: new[] { "Id", "PasswordHash", "Role", "Username" },
                values: new object[] { 1L, "AN/Z43c7smrB86EnswEy3TeRplStPYB7hc0ZOUUIXF4piqxT/hYkJyWA7JO329h6qg==", "Administrator", "Admin" });

            migrationBuilder.InsertData(
                table: "ShoppingCarts",
                columns: new[] { "Id", "MemberId" },
                values: new object[] { 1L, 1L });

            migrationBuilder.CreateIndex(
                name: "IX_AuctionBids_AuctionId",
                table: "AuctionBids",
                column: "AuctionId");

            migrationBuilder.CreateIndex(
                name: "IX_AuctionBids_MemberId",
                table: "AuctionBids",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Auctions_ProductId",
                table: "Auctions",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountRuleBooleans_DiscountRuleBooleanId",
                table: "DiscountRuleBooleans",
                column: "DiscountRuleBooleanId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountRules_discountRuleBooleanId",
                table: "DiscountRules",
                column: "discountRuleBooleanId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountRules_DiscountRuleId",
                table: "DiscountRules",
                column: "DiscountRuleId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountRules_StoreId",
                table: "DiscountRules",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_Members_Username",
                table: "Members",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_MemberId",
                table: "Notifications",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_NotificationTypeId",
                table: "Notifications",
                column: "NotificationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_StoreId",
                table: "Products",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRules_PurchaseRuleId",
                table: "PurchaseRules",
                column: "PurchaseRuleId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRules_StoreId",
                table: "PurchaseRules",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingBaskets_ShoppingCartId",
                table: "ShoppingBaskets",
                column: "ShoppingCartId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCarts_MemberId",
                table: "ShoppingCarts",
                column: "MemberId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingItems_ProductId",
                table: "ShoppingItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingItems_ShoppingBasketId",
                table: "ShoppingItems",
                column: "ShoppingBasketId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreManagers_MemberId",
                table: "StoreManagers",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreManagers_ParentManagerId",
                table: "StoreManagers",
                column: "ParentManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreManagers_StoreId",
                table: "StoreManagers",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_StorePermissions_StoreManagerId",
                table: "StorePermissions",
                column: "StoreManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Stores_AppliedDiscountRuleId",
                table: "Stores",
                column: "AppliedDiscountRuleId");

            migrationBuilder.CreateIndex(
                name: "IX_Stores_AppliedPurchaseRuleId",
                table: "Stores",
                column: "AppliedPurchaseRuleId");

            migrationBuilder.CreateIndex(
                name: "IX_Stores_RootManagerId",
                table: "Stores",
                column: "RootManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionItems_TransactionId",
                table: "TransactionItems",
                column: "TransactionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuctionBids_Auctions_AuctionId",
                table: "AuctionBids",
                column: "AuctionId",
                principalTable: "Auctions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Auctions_Products_ProductId",
                table: "Auctions",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DiscountRules_Stores_StoreId",
                table: "DiscountRules",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Stores_StoreId",
                table: "Products",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseRules_Stores_StoreId",
                table: "PurchaseRules",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StoreManagers_Stores_StoreId",
                table: "StoreManagers",
                column: "StoreId",
                principalTable: "Stores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StoreManagers_Members_MemberId",
                table: "StoreManagers");

            migrationBuilder.DropForeignKey(
                name: "FK_DiscountRules_DiscountRuleBooleans_discountRuleBooleanId",
                table: "DiscountRules");

            migrationBuilder.DropForeignKey(
                name: "FK_DiscountRules_Stores_StoreId",
                table: "DiscountRules");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseRules_Stores_StoreId",
                table: "PurchaseRules");

            migrationBuilder.DropForeignKey(
                name: "FK_StoreManagers_Stores_StoreId",
                table: "StoreManagers");

            migrationBuilder.DropTable(
                name: "AuctionBids");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "RaffleBids");

            migrationBuilder.DropTable(
                name: "Raffles");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "ShoppingItems");

            migrationBuilder.DropTable(
                name: "StorePermissions");

            migrationBuilder.DropTable(
                name: "TransactionItems");

            migrationBuilder.DropTable(
                name: "Auctions");

            migrationBuilder.DropTable(
                name: "NotificationTypes");

            migrationBuilder.DropTable(
                name: "ShoppingBaskets");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "ShoppingCarts");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropTable(
                name: "DiscountRuleBooleans");

            migrationBuilder.DropTable(
                name: "Stores");

            migrationBuilder.DropTable(
                name: "DiscountRules");

            migrationBuilder.DropTable(
                name: "PurchaseRules");

            migrationBuilder.DropTable(
                name: "StoreManagers");
        }
    }
}
