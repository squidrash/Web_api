using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Web_api_pizza.Migrations
{
    public partial class customerOrderEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Customers_CustomerEntityId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_CustomerEntityId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CustomerEntityId",
                table: "Orders");

            migrationBuilder.CreateTable(
                name: "CustomerOrderEntities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CustomerEntityId = table.Column<int>(nullable: true),
                    OrderEntityId = table.Column<int>(nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOrderEntities_CustomerEntityId",
                table: "CustomerOrderEntities",
                column: "CustomerEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerOrderEntities_OrderEntityId",
                table: "CustomerOrderEntities",
                column: "OrderEntityId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerOrderEntities");

            migrationBuilder.AddColumn<int>(
                name: "CustomerEntityId",
                table: "Orders",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerEntityId",
                table: "Orders",
                column: "CustomerEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Customers_CustomerEntityId",
                table: "Orders",
                column: "CustomerEntityId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
