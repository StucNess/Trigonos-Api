using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogicaTrigonos.Identity.Migrations
{
    public partial class MigracionRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Bhabilitado",
                table: "AspNetRoles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Descripcion",
                table: "AspNetRoles",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bhabilitado",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "Descripcion",
                table: "AspNetRoles");
        }
    }
}
