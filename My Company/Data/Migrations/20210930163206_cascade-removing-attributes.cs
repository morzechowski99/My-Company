using Microsoft.EntityFrameworkCore.Migrations;

namespace My_Company.Data.Migrations
{
    public partial class cascaderemovingattributes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductAttributes_Attributes_AttributeId",
                table: "ProductAttributes");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAttributes_Attributes_AttributeId",
                table: "ProductAttributes",
                column: "AttributeId",
                principalTable: "Attributes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductAttributes_Attributes_AttributeId",
                table: "ProductAttributes");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAttributes_Attributes_AttributeId",
                table: "ProductAttributes",
                column: "AttributeId",
                principalTable: "Attributes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
