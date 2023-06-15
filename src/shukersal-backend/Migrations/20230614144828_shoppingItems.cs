using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace shukersal_backend.Migrations
{
    /// <inheritdoc />
    public partial class shoppingItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 1L,
                column: "PasswordHash",
                value: "AFH3abgkBwz/pBf8fHfjk2FjmEdIGPRRMBEkFHJ000DqoXmb2OLlPQym9P5tnwv0hQ==");
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
