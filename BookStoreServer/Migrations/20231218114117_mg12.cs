using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStoreServer.Migrations
{
    /// <inheritdoc />
    public partial class mg12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCartItems_Books_BookId",
                table: "ShoppingCartItems");

            migrationBuilder.RenameColumn(
                name: "BookId",
                table: "ShoppingCartItems",
                newName: "BookVariationId");

            migrationBuilder.RenameIndex(
                name: "IX_ShoppingCartItems_BookId",
                table: "ShoppingCartItems",
                newName: "IX_ShoppingCartItems_BookVariationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCartItems_BookVariations_BookVariationId",
                table: "ShoppingCartItems",
                column: "BookVariationId",
                principalTable: "BookVariations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCartItems_BookVariations_BookVariationId",
                table: "ShoppingCartItems");

            migrationBuilder.RenameColumn(
                name: "BookVariationId",
                table: "ShoppingCartItems",
                newName: "BookId");

            migrationBuilder.RenameIndex(
                name: "IX_ShoppingCartItems_BookVariationId",
                table: "ShoppingCartItems",
                newName: "IX_ShoppingCartItems_BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCartItems_Books_BookId",
                table: "ShoppingCartItems",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
