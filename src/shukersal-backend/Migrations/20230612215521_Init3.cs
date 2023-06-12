using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace shukersal_backend.Migrations
{
    /// <inheritdoc />
    public partial class Init3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 1L,
                column: "PasswordHash",
                value: "AAzHbMVAV3Vq6/b3VidK+WNYvuHyguwm5rS1jjyaNIlL2bhGaceCUImyeavLoNNLpQ==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 1L,
                column: "PasswordHash",
                value: "ALyCzpxp4j5uYKEQCwQYvb6906QMYBdZdT7d5db8Hs3uYul/ETdnAJLpzKvKMkxkwA==");
        }
    }
}
