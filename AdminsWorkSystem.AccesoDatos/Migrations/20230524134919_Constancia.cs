using Microsoft.EntityFrameworkCore.Migrations;

namespace AdminsWorkSystem.AccesoDatos.Migrations
{
    public partial class Constancia : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Estatu",
                table: "Constancia");

            migrationBuilder.AlterColumn<string>(
                name: "Estado",
                table: "evidenciaOReporte",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Estado",
                table: "evidenciaOReporte",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Estatu",
                table: "Constancia",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
