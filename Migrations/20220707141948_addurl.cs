using Microsoft.EntityFrameworkCore.Migrations;

namespace UniProject.DataLayer.Migrations
{
    public partial class addurl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CategoryUrl",
                table: "ProductCategories",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryUrl",
                table: "ProductCategories");
        }
    }
}
