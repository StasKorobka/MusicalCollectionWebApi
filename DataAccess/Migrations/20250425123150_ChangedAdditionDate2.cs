using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ChangedAdditionDate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "AdditionDate",
                table: "UserCollections",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.UpdateData(
                table: "Playlists",
                keyColumn: "PlaylistId",
                keyValue: 1,
                column: "CreationDate",
                value: new DateTime(2025, 4, 25, 12, 31, 50, 591, DateTimeKind.Utc).AddTicks(7117));

            migrationBuilder.UpdateData(
                table: "UserCollections",
                keyColumn: "UserCollectionId",
                keyValue: 1,
                column: "AdditionDate",
                value: new DateTime(2025, 4, 25, 12, 31, 50, 591, DateTimeKind.Utc).AddTicks(3951));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "CreationDate",
                value: new DateTime(2025, 4, 25, 12, 31, 50, 591, DateTimeKind.Utc).AddTicks(3522));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "AdditionDate",
                table: "UserCollections",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.UpdateData(
                table: "Playlists",
                keyColumn: "PlaylistId",
                keyValue: 1,
                column: "CreationDate",
                value: new DateTime(2025, 4, 25, 12, 26, 27, 984, DateTimeKind.Utc).AddTicks(8535));

            migrationBuilder.UpdateData(
                table: "UserCollections",
                keyColumn: "UserCollectionId",
                keyValue: 1,
                column: "AdditionDate",
                value: new DateOnly(2025, 4, 25));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                column: "CreationDate",
                value: new DateTime(2025, 4, 25, 12, 26, 27, 984, DateTimeKind.Utc).AddTicks(4957));
        }
    }
}
