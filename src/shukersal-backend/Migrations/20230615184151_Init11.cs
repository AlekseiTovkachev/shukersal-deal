using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace shukersal_backend.Migrations
{
    /// <inheritdoc />
    public partial class Init11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 1L,
                column: "PasswordHash",
                value: "AEHE87GGp4BrukGD/K9cgE5IDyUeUjh8TFZh/Law/h4905m+NJctozhtkXXkRipS5Q==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 1L,
                column: "PasswordHash",
                value: "APCgxLDNHp9M8YocPhHKAvn2FoRuvH0VUBp+ghMj8NQjfpwZfnC6MOW85Be4Ya2bPQ==");
        }
    }
}
