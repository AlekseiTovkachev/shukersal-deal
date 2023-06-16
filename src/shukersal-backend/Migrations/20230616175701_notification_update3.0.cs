﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace shukersal_backend.Migrations
{
    /// <inheritdoc />
    public partial class notification_update30 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_NotificationTypes_NotificationTypeId",
                table: "Notifications");

            migrationBuilder.DropTable(
                name: "NotificationTypes");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_NotificationTypeId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "NotificationTypeId",
                table: "Notifications");

            migrationBuilder.AddColumn<int>(
                name: "NotificationType",
                table: "Notifications",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 1L,
                column: "PasswordHash",
                value: "AMowDf23CA7KezPmWckLGr+Po5BQJ6liL6edIYcM19TPpqR6nYASuWoAfjpgCQYvsw==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotificationType",
                table: "Notifications");

            migrationBuilder.AddColumn<long>(
                name: "NotificationTypeId",
                table: "Notifications",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "NotificationTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationTypes", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 1L,
                column: "PasswordHash",
                value: "AMlpMoV1ZbtL+/rgxod+976KT3pcPeZbAEfD0in7xd9MTB3betR8tXqgb/AqenLjPw==");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_NotificationTypeId",
                table: "Notifications",
                column: "NotificationTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_NotificationTypes_NotificationTypeId",
                table: "Notifications",
                column: "NotificationTypeId",
                principalTable: "NotificationTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
