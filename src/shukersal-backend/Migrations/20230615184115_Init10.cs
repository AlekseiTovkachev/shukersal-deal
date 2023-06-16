using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace shukersal_backend.Migrations
{
    /// <inheritdoc />
    public partial class Init10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 1L,
                column: "PasswordHash",
                value: "APCgxLDNHp9M8YocPhHKAvn2FoRuvH0VUBp+ghMj8NQjfpwZfnC6MOW85Be4Ya2bPQ==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 1L,
                column: "PasswordHash",
                value: "AP08PsORkhEbpbWRgl4FTgGnVWxeEsP5j1euQYU09xQ2GLG9KQCPQRWwEcOf2nTzxg==");
        }
    }
}
