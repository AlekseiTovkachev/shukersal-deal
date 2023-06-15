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
            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 1L,
                column: "PasswordHash",
                value: "AISMxxtOta8qqFZ31W6ZIycd6SNiozWahNDiiItYNR+aQHidM3g52PQqOkx0/OCgmw==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 1L,
                column: "PasswordHash",
                value: "AGOR2vnn74G1IgyRtTv3W17pTQnby2MGFxDigc/stQfNsITNZFWLPj+BIsAEM9hycw==");
        }
    }
}
