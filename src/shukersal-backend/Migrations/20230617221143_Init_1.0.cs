using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace shukersal_backend.Migrations
{
    /// <inheritdoc />
    public partial class Init_10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 1L,
                column: "PasswordHash",
                value: "ADWVEsYuWKNhvdwgLdCdRVgBA/8EaKx7TDPsmbyPGaCujcRGb2qDjMlPLn+OQ+geZA==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 1L,
                column: "PasswordHash",
                value: "ABvWNvZTAGXOm4HLAoAjpE/jLSYhpO225EQniRq1y0YeSI1CLqc1OsAad0PXOc6/hA==");
        }
    }
}
