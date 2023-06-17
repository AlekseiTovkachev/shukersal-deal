using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace shukersal_backend.Migrations
{
    /// <inheritdoc />
    public partial class transactionItemPost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 1L,
                column: "PasswordHash",
                value: "ABmdeX+c55rS/DUj4eGfRGkb6RedbOUGioYbbX4zn3atEVEkijAx/6Js9Ji2k67LCw==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 1L,
                column: "PasswordHash",
                value: "AH33tGt9qqR3sJjPJtkdGht4JKo9go3c+cSND3XsBwiXYEAoz/lo8rnXUusH0AsAog==");
        }
    }
}
