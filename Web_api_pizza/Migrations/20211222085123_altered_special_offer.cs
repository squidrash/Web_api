using Microsoft.EntityFrameworkCore.Migrations;

namespace Web_api_pizza.Migrations
{
    public partial class altered_special_offer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OfferMainDishesEntity");

            migrationBuilder.AddColumn<int>(
                name: "MainDishId",
                table: "Offers",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Offers_MainDishId",
                table: "Offers",
                column: "MainDishId");

            migrationBuilder.AddForeignKey(
                name: "FK_Offers_Dishes_MainDishId",
                table: "Offers",
                column: "MainDishId",
                principalTable: "Dishes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Offers_Dishes_MainDishId",
                table: "Offers");

            migrationBuilder.DropIndex(
                name: "IX_Offers_MainDishId",
                table: "Offers");

            migrationBuilder.DropColumn(
                name: "MainDishId",
                table: "Offers");

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

            migrationBuilder.CreateIndex(
                name: "IX_OfferMainDishesEntity_OfferMainDishesId",
                table: "OfferMainDishesEntity",
                column: "OfferMainDishesId");
        }
    }
}
