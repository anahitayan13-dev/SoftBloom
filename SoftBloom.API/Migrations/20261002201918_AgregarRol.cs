using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SoftBloom.API.Migrations
{
    /// <inheritdoc />
    public partial class AgregarRol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "rol",
                table: "Usuarios",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "rol",
                table: "Usuarios");
        }
    }
}
