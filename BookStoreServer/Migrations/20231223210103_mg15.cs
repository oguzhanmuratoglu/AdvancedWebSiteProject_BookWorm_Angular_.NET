using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStoreServer.Migrations
{
    /// <inheritdoc />
    public partial class mg15 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WishLists_Books_BookId",
                table: "WishLists");

            migrationBuilder.DropIndex(
                name: "IX_WishLists_BookId",
                table: "WishLists");

            migrationBuilder.DropColumn(
                name: "BookId",
                table: "WishLists");

            migrationBuilder.AddColumn<int>(
                name: "WishListId",
                table: "Books",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Books_WishListId",
                table: "Books",
                column: "WishListId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_WishLists_WishListId",
                table: "Books",
                column: "WishListId",
                principalTable: "WishLists",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_WishLists_WishListId",
                table: "Books");

            migrationBuilder.DropIndex(
                name: "IX_Books_WishListId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "WishListId",
                table: "Books");

            migrationBuilder.AddColumn<int>(
                name: "BookId",
                table: "WishLists",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_WishLists_BookId",
                table: "WishLists",
                column: "BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_WishLists_Books_BookId",
                table: "WishLists",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
