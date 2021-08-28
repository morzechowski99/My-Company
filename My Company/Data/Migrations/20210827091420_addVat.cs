using Microsoft.EntityFrameworkCore.Migrations;

namespace My_Company.Data.Migrations
{
    public partial class addVat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VATRateId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "VATRate",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Rate = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VATRate", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_VATRateId",
                table: "Products",
                column: "VATRateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_VATRate_VATRateId",
                table: "Products",
                column: "VATRateId",
                principalTable: "VATRate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_VATRate_VATRateId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "VATRate");

            migrationBuilder.DropIndex(
                name: "IX_Products_VATRateId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "VATRateId",
                table: "Products");
        }
    }
}
