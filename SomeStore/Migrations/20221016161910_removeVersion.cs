using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SomeStore.Migrations
{
    public partial class removeVersion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Version",
                table: "Products");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "Products",
                type: "int",
                nullable: true);
        }
    }
}
