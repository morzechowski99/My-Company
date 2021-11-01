using Microsoft.EntityFrameworkCore.Migrations;

namespace My_Company.Data.Migrations
{
    public partial class addpzNumberindelivery : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PZNumber",
                table: "Deliveries",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_PZNumber",
                table: "Deliveries",
                column: "PZNumber",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Deliveries_PZNumber",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "PZNumber",
                table: "Deliveries");
        }
    }
}
