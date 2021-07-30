using Microsoft.EntityFrameworkCore.Migrations;

namespace My_Company.Data.Migrations
{
    public partial class addwarehouserow : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseSectors_Warehouses_WarehouseId",
                table: "WarehouseSectors");

            migrationBuilder.DropIndex(
                name: "IX_WarehouseSectors_WarehouseId",
                table: "WarehouseSectors");

            migrationBuilder.DropColumn(
                name: "Column",
                table: "WarehouseSectors");

            migrationBuilder.DropColumn(
                name: "Row",
                table: "WarehouseSectors");

            migrationBuilder.DropColumn(
                name: "WarehouseId",
                table: "WarehouseSectors");

            migrationBuilder.RenameColumn(
                name: "SectorId",
                table: "WarehouseSectors",
                newName: "RowId");

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "WarehouseSectors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Street",
                table: "Warehouses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Warehouses",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "Warehouses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "warehouseRows",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RowName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WarehouseId = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_warehouseRows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_warehouseRows_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseSectors_RowId",
                table: "WarehouseSectors",
                column: "RowId");

            migrationBuilder.CreateIndex(
                name: "IX_warehouseRows_WarehouseId",
                table: "warehouseRows",
                column: "WarehouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseSectors_warehouseRows_RowId",
                table: "WarehouseSectors",
                column: "RowId",
                principalTable: "warehouseRows",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseSectors_warehouseRows_RowId",
                table: "WarehouseSectors");

            migrationBuilder.DropTable(
                name: "warehouseRows");

            migrationBuilder.DropIndex(
                name: "IX_WarehouseSectors_RowId",
                table: "WarehouseSectors");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "WarehouseSectors");

            migrationBuilder.RenameColumn(
                name: "RowId",
                table: "WarehouseSectors",
                newName: "SectorId");

            migrationBuilder.AddColumn<string>(
                name: "Column",
                table: "WarehouseSectors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Row",
                table: "WarehouseSectors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WarehouseId",
                table: "WarehouseSectors",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Street",
                table: "Warehouses",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "Name",
                table: "Warehouses",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "Warehouses",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseSectors_WarehouseId",
                table: "WarehouseSectors",
                column: "WarehouseId");

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseSectors_Warehouses_WarehouseId",
                table: "WarehouseSectors",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
