using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStoreServer.Migrations
{
    /// <inheritdoc />
    public partial class mg18 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WishListItems_WishLists_WishListId",
                table: "WishListItems");

            migrationBuilder.AlterColumn<int>(
                name: "WishListId",
                table: "WishListItems",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WishListItems_WishLists_WishListId",
                table: "WishListItems",
                column: "WishListId",
                principalTable: "WishLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WishListItems_WishLists_WishListId",
                table: "WishListItems");

            migrationBuilder.AlterColumn<int>(
                name: "WishListId",
                table: "WishListItems",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_WishListItems_WishLists_WishListId",
                table: "WishListItems",
                column: "WishListId",
                principalTable: "WishLists",
                principalColumn: "Id");
        }
    }
}
