using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class VinculoUsuarioAlumnoPortal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AlumnoId",
                table: "Usuarios",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_AlumnoId",
                table: "Usuarios",
                column: "AlumnoId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Alumnos_AlumnoId",
                table: "Usuarios",
                column: "AlumnoId",
                principalTable: "Alumnos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Alumnos_AlumnoId",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_AlumnoId",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "AlumnoId",
                table: "Usuarios");
        }
    }
}
