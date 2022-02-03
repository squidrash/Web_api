using Microsoft.EntityFrameworkCore.Migrations;

namespace Web_api_pizza.Migrations
{
    public partial class Category4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddUniqueConstraint(
                name: "AK_Categories_Name",
                table: "Categories",
                column: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Categories_Name",
                table: "Categories");
        }
    }
}
