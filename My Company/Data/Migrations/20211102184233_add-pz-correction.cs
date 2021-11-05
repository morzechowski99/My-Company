using Microsoft.EntityFrameworkCore.Migrations;

namespace My_Company.Data.Migrations
{
    public partial class addpzcorrection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CorrectingId",
                table: "Deliveries",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCorrecting",
                table: "Deliveries",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_CorrectingId",
                table: "Deliveries",
                column: "CorrectingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Deliveries_Deliveries_CorrectingId",
                table: "Deliveries",
                column: "CorrectingId",
                principalTable: "Deliveries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deliveries_Deliveries_CorrectingId",
                table: "Deliveries");

            migrationBuilder.DropIndex(
                name: "IX_Deliveries_CorrectingId",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "CorrectingId",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "IsCorrecting",
                table: "Deliveries");
        }
    }
}
