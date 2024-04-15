using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ccytet.Server.Migrations
{
    /// <inheritdoc />
    public partial class _02 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "imagesArray",
                table: "Noticias",
                newName: "ImagesArray");

            migrationBuilder.AddColumn<string>(
                name: "Portada",
                table: "Noticias",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserCreatorName",
                table: "Noticias",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserUpdaterName",
                table: "Noticias",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Portada",
                table: "Noticias");

            migrationBuilder.DropColumn(
                name: "UserCreatorName",
                table: "Noticias");

            migrationBuilder.DropColumn(
                name: "UserUpdaterName",
                table: "Noticias");

            migrationBuilder.RenameColumn(
                name: "ImagesArray",
                table: "Noticias",
                newName: "imagesArray");
        }
    }
}
