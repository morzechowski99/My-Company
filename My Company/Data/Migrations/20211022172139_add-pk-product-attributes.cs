using Microsoft.EntityFrameworkCore.Migrations;

namespace My_Company.Data.Migrations
{
    public partial class addpkproductattributes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductAttributes",
                table: "ProductAttributes");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ProductAttributes",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductAttributes",
                table: "ProductAttributes",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttributes_ProductId",
                table: "ProductAttributes",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductAttributes",
                table: "ProductAttributes");

            migrationBuilder.DropIndex(
                name: "IX_ProductAttributes_ProductId",
                table: "ProductAttributes");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ProductAttributes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductAttributes",
                table: "ProductAttributes",
                columns: new[] { "ProductId", "AttributeId" });
        }
    }
}
