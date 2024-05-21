using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Photoforge_Server.Migrations
{
    /// <inheritdoc />
    public partial class AddStoredFileName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StroredImageName",
                table: "Layers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StroredImageName",
                table: "Layers");
        }
    }
}
