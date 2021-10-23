using Microsoft.EntityFrameworkCore.Migrations;

namespace My_Company.Data.Migrations
{
    public partial class addmainphotoflag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsMainPhoto",
                table: "Photo",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMainPhoto",
                table: "Photo");
        }
    }
}
