using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StoreApi.Migrations
{
    /// <inheritdoc />
    public partial class ConfigurationSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Carts",
                columns: new[] { "CustomerId", "Id", "ProductId", "Quantity" },
                values: new object[] { new Guid("860c06fb-fd88-4661-bbba-9afefe9ccc3f"), new Guid("7d15ac84-2c94-4ad9-afd3-d5a7d2a9fe1c"), new Guid("886b55ae-3dd1-44fb-992c-20ef034134bf"), 0 });

            migrationBuilder.InsertData(
                table: "OrderItems",
                columns: new[] { "Id", "OrderId", "Price", "ProductId", "Quantity" },
                values: new object[,]
                {
                    { new Guid("1aa7b22d-c50f-477e-93df-822e2ad2c264"), new Guid("0aa7b22d-c50f-477e-93df-822e2ad2c264"), 0, new Guid("17892431-77a6-4a94-9fac-70a05552aa8f"), 0 },
                    { new Guid("2aaf2d9b-2007-4798-be1c-115c852c2ffe"), new Guid("0aa7b22d-c50f-477e-93df-822e2ad2c264"), 0, new Guid("886b55ae-3dd1-44fb-992c-20ef034134bf"), 0 },
                    { new Guid("3aaf2d9b-2007-4798-be1c-115c852c2ffe"), new Guid("10af2d9b-2007-4798-be1c-115c852c2ffe"), 0, new Guid("17892431-77a6-4a94-9fac-70a05552aa8f"), 0 },
                    { new Guid("4aaf2d9b-2007-4798-be1c-115c852c2ffe"), new Guid("10af2d9b-2007-4798-be1c-115c852c2ffe"), 0, new Guid("ae450822-2ddf-4ca3-8476-8849b6041939"), 0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Carts",
                keyColumns: new[] { "CustomerId", "Id" },
                keyValues: new object[] { new Guid("860c06fb-fd88-4661-bbba-9afefe9ccc3f"), new Guid("7d15ac84-2c94-4ad9-afd3-d5a7d2a9fe1c") });

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumns: new[] { "Id", "OrderId" },
                keyValues: new object[] { new Guid("1aa7b22d-c50f-477e-93df-822e2ad2c264"), new Guid("0aa7b22d-c50f-477e-93df-822e2ad2c264") });

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumns: new[] { "Id", "OrderId" },
                keyValues: new object[] { new Guid("2aaf2d9b-2007-4798-be1c-115c852c2ffe"), new Guid("0aa7b22d-c50f-477e-93df-822e2ad2c264") });

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumns: new[] { "Id", "OrderId" },
                keyValues: new object[] { new Guid("3aaf2d9b-2007-4798-be1c-115c852c2ffe"), new Guid("10af2d9b-2007-4798-be1c-115c852c2ffe") });

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumns: new[] { "Id", "OrderId" },
                keyValues: new object[] { new Guid("4aaf2d9b-2007-4798-be1c-115c852c2ffe"), new Guid("10af2d9b-2007-4798-be1c-115c852c2ffe") });
        }
    }
}
