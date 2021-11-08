using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace My_Company.Data.Migrations
{
    public partial class fixpickingitem2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PickingItem_Picking_PickingOrderId",
                table: "PickingItem");

            migrationBuilder.DropIndex(
                name: "IX_PickingItem_PickingOrderId",
                table: "PickingItem");

            migrationBuilder.DropColumn(
                name: "PickingOrderId",
                table: "PickingItem");

            migrationBuilder.AddColumn<Guid>(
                name: "PickingId",
                table: "PickingItem",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_PickingItem_PickingId",
                table: "PickingItem",
                column: "PickingId");

            migrationBuilder.AddForeignKey(
                name: "FK_PickingItem_Picking_PickingId",
                table: "PickingItem",
                column: "PickingId",
                principalTable: "Picking",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PickingItem_Picking_PickingId",
                table: "PickingItem");

            migrationBuilder.DropIndex(
                name: "IX_PickingItem_PickingId",
                table: "PickingItem");

            migrationBuilder.DropColumn(
                name: "PickingId",
                table: "PickingItem");

            migrationBuilder.AddColumn<Guid>(
                name: "PickingOrderId",
                table: "PickingItem",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PickingItem_PickingOrderId",
                table: "PickingItem",
                column: "PickingOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_PickingItem_Picking_PickingOrderId",
                table: "PickingItem",
                column: "PickingOrderId",
                principalTable: "Picking",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
