using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace shukersal_backend.Migrations
{
    /// <inheritdoc />
    public partial class Init9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 1L,
                column: "PasswordHash",
                value: "AP08PsORkhEbpbWRgl4FTgGnVWxeEsP5j1euQYU09xQ2GLG9KQCPQRWwEcOf2nTzxg==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 1L,
                column: "PasswordHash",
                value: "AI4LqBpj4Ygy3UFOS/VWsK+62ljpWMr6AiAciS/beVzP53yDtyJg2+PkbxSAiQVFHQ==");
        }
    }
}
