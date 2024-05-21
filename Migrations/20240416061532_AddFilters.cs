using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Photoforge_Server.Migrations
{
    /// <inheritdoc />
    public partial class AddFilters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Height",
                table: "Projects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Width",
                table: "Projects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<float>(
                name: "Brightness",
                table: "Layers",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Contrast",
                table: "Layers",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Hue",
                table: "Layers",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Lightness",
                table: "Layers",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Saturation",
                table: "Layers",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Height",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Width",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Brightness",
                table: "Layers");

            migrationBuilder.DropColumn(
                name: "Contrast",
                table: "Layers");

            migrationBuilder.DropColumn(
                name: "Hue",
                table: "Layers");

            migrationBuilder.DropColumn(
                name: "Lightness",
                table: "Layers");

            migrationBuilder.DropColumn(
                name: "Saturation",
                table: "Layers");
        }
    }
}
