using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class passwordSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("13cd05bf-d06d-4613-b236-21e9d53fa500"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("75bcaff1-ee56-4c60-99cb-824ed56b50eb"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("7ba94656-dd26-45bc-9c94-3327f8b791de"), null, "It's role User", "User", "USER" },
                    { new Guid("867e8ec6-b5fe-4512-9211-c3f613b79d0d"), null, "It's role Admin", "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7ba94656-dd26-45bc-9c94-3327f8b791de"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("867e8ec6-b5fe-4512-9211-c3f613b79d0d"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("13cd05bf-d06d-4613-b236-21e9d53fa500"), null, "It's role User", "User", "USER" },
                    { new Guid("75bcaff1-ee56-4c60-99cb-824ed56b50eb"), null, "It's role Admin", "Admin", "ADMIN" }
                });
        }
    }
}
