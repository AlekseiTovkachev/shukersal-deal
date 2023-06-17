using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace shukersal_backend.Migrations
{
    /// <inheritdoc />
    public partial class m1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Stores",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 1L,
                column: "PasswordHash",
                value: "AIE4gIzxSLvmXrfqGqKX59F6x9b4QJ5x6F9IBBGdcX5shnckbndN9z0NFKETpgJ+fw==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Stores",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 1L,
                column: "PasswordHash",
                value: "ACh/lRDJIhvYetpxSKty4xdICosUENRhK38iSBledxSoGKJewAzDmcyjeLDkgGNA7w==");
        }
    }
}
