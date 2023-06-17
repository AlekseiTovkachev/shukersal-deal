using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace shukersal_backend.Migrations
{
    /// <inheritdoc />
    public partial class Init4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParentBooleanId",
                table: "DiscountRuleBooleans");

            migrationBuilder.AddColumn<bool>(
                name: "IsRoot",
                table: "DiscountRuleBooleans",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 1L,
                column: "PasswordHash",
                value: "AO7/MZIh21UC2VHmiaArzoA2uOVSHrDHheS2nXVhFjVcZ30c5fi7mXp6OlzzbGxDyw==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRoot",
                table: "DiscountRuleBooleans");

            migrationBuilder.AddColumn<long>(
                name: "ParentBooleanId",
                table: "DiscountRuleBooleans",
                type: "bigint",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 1L,
                column: "PasswordHash",
                value: "AA5rPysPmyfug3rHIUmk9iwUOeZ5rGXhMSg9dwo/Z9EXvHPn+e9+EMVufW2BVd50/g==");
        }
    }
}
