using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace shukersal_backend.Migrations
{
    /// <inheritdoc />
    public partial class Init6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 1L,
                column: "PasswordHash",
                value: "AEqpxmnx7v6OA5EwhohQepjH7jvcJxcigUKxLl0Xt1pEbkoCyxDMlrY4MaMUR2MQGw==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 1L,
                column: "PasswordHash",
                value: "AKD0bWCFCMQFDi2LHYkV2wIDMo9AtlL/A/oSzibvSI3RpeOCtO9x8t7eyT49PiqBbw==");
        }
    }
}
