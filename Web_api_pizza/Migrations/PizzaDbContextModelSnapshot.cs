﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Web_api_pizza.Storage.DTO;

namespace Web_api_pizza.Migrations
{
    [DbContext(typeof(PizzaDbContext))]
    partial class PizzaDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Web_api_pizza.Storage.Models.AddressEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("Apartment")
                        .HasColumnType("integer");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("NumberOfBuild")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("NumberOfEntrance")
                        .HasColumnType("integer");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("Web_api_pizza.Storage.Models.AddressOrderEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("AddressEntityId")
                        .HasColumnType("integer");

                    b.Property<int>("OrderEntityId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("AddressEntityId");

                    b.HasIndex("OrderEntityId")
                        .IsUnique();

                    b.ToTable("AddressOrderEntities");
                });

            modelBuilder.Entity("Web_api_pizza.Storage.Models.CustomerAddressEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("AddressEntityId")
                        .HasColumnType("integer");

                    b.Property<int>("CustomerEntityId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("AddressEntityId");

                    b.HasIndex("CustomerEntityId");

                    b.ToTable("CustomerAddressEntities");
                });

            modelBuilder.Entity("Web_api_pizza.Storage.Models.CustomerEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("Discount")
                        .HasColumnType("integer");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("Web_api_pizza.Storage.Models.DishEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("Price")
                        .HasColumnType("integer");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Dishes");
                });

            modelBuilder.Entity("Web_api_pizza.Storage.Models.OrderDishEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("DishEntityId")
                        .HasColumnType("integer");

                    b.Property<int>("OrderEntityId")
                        .HasColumnType("integer");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("DishEntityId");

                    b.HasIndex("OrderEntityId");

                    b.ToTable("OrderDishEntities");
                });

            modelBuilder.Entity("Web_api_pizza.Storage.Models.OrderEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("CreatTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int?>("CustomerEntityId")
                        .HasColumnType("integer");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CustomerEntityId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Web_api_pizza.Storage.Models.AddressOrderEntity", b =>
                {
                    b.HasOne("Web_api_pizza.Storage.Models.AddressEntity", "Address")
                        .WithMany("AddressOrder")
                        .HasForeignKey("AddressEntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Web_api_pizza.Storage.Models.OrderEntity", "Order")
                        .WithOne("AddressOrder")
                        .HasForeignKey("Web_api_pizza.Storage.Models.AddressOrderEntity", "OrderEntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Web_api_pizza.Storage.Models.CustomerAddressEntity", b =>
                {
                    b.HasOne("Web_api_pizza.Storage.Models.AddressEntity", "Address")
                        .WithMany("Customers")
                        .HasForeignKey("AddressEntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Web_api_pizza.Storage.Models.CustomerEntity", "Customer")
                        .WithMany("Addresses")
                        .HasForeignKey("CustomerEntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Web_api_pizza.Storage.Models.OrderDishEntity", b =>
                {
                    b.HasOne("Web_api_pizza.Storage.Models.DishEntity", "Dish")
                        .WithMany("Orders")
                        .HasForeignKey("DishEntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Web_api_pizza.Storage.Models.OrderEntity", "Order")
                        .WithMany("Products")
                        .HasForeignKey("OrderEntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Web_api_pizza.Storage.Models.OrderEntity", b =>
                {
                    b.HasOne("Web_api_pizza.Storage.Models.CustomerEntity", "Customer")
                        .WithMany("Orders")
                        .HasForeignKey("CustomerEntityId");
                });
#pragma warning restore 612, 618
        }
    }
}
