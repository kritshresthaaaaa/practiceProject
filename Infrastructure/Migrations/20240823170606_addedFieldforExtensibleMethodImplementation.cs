using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedFieldforExtensibleMethodImplementation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7ba94656-dd26-45bc-9c94-3327f8b791de"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("867e8ec6-b5fe-4512-9211-c3f613b79d0d"));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfBirth",
                table: "AspNetUsers",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("396674ed-70e6-4ff6-bd76-90b1b53bd681"), null, "It's role Admin", "Admin", "ADMIN" },
                    { new Guid("647cdfdb-5ff6-4509-a878-316e52f7238d"), null, "It's role User", "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("396674ed-70e6-4ff6-bd76-90b1b53bd681"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("647cdfdb-5ff6-4509-a878-316e52f7238d"));

            migrationBuilder.DropColumn(
                name: "DateOfBirth",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("7ba94656-dd26-45bc-9c94-3327f8b791de"), null, "It's role User", "User", "USER" },
                    { new Guid("867e8ec6-b5fe-4512-9211-c3f613b79d0d"), null, "It's role Admin", "Admin", "ADMIN" }
                });
        }
    }
}
