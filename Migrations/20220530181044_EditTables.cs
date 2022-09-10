using Microsoft.EntityFrameworkCore.Migrations;

namespace UniProject.DataLayer.Migrations
{
    public partial class EditTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_ProductCategories_CategoryId",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_ProductCategories_SubCategory",
                table: "Product");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductFeature_Product_ProductId",
                table: "ProductFeature");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductGallery_Product_ProductId",
                table: "ProductGallery");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductGallery",
                table: "ProductGallery");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductFeature",
                table: "ProductFeature");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Product",
                table: "Product");

            migrationBuilder.RenameTable(
                name: "ProductGallery",
                newName: "ProductGalleries");

            migrationBuilder.RenameTable(
                name: "ProductFeature",
                newName: "ProductFeatures");

            migrationBuilder.RenameTable(
                name: "Product",
                newName: "Products");

            migrationBuilder.RenameIndex(
                name: "IX_ProductGallery_ProductId",
                table: "ProductGalleries",
                newName: "IX_ProductGalleries_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductFeature_ProductId",
                table: "ProductFeatures",
                newName: "IX_ProductFeatures_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_Product_SubCategory",
                table: "Products",
                newName: "IX_Products_SubCategory");

            migrationBuilder.RenameIndex(
                name: "IX_Product_CategoryId",
                table: "Products",
                newName: "IX_Products_CategoryId");

            migrationBuilder.AddColumn<int>(
                name: "SellerId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductGalleries",
                table: "ProductGalleries",
                column: "GalleryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductFeatures",
                table: "ProductFeatures",
                column: "FeatureId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products",
                table: "Products",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_SellerId",
                table: "Products",
                column: "SellerId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductFeatures_Products_ProductId",
                table: "ProductFeatures",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductGalleries_Products_ProductId",
                table: "ProductGalleries",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductCategories_CategoryId",
                table: "Products",
                column: "CategoryId",
                principalTable: "ProductCategories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductCategories_SubCategory",
                table: "Products",
                column: "SubCategory",
                principalTable: "ProductCategories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Users_SellerId",
                table: "Products",
                column: "SellerId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductFeatures_Products_ProductId",
                table: "ProductFeatures");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductGalleries_Products_ProductId",
                table: "ProductGalleries");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductCategories_CategoryId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductCategories_SubCategory",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Users_SellerId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_SellerId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductGalleries",
                table: "ProductGalleries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductFeatures",
                table: "ProductFeatures");

            migrationBuilder.DropColumn(
                name: "SellerId",
                table: "Products");

            migrationBuilder.RenameTable(
                name: "Products",
                newName: "Product");

            migrationBuilder.RenameTable(
                name: "ProductGalleries",
                newName: "ProductGallery");

            migrationBuilder.RenameTable(
                name: "ProductFeatures",
                newName: "ProductFeature");

            migrationBuilder.RenameIndex(
                name: "IX_Products_SubCategory",
                table: "Product",
                newName: "IX_Product_SubCategory");

            migrationBuilder.RenameIndex(
                name: "IX_Products_CategoryId",
                table: "Product",
                newName: "IX_Product_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductGalleries_ProductId",
                table: "ProductGallery",
                newName: "IX_ProductGallery_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductFeatures_ProductId",
                table: "ProductFeature",
                newName: "IX_ProductFeature_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Product",
                table: "Product",
                column: "ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductGallery",
                table: "ProductGallery",
                column: "GalleryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductFeature",
                table: "ProductFeature",
                column: "FeatureId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_ProductCategories_CategoryId",
                table: "Product",
                column: "CategoryId",
                principalTable: "ProductCategories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_ProductCategories_SubCategory",
                table: "Product",
                column: "SubCategory",
                principalTable: "ProductCategories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductFeature_Product_ProductId",
                table: "ProductFeature",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductGallery_Product_ProductId",
                table: "ProductGallery",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
