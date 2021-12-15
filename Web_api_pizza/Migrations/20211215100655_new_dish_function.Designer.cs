﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Web_api_pizza.Storage.DTO;

namespace Web_api_pizza.Migrations
{
    [DbContext(typeof(PizzaDbContext))]
    [Migration("20211215100655_new_dish_function")]
    partial class new_dish_function
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.12")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("DishEntitySpecialOfferEntity", b =>
                {
                    b.Property<int>("MainDishesId")
                        .HasColumnType("integer");

                    b.Property<int>("OfferMainDishesId")
                        .HasColumnType("integer");

                    b.HasKey("MainDishesId", "OfferMainDishesId");

                    b.HasIndex("OfferMainDishesId");

                    b.ToTable("OfferMainDishesEntity");
                });

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

            modelBuilder.Entity("Web_api_pizza.Storage.Models.CustomerOrderEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("CustomerEntityId")
                        .HasColumnType("integer");

                    b.Property<int>("OrderEntityId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CustomerEntityId");

                    b.HasIndex("OrderEntityId")
                        .IsUnique();

                    b.ToTable("CustomerOrderEntities");
                });

            modelBuilder.Entity("Web_api_pizza.Storage.Models.DishEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ShortDescription")
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

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<decimal>("TotalSum")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Web_api_pizza.Storage.Models.SpecialOfferEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("character varying(300)");

                    b.Property<int>("Discount")
                        .HasColumnType("integer");

                    b.Property<int?>("ExtraDishId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("NumberOfExtraDish")
                        .HasColumnType("integer");

                    b.Property<string>("PromoCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("RequiredNumberOfDish")
                        .HasColumnType("integer");

                    b.Property<int>("TypeOffer")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ExtraDishId");

                    b.ToTable("Offers");
                });

            modelBuilder.Entity("Web_api_pizza.Storage.Models.SpecialOfferOrderEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("OrderEntityId")
                        .HasColumnType("integer");

                    b.Property<int>("SpecialOfferEntity")
                        .HasColumnType("integer");

                    b.Property<int?>("SpecialOfferId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("OrderEntityId")
                        .IsUnique();

                    b.HasIndex("SpecialOfferId");

                    b.ToTable("SpecialOfferOrderEntities");
                });

            modelBuilder.Entity("DishEntitySpecialOfferEntity", b =>
                {
                    b.HasOne("Web_api_pizza.Storage.Models.DishEntity", null)
                        .WithMany()
                        .HasForeignKey("MainDishesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Web_api_pizza.Storage.Models.SpecialOfferEntity", null)
                        .WithMany()
                        .HasForeignKey("OfferMainDishesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
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

                    b.Navigation("Address");

                    b.Navigation("Order");
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

                    b.Navigation("Address");

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("Web_api_pizza.Storage.Models.CustomerOrderEntity", b =>
                {
                    b.HasOne("Web_api_pizza.Storage.Models.CustomerEntity", "Customer")
                        .WithMany("Orders")
                        .HasForeignKey("CustomerEntityId");

                    b.HasOne("Web_api_pizza.Storage.Models.OrderEntity", "Order")
                        .WithOne("Customer")
                        .HasForeignKey("Web_api_pizza.Storage.Models.CustomerOrderEntity", "OrderEntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("Web_api_pizza.Storage.Models.OrderDishEntity", b =>
                {
                    b.HasOne("Web_api_pizza.Storage.Models.DishEntity", "Dish")
                        .WithMany()
                        .HasForeignKey("DishEntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Web_api_pizza.Storage.Models.OrderEntity", "Order")
                        .WithMany("Products")
                        .HasForeignKey("OrderEntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Dish");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("Web_api_pizza.Storage.Models.SpecialOfferEntity", b =>
                {
                    b.HasOne("Web_api_pizza.Storage.Models.DishEntity", "ExtraDish")
                        .WithMany("OfferExtraDish")
                        .HasForeignKey("ExtraDishId");

                    b.Navigation("ExtraDish");
                });

            modelBuilder.Entity("Web_api_pizza.Storage.Models.SpecialOfferOrderEntity", b =>
                {
                    b.HasOne("Web_api_pizza.Storage.Models.OrderEntity", "Order")
                        .WithOne("SpecialOffer")
                        .HasForeignKey("Web_api_pizza.Storage.Models.SpecialOfferOrderEntity", "OrderEntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Web_api_pizza.Storage.Models.SpecialOfferEntity", "SpecialOffer")
                        .WithMany()
                        .HasForeignKey("SpecialOfferId");

                    b.Navigation("Order");

                    b.Navigation("SpecialOffer");
                });

            modelBuilder.Entity("Web_api_pizza.Storage.Models.AddressEntity", b =>
                {
                    b.Navigation("AddressOrder");

                    b.Navigation("Customers");
                });

            modelBuilder.Entity("Web_api_pizza.Storage.Models.CustomerEntity", b =>
                {
                    b.Navigation("Addresses");

                    b.Navigation("Orders");
                });

            modelBuilder.Entity("Web_api_pizza.Storage.Models.DishEntity", b =>
                {
                    b.Navigation("OfferExtraDish");
                });

            modelBuilder.Entity("Web_api_pizza.Storage.Models.OrderEntity", b =>
                {
                    b.Navigation("AddressOrder");

                    b.Navigation("Customer");

                    b.Navigation("Products");

                    b.Navigation("SpecialOffer");
                });
#pragma warning restore 612, 618
        }
    }
}
