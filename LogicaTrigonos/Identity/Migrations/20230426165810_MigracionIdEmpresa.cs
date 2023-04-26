using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LogicaTrigonos.Identity.Migrations
{
    public partial class MigracionIdEmpresa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdEmpresa",
                table: "AspNetUsers",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IdEmpresa",
                table: "AspNetUsers");
        }
    }
}
