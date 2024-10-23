using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderTotalPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "Orders");

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumns: new[] { "Id", "OrderId" },
                keyValues: new object[] { new Guid("1aa7b22d-c50f-477e-93df-822e2ad2c264"), new Guid("0aa7b22d-c50f-477e-93df-822e2ad2c264") },
                column: "Quantity",
                value: 3);

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumns: new[] { "Id", "OrderId" },
                keyValues: new object[] { new Guid("2aaf2d9b-2007-4798-be1c-115c852c2ffe"), new Guid("0aa7b22d-c50f-477e-93df-822e2ad2c264") },
                column: "Quantity",
                value: 2);

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumns: new[] { "Id", "OrderId" },
                keyValues: new object[] { new Guid("3aaf2d9b-2007-4798-be1c-115c852c2ffe"), new Guid("10af2d9b-2007-4798-be1c-115c852c2ffe") },
                column: "Quantity",
                value: 1);

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumns: new[] { "Id", "OrderId" },
                keyValues: new object[] { new Guid("4aaf2d9b-2007-4798-be1c-115c852c2ffe"), new Guid("10af2d9b-2007-4798-be1c-115c852c2ffe") },
                column: "Quantity",
                value: 4);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "Orders",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumns: new[] { "Id", "OrderId" },
                keyValues: new object[] { new Guid("1aa7b22d-c50f-477e-93df-822e2ad2c264"), new Guid("0aa7b22d-c50f-477e-93df-822e2ad2c264") },
                column: "Quantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumns: new[] { "Id", "OrderId" },
                keyValues: new object[] { new Guid("2aaf2d9b-2007-4798-be1c-115c852c2ffe"), new Guid("0aa7b22d-c50f-477e-93df-822e2ad2c264") },
                column: "Quantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumns: new[] { "Id", "OrderId" },
                keyValues: new object[] { new Guid("3aaf2d9b-2007-4798-be1c-115c852c2ffe"), new Guid("10af2d9b-2007-4798-be1c-115c852c2ffe") },
                column: "Quantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "OrderItems",
                keyColumns: new[] { "Id", "OrderId" },
                keyValues: new object[] { new Guid("4aaf2d9b-2007-4798-be1c-115c852c2ffe"), new Guid("10af2d9b-2007-4798-be1c-115c852c2ffe") },
                column: "Quantity",
                value: 0);

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: new Guid("0aa7b22d-c50f-477e-93df-822e2ad2c264"),
                column: "TotalPrice",
                value: 55.00m);

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: new Guid("10af2d9b-2007-4798-be1c-115c852c2ffe"),
                column: "TotalPrice",
                value: 125.00m);
        }
    }
}
