using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Photoforge_Server.Migrations
{
    /// <inheritdoc />
    public partial class AddUrlPropertyForLayer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Path",
                table: "Layers",
                newName: "Url");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Url",
                table: "Layers",
                newName: "Path");
        }
    }
}
