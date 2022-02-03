using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Web_api_pizza.Migrations
{
    public partial class Category : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpecialOfferOrderEntities");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Dishes");

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.AddColumn<int>(
                name: "Category",
                table: "Dishes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

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
                name: "IX_SpecialOfferOrderEntities_OrderEntityId",
                table: "SpecialOfferOrderEntities",
                column: "OrderEntityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SpecialOfferOrderEntities_SpecialOfferId",
                table: "SpecialOfferOrderEntities",
                column: "SpecialOfferId");
        }
    }
}
