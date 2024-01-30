﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStoreServer.Migrations
{
    /// <inheritdoc />
    public partial class mg22 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrderUniqeNumber",
                table: "Orders",
                type: "nvarchar(6)",
                maxLength: 6,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderUniqeNumber",
                table: "Orders");
        }
    }
}
