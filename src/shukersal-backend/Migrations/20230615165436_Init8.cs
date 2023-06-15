using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace shukersal_backend.Migrations
{
    /// <inheritdoc />
    public partial class Init8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 1L,
                column: "PasswordHash",
                value: "AI4LqBpj4Ygy3UFOS/VWsK+62ljpWMr6AiAciS/beVzP53yDtyJg2+PkbxSAiQVFHQ==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 1L,
                column: "PasswordHash",
                value: "AHi7CYCwBTLk4tIYlRaPPk2LFTb0uwCfF9/hrTUOR+qrCB0mSb6kxugiMSgndbqq5A==");
        }
    }
}
