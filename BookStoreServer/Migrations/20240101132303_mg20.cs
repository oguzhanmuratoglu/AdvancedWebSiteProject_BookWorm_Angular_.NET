using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStoreServer.Migrations
{
    /// <inheritdoc />
    public partial class mg20 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_Books_BookId",
                table: "OrderItems");

            migrationBuilder.RenameColumn(
                name: "BookId",
                table: "OrderItems",
                newName: "BookVariationId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderItems_BookId",
                table: "OrderItems",
                newName: "IX_OrderItems_BookVariationId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_BookVariations_BookVariationId",
                table: "OrderItems",
                column: "BookVariationId",
                principalTable: "BookVariations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_BookVariations_BookVariationId",
                table: "OrderItems");

            migrationBuilder.RenameColumn(
                name: "BookVariationId",
                table: "OrderItems",
                newName: "BookId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderItems_BookVariationId",
                table: "OrderItems",
                newName: "IX_OrderItems_BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Books_BookId",
                table: "OrderItems",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
