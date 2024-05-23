using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ccytet.Server.Migrations
{
    /// <inheritdoc />
    public partial class _04 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Desde",
                table: "EstadosSituacionFinanciera");

            migrationBuilder.RenameColumn(
                name: "Titulo",
                table: "EstadosSituacionFinanciera",
                newName: "UserUpdaterName");

            migrationBuilder.RenameColumn(
                name: "Hasta",
                table: "EstadosSituacionFinanciera",
                newName: "Periodo");

            migrationBuilder.AddColumn<bool>(
                name: "Eliminado",
                table: "EstadosSituacionFinanciera",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "UserCreatorName",
                table: "EstadosSituacionFinanciera",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EstadoSituacionFinancieraArchivos",
                columns: table => new
                {
                    IdEstadoSituacionFinancieraArchivo = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaEliminación = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdEstadoSituacionFinanciera = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UserCreatorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IdUserCreator = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserDeleterId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IdUserDeleter = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadoSituacionFinancieraArchivos", x => x.IdEstadoSituacionFinancieraArchivo);
                    table.ForeignKey(
                        name: "FK_EstadoSituacionFinancieraArchivos_AspNetUsers_UserCreatorId",
                        column: x => x.UserCreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EstadoSituacionFinancieraArchivos_AspNetUsers_UserDeleterId",
                        column: x => x.UserDeleterId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_EstadoSituacionFinancieraArchivos_EstadosSituacionFinanciera_IdEstadoSituacionFinanciera",
                        column: x => x.IdEstadoSituacionFinanciera,
                        principalTable: "EstadosSituacionFinanciera",
                        principalColumn: "IdEstadoSituacionFinanciera",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EstadoSituacionFinancieraArchivos_IdEstadoSituacionFinanciera",
                table: "EstadoSituacionFinancieraArchivos",
                column: "IdEstadoSituacionFinanciera");

            migrationBuilder.CreateIndex(
                name: "IX_EstadoSituacionFinancieraArchivos_UserCreatorId",
                table: "EstadoSituacionFinancieraArchivos",
                column: "UserCreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_EstadoSituacionFinancieraArchivos_UserDeleterId",
                table: "EstadoSituacionFinancieraArchivos",
                column: "UserDeleterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EstadoSituacionFinancieraArchivos");

            migrationBuilder.DropColumn(
                name: "Eliminado",
                table: "EstadosSituacionFinanciera");

            migrationBuilder.DropColumn(
                name: "UserCreatorName",
                table: "EstadosSituacionFinanciera");

            migrationBuilder.RenameColumn(
                name: "UserUpdaterName",
                table: "EstadosSituacionFinanciera",
                newName: "Titulo");

            migrationBuilder.RenameColumn(
                name: "Periodo",
                table: "EstadosSituacionFinanciera",
                newName: "Hasta");

            migrationBuilder.AddColumn<DateTime>(
                name: "Desde",
                table: "EstadosSituacionFinanciera",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
