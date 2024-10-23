using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreApi.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTotalPriceColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Products",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "OrderItems",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumns: new[] { "Id", "OrderId" },
                keyValues: new object[] { new Guid("1aa7b22d-c50f-477e-93df-822e2ad2c264"), new Guid("0aa7b22d-c50f-477e-93df-822e2ad2c264") },
                column: "Price",
                value: 29.99m);

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumns: new[] { "Id", "OrderId" },
                keyValues: new object[] { new Guid("2aaf2d9b-2007-4798-be1c-115c852c2ffe"), new Guid("0aa7b22d-c50f-477e-93df-822e2ad2c264") },
                column: "Price",
                value: 19.99m);

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumns: new[] { "Id", "OrderId" },
                keyValues: new object[] { new Guid("3aaf2d9b-2007-4798-be1c-115c852c2ffe"), new Guid("10af2d9b-2007-4798-be1c-115c852c2ffe") },
                column: "Price",
                value: 29.99m);

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumns: new[] { "Id", "OrderId" },
                keyValues: new object[] { new Guid("4aaf2d9b-2007-4798-be1c-115c852c2ffe"), new Guid("10af2d9b-2007-4798-be1c-115c852c2ffe") },
                column: "Price",
                value: 9.99m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Products",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "Price",
                table: "OrderItems",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "TEXT");

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumns: new[] { "Id", "OrderId" },
                keyValues: new object[] { new Guid("1aa7b22d-c50f-477e-93df-822e2ad2c264"), new Guid("0aa7b22d-c50f-477e-93df-822e2ad2c264") },
                column: "Price",
                value: 0);

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumns: new[] { "Id", "OrderId" },
                keyValues: new object[] { new Guid("2aaf2d9b-2007-4798-be1c-115c852c2ffe"), new Guid("0aa7b22d-c50f-477e-93df-822e2ad2c264") },
                column: "Price",
                value: 0);

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumns: new[] { "Id", "OrderId" },
                keyValues: new object[] { new Guid("3aaf2d9b-2007-4798-be1c-115c852c2ffe"), new Guid("10af2d9b-2007-4798-be1c-115c852c2ffe") },
                column: "Price",
                value: 0);

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumns: new[] { "Id", "OrderId" },
                keyValues: new object[] { new Guid("4aaf2d9b-2007-4798-be1c-115c852c2ffe"), new Guid("10af2d9b-2007-4798-be1c-115c852c2ffe") },
                column: "Price",
                value: 0);
        }
    }
}
