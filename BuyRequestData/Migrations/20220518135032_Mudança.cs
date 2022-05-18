using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BuyRequest.Data.Migrations
{
    public partial class Mudança : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_BuyRequests_BuyRequestsId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_BuyRequestsId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "BuyRequestsId",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "RequestId",
                table: "Products",
                newName: "BuyRequestId");

            migrationBuilder.AlterColumn<string>(
                name: "ProductDescription",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_BuyRequestId",
                table: "Products",
                column: "BuyRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_BuyRequests_BuyRequestId",
                table: "Products",
                column: "BuyRequestId",
                principalTable: "BuyRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_BuyRequests_BuyRequestId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_BuyRequestId",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "BuyRequestId",
                table: "Products",
                newName: "RequestId");

            migrationBuilder.AlterColumn<string>(
                name: "ProductDescription",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "BuyRequestsId",
                table: "Products",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_BuyRequestsId",
                table: "Products",
                column: "BuyRequestsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_BuyRequests_BuyRequestsId",
                table: "Products",
                column: "BuyRequestsId",
                principalTable: "BuyRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
