using Microsoft.EntityFrameworkCore.Migrations;

namespace UniProject.DataLayer.Migrations
{
    public partial class EditProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Remainder",
                table: "Products",
                type: "int",
                maxLength: 500,
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Remainder",
                table: "Products");
        }
    }
}
