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
            migrationBuilder.DropIndex(
                name: "IX_DiscountRuleBooleans_RootDiscountId",
                table: "DiscountRuleBooleans");

            migrationBuilder.AlterColumn<long>(
                name: "RootDiscountId",
                table: "DiscountRuleBooleans",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 1L,
                column: "PasswordHash",
                value: "ANeAmz6iD6KRkEL065vLek2ilYcJvEML75lBOmNglrZNTu9x+MrNvPd1jGbt3wltBQ==");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountRuleBooleans_RootDiscountId",
                table: "DiscountRuleBooleans",
                column: "RootDiscountId",
                unique: true,
                filter: "[RootDiscountId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DiscountRuleBooleans_RootDiscountId",
                table: "DiscountRuleBooleans");

            migrationBuilder.AlterColumn<long>(
                name: "RootDiscountId",
                table: "DiscountRuleBooleans",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 1L,
                column: "PasswordHash",
                value: "ALjIOJWU4k2kmI3uivNcvjT6wtcuqP4byPbq2kouWvl96dueb1/bgts4xr2R8PANSg==");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountRuleBooleans_RootDiscountId",
                table: "DiscountRuleBooleans",
                column: "RootDiscountId",
                unique: true);
        }
    }
}
