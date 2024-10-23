﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StoreApi.Infrastructure;

#nullable disable

namespace StoreApi.Migrations
{
    [DbContext(typeof(StoreContext))]
    [Migration("20241023125319_RemoveTotalPriceColumns")]
    partial class RemoveTotalPriceColumns
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.10");

            modelBuilder.Entity("StoreApi.Entities.Cart", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("TEXT");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id", "CustomerId");

                    b.HasIndex("CustomerId");

                    b.HasIndex("ProductId");

                    b.ToTable("Carts");

                    b.HasData(
                        new
                        {
                            Id = new Guid("7d15ac84-2c94-4ad9-afd3-d5a7d2a9fe1c"),
                            CustomerId = new Guid("860c06fb-fd88-4661-bbba-9afefe9ccc3f"),
                            ProductId = new Guid("886b55ae-3dd1-44fb-992c-20ef034134bf"),
                            Quantity = 0
                        });
                });

            modelBuilder.Entity("StoreApi.Entities.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Id = new Guid("73d0b378-6a25-4919-b2e7-4c3e9ed95695"),
                            Name = "Electronics"
                        });
                });

            modelBuilder.Entity("StoreApi.Entities.Customer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Address")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Customers");

                    b.HasData(
                        new
                        {
                            Id = new Guid("860c06fb-fd88-4661-bbba-9afefe9ccc3f"),
                            Address = "İzmir",
                            Email = "hasancoban@std.iyte.edu.tr",
                            Name = "Hasan Çoban",
                            PhoneNumber = "5316731512"
                        },
                        new
                        {
                            Id = new Guid("4d4d5339-b460-4afe-a131-9de34de8ee69"),
                            Address = "İzmir",
                            Email = "boraersoy@std.iyte.edu.tr",
                            Name = "Bora Ersoy",
                            PhoneNumber = "5416731512"
                        });
                });

            modelBuilder.Entity("StoreApi.Entities.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("Orders");

                    b.HasData(
                        new
                        {
                            Id = new Guid("0aa7b22d-c50f-477e-93df-822e2ad2c264"),
                            CustomerId = new Guid("860c06fb-fd88-4661-bbba-9afefe9ccc3f"),
                            Date = new DateTime(2024, 10, 21, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        },
                        new
                        {
                            Id = new Guid("10af2d9b-2007-4798-be1c-115c852c2ffe"),
                            CustomerId = new Guid("860c06fb-fd88-4661-bbba-9afefe9ccc3f"),
                            Date = new DateTime(2024, 10, 21, 0, 0, 0, 0, DateTimeKind.Unspecified)
                        });
                });

            modelBuilder.Entity("StoreApi.Entities.OrderItem", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Price")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("TEXT");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id", "OrderId");

                    b.HasIndex("OrderId");

                    b.HasIndex("ProductId");

                    b.ToTable("OrderItems");

                    b.HasData(
                        new
                        {
                            Id = new Guid("1aa7b22d-c50f-477e-93df-822e2ad2c264"),
                            OrderId = new Guid("0aa7b22d-c50f-477e-93df-822e2ad2c264"),
                            Price = 29.99m,
                            ProductId = new Guid("17892431-77a6-4a94-9fac-70a05552aa8f"),
                            Quantity = 3
                        },
                        new
                        {
                            Id = new Guid("2aaf2d9b-2007-4798-be1c-115c852c2ffe"),
                            OrderId = new Guid("0aa7b22d-c50f-477e-93df-822e2ad2c264"),
                            Price = 19.99m,
                            ProductId = new Guid("886b55ae-3dd1-44fb-992c-20ef034134bf"),
                            Quantity = 2
                        },
                        new
                        {
                            Id = new Guid("3aaf2d9b-2007-4798-be1c-115c852c2ffe"),
                            OrderId = new Guid("10af2d9b-2007-4798-be1c-115c852c2ffe"),
                            Price = 29.99m,
                            ProductId = new Guid("17892431-77a6-4a94-9fac-70a05552aa8f"),
                            Quantity = 1
                        },
                        new
                        {
                            Id = new Guid("4aaf2d9b-2007-4798-be1c-115c852c2ffe"),
                            OrderId = new Guid("10af2d9b-2007-4798-be1c-115c852c2ffe"),
                            Price = 9.99m,
                            ProductId = new Guid("ae450822-2ddf-4ca3-8476-8849b6041939"),
                            Quantity = 4
                        });
                });

            modelBuilder.Entity("StoreApi.Entities.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Price")
                        .HasColumnType("TEXT");

                    b.Property<string>("SKU")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = new Guid("886b55ae-3dd1-44fb-992c-20ef034134bf"),
                            CategoryId = new Guid("73d0b378-6a25-4919-b2e7-4c3e9ed95695"),
                            Description = "A smooth and responsive wireless mouse for everyday use.",
                            Name = "Wireless Mouse",
                            Price = 29.99m,
                            SKU = "WM-001"
                        },
                        new
                        {
                            Id = new Guid("17892431-77a6-4a94-9fac-70a05552aa8f"),
                            CategoryId = new Guid("73d0b378-6a25-4919-b2e7-4c3e9ed95695"),
                            Description = "A high-quality mechanical keyboard with customizable RGB lighting.",
                            Name = "Mechanical Keyboard",
                            Price = 79.99m,
                            SKU = "MK-002"
                        },
                        new
                        {
                            Id = new Guid("ae450822-2ddf-4ca3-8476-8849b6041939"),
                            CategoryId = new Guid("73d0b378-6a25-4919-b2e7-4c3e9ed95695"),
                            Description = "A versatile USB-C hub with multiple ports for connectivity.",
                            Name = "USB-C Hub",
                            Price = 49.99m,
                            SKU = "UH-003"
                        });
                });

            modelBuilder.Entity("StoreApi.Entities.Wishlist", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id", "CustomerId");

                    b.HasIndex("CustomerId");

                    b.HasIndex("ProductId");

                    b.ToTable("Wishlists");

                    b.HasData(
                        new
                        {
                            Id = new Guid("797c646a-c836-43db-b232-742254d03bf0"),
                            CustomerId = new Guid("860c06fb-fd88-4661-bbba-9afefe9ccc3f"),
                            ProductId = new Guid("17892431-77a6-4a94-9fac-70a05552aa8f")
                        },
                        new
                        {
                            Id = new Guid("697c646a-c836-43db-b232-742254d03bf0"),
                            CustomerId = new Guid("860c06fb-fd88-4661-bbba-9afefe9ccc3f"),
                            ProductId = new Guid("ae450822-2ddf-4ca3-8476-8849b6041939")
                        });
                });

            modelBuilder.Entity("StoreApi.Entities.Cart", b =>
                {
                    b.HasOne("StoreApi.Entities.Customer", "Customer")
                        .WithMany("Carts")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("StoreApi.Entities.Product", "Product")
                        .WithMany("Carts")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("StoreApi.Entities.Order", b =>
                {
                    b.HasOne("StoreApi.Entities.Customer", "Customer")
                        .WithMany("Orders")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("StoreApi.Entities.OrderItem", b =>
                {
                    b.HasOne("StoreApi.Entities.Order", "Order")
                        .WithMany("OrderItems")
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StoreApi.Entities.Product", "Product")
                        .WithMany("OrderItems")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("StoreApi.Entities.Product", b =>
                {
                    b.HasOne("StoreApi.Entities.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("StoreApi.Entities.Wishlist", b =>
                {
                    b.HasOne("StoreApi.Entities.Customer", "Customer")
                        .WithMany("Wishlists")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("StoreApi.Entities.Product", "Product")
                        .WithMany("Wishlists")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("StoreApi.Entities.Category", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("StoreApi.Entities.Customer", b =>
                {
                    b.Navigation("Carts");

                    b.Navigation("Orders");

                    b.Navigation("Wishlists");
                });

            modelBuilder.Entity("StoreApi.Entities.Order", b =>
                {
                    b.Navigation("OrderItems");
                });

            modelBuilder.Entity("StoreApi.Entities.Product", b =>
                {
                    b.Navigation("Carts");

                    b.Navigation("OrderItems");

                    b.Navigation("Wishlists");
                });
#pragma warning restore 612, 618
        }
    }
}
