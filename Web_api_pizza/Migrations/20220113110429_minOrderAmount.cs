using Microsoft.EntityFrameworkCore.Migrations;

namespace Web_api_pizza.Migrations
{
    public partial class minOrderAmount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "MinOrderAmount",
                table: "Offers",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MinOrderAmount",
                table: "Offers");
        }
    }
}
