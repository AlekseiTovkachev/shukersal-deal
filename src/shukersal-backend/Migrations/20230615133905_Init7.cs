using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace shukersal_backend.Migrations
{
    /// <inheritdoc />
    public partial class Init7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Stores",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 1L,
                column: "PasswordHash",
                value: "AHi7CYCwBTLk4tIYlRaPPk2LFTb0uwCfF9/hrTUOR+qrCB0mSb6kxugiMSgndbqq5A==");

            migrationBuilder.CreateIndex(
                name: "IX_Stores_Name",
                table: "Stores",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Stores_Name",
                table: "Stores");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Stores",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 1L,
                column: "PasswordHash",
                value: "AEqpxmnx7v6OA5EwhohQepjH7jvcJxcigUKxLl0Xt1pEbkoCyxDMlrY4MaMUR2MQGw==");
        }
    }
}
