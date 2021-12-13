using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Web_api_pizza.Migrations
{
    public partial class special_offer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    City = table.Column<string>(type: "text", nullable: false),
                    Street = table.Column<string>(type: "text", nullable: false),
                    NumberOfBuild = table.Column<string>(type: "text", nullable: false),
                    NumberOfEntrance = table.Column<int>(type: "integer", nullable: true),
                    Apartment = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<string>(type: "text", nullable: false),
                    Discount = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Dishes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductName = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ShortDescription = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dishes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    TotalSum = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerAddressEntities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CustomerEntityId = table.Column<int>(type: "integer", nullable: false),
                    AddressEntityId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerAddressEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerAddressEntities_Addresses_AddressEntityId",
                        column: x => x.AddressEntityId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerAddressEntities_Customers_CustomerEntityId",
                        column: x => x.CustomerEntityId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Offers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    PromoCode = table.Column<string>(type: "text", nullable: false),
                    TypeOffer = table.Column<int>(type: "integer", nullable: false),
                    Discount = table.Column<int>(type: "integer", nullable: false),
                    ExtraDishId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Offers_Dishes_ExtraDishId",
                        column: x => x.ExtraDishId,
                        principalTable: "Dishes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AddressOrderEntities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderEntityId = table.Column<int>(type: "integer", nullable: false),
                    AddressEntityId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddressOrderEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AddressOrderEntities_Addresses_AddressEntityId",
                        column: x => x.AddressEntityId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AddressOrderEntities_Orders_OrderEntityId",
                        column: x => x.OrderEntityId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomerOrderEntities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CustomerEntityId = table.Column<int>(type: "integer", nullable: true),
                    OrderEntityId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerOrderEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerOrderEntities_Customers_CustomerEntityId",
                        column: x => x.CustomerEntityId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CustomerOrderEntities_Orders_OrderEntityId",
                        column: x => x.OrderEntityId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderDishEntities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderEntityId = table.Column<int>(type: "integer", nullable: false),
                    DishEntityId = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDishEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderDishEntities_Dishes_DishEntityId",
                        column: x => x.DishEntityId,
                        principalTable: "Dishes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDishEntities_Orders_OrderEntityId",
                        column: x => x.OrderEntityId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OfferMainDishesEntity",
                columns: table => new
                {
                    MainDishesId = table.Column<int>(type: "integer", nullable: false),
                    OfferMainDishesId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfferMainDishesEntity", x => new { x.MainDishesId, x.OfferMainDishesId });
                    table.ForeignKey(
                        name: "FK_OfferMainDishesEntity_Dishes_MainDishesId",
                        column: x => x.MainDishesId,
                        principalTable: "Dishes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OfferMainDishesEntity_Offers_OfferMainDishesId",
                        column: x => x.OfferMainDishesId,
                        principalTable: "Offers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpecialOfferOrderEntities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderEntityId = table.Column<int>(type: "integer", nullable: false),
                    SpecialOfferEntity = table.Column<int>(type: "integer", nullable: false),
                    SpecialOfferId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialOfferOrderEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpecialOfferOrderEntities_Offers_SpecialOfferId",
                        column: x => x.SpecialOfferId,
                        principalTable: "Offers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SpecialOfferOrderEntities_Orders_OrderEntityId",
                        column: x => x.OrderEntityId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AddressOrderEntities_AddressEntityId",
                table: "AddressOrderEntities",
                column: "AddressEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_AddressOrderEntities_OrderEntityId",
                table: "AddressOrderEntities",
                column: "OrderEntityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerAddressEntities_AddressEntityId",
                table: "CustomerAddressEntities",
                column: "AddressEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerAddressEntities_CustomerEntityId",
                table: "CustomerAddressEntities",
                column: "CustomerEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOrderEntities_CustomerEntityId",
                table: "CustomerOrderEntities",
                column: "CustomerEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOrderEntities_OrderEntityId",
                table: "CustomerOrderEntities",
                column: "OrderEntityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OfferMainDishesEntity_OfferMainDishesId",
                table: "OfferMainDishesEntity",
                column: "OfferMainDishesId");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_ExtraDishId",
                table: "Offers",
                column: "ExtraDishId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDishEntities_DishEntityId",
                table: "OrderDishEntities",
                column: "DishEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDishEntities_OrderEntityId",
                table: "OrderDishEntities",
                column: "OrderEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecialOfferOrderEntities_OrderEntityId",
                table: "SpecialOfferOrderEntities",
                column: "OrderEntityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SpecialOfferOrderEntities_SpecialOfferId",
                table: "SpecialOfferOrderEntities",
                column: "SpecialOfferId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AddressOrderEntities");

            migrationBuilder.DropTable(
                name: "CustomerAddressEntities");

            migrationBuilder.DropTable(
                name: "CustomerOrderEntities");

            migrationBuilder.DropTable(
                name: "OfferMainDishesEntity");

            migrationBuilder.DropTable(
                name: "OrderDishEntities");

            migrationBuilder.DropTable(
                name: "SpecialOfferOrderEntities");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Offers");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Dishes");
        }
    }
}
