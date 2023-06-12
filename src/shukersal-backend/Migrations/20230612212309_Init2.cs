using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace shukersal_backend.Migrations
{
    /// <inheritdoc />
    public partial class Init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 1L,
                column: "PasswordHash",
                value: "ALyCzpxp4j5uYKEQCwQYvb6906QMYBdZdT7d5db8Hs3uYul/ETdnAJLpzKvKMkxkwA==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 1L,
                column: "PasswordHash",
                value: "AITM9AYqXZBM6qt6Qxv3CIZw4qrako0WVMKzYhUg4Rc476kyfKII0RXvfJ2aoYdPig==");
        }
    }
}
