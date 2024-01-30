using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStoreServer.Migrations
{
    /// <inheritdoc />
    public partial class mg17 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WishListItem_WishLists_WishListId",
                table: "WishListItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WishListItem",
                table: "WishListItem");

            migrationBuilder.RenameTable(
                name: "WishListItem",
                newName: "WishListItems");

            migrationBuilder.RenameIndex(
                name: "IX_WishListItem_WishListId",
                table: "WishListItems",
                newName: "IX_WishListItems_WishListId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WishListItems",
                table: "WishListItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WishListItems_WishLists_WishListId",
                table: "WishListItems",
                column: "WishListId",
                principalTable: "WishLists",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WishListItems_WishLists_WishListId",
                table: "WishListItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WishListItems",
                table: "WishListItems");

            migrationBuilder.RenameTable(
                name: "WishListItems",
                newName: "WishListItem");

            migrationBuilder.RenameIndex(
                name: "IX_WishListItems_WishListId",
                table: "WishListItem",
                newName: "IX_WishListItem_WishListId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WishListItem",
                table: "WishListItem",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WishListItem_WishLists_WishListId",
                table: "WishListItem",
                column: "WishListId",
                principalTable: "WishLists",
                principalColumn: "Id");
        }
    }
}
