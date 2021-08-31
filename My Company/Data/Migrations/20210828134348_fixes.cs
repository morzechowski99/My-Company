using Microsoft.EntityFrameworkCore.Migrations;

namespace My_Company.Data.Migrations
{
    public partial class fixes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_VATRate_VATRateId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_warehouseRows_Warehouses_WarehouseId",
                table: "warehouseRows");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseSectors_warehouseRows_RowId",
                table: "WarehouseSectors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_warehouseRows",
                table: "warehouseRows");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VATRate",
                table: "VATRate");

            migrationBuilder.DropColumn(
                name: "MyProperty",
                table: "Suppliers");

            migrationBuilder.RenameTable(
                name: "warehouseRows",
                newName: "WarehouseRows");

            migrationBuilder.RenameTable(
                name: "VATRate",
                newName: "VATRates");

            migrationBuilder.RenameIndex(
                name: "IX_warehouseRows_WarehouseId",
                table: "WarehouseRows",
                newName: "IX_WarehouseRows_WarehouseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WarehouseRows",
                table: "WarehouseRows",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VATRates",
                table: "VATRates",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_VATRates_VATRateId",
                table: "Products",
                column: "VATRateId",
                principalTable: "VATRates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseRows_Warehouses_WarehouseId",
                table: "WarehouseRows",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseSectors_WarehouseRows_RowId",
                table: "WarehouseSectors",
                column: "RowId",
                principalTable: "WarehouseRows",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_VATRates_VATRateId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseRows_Warehouses_WarehouseId",
                table: "WarehouseRows");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseSectors_WarehouseRows_RowId",
                table: "WarehouseSectors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WarehouseRows",
                table: "WarehouseRows");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VATRates",
                table: "VATRates");

            migrationBuilder.RenameTable(
                name: "WarehouseRows",
                newName: "warehouseRows");

            migrationBuilder.RenameTable(
                name: "VATRates",
                newName: "VATRate");

            migrationBuilder.RenameIndex(
                name: "IX_WarehouseRows_WarehouseId",
                table: "warehouseRows",
                newName: "IX_warehouseRows_WarehouseId");

            migrationBuilder.AddColumn<int>(
                name: "MyProperty",
                table: "Suppliers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_warehouseRows",
                table: "warehouseRows",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VATRate",
                table: "VATRate",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_VATRate_VATRateId",
                table: "Products",
                column: "VATRateId",
                principalTable: "VATRate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_warehouseRows_Warehouses_WarehouseId",
                table: "warehouseRows",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseSectors_warehouseRows_RowId",
                table: "WarehouseSectors",
                column: "RowId",
                principalTable: "warehouseRows",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
