using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ccytet.Server.Migrations
{
    /// <inheritdoc />
    public partial class _03 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FilesArray",
                table: "Convocatorias",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserCreatorName",
                table: "Convocatorias",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserUpdaterName",
                table: "Convocatorias",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FilesArray",
                table: "Convocatorias");

            migrationBuilder.DropColumn(
                name: "UserCreatorName",
                table: "Convocatorias");

            migrationBuilder.DropColumn(
                name: "UserUpdaterName",
                table: "Convocatorias");
        }
    }
}
